using System.IO.Compression;
using SherpaOnnx;

namespace CustomSoundboard.Services;

// AI voices (Piper) that are packaged inside the app, so they work offline. Only the voice in use is kept in memory.
//   BritishFemale: Piper cori (medium).
//   BritishMale and AmericanFemale: Kokoro (int8) speakers bm_george and af_bella, one shared model.
public sealed class NeuralVoice : INeuralVoice
{
    private const string AssetFolder = "voices";
    private const string ReadyMarker = "ready-v7";

    // Speaker numbers in kokoro-int8-en-v0_19 (voices.bin).
    private const int KokoroAmericanFemale = 1; // af_bella
    private const int KokoroBritishMale = 9;    // bm_george

    private readonly SemaphoreSlim _gate = new(1, 1);
    private OfflineTts? _tts;
    private string? _loaded;

    private static string FileName(NeuralVoiceKind voice) => voice switch
    {
        NeuralVoiceKind.BritishFemale => "british-female",
        _ => "kokoro",
    };

    private static int SpeakerId(NeuralVoiceKind voice) => voice switch
    {
        NeuralVoiceKind.BritishMale => KokoroBritishMale,
        NeuralVoiceKind.AmericanFemale => KokoroAmericanFemale,
        _ => 0,
    };

    public async Task SynthesizeAsync(NeuralVoiceKind voice, string text, string filePath)
    {
        await _gate.WaitAsync();
        try
        {
            var name = FileName(voice);
            if (_tts is null || _loaded != name)
            {
                _tts?.Dispose();
                _tts = null;
                _tts = await CreateAsync(name);
                _loaded = name;
            }

            var tts = _tts;
            var speaker = SpeakerId(voice);
            var audio = await Task.Run(() => tts.Generate(text, 1.0f, speaker));
            if (audio.Samples.Length == 0 || !audio.SaveToWaveFile(filePath))
            {
                throw new InvalidOperationException("The voice could not make speech from these words.");
            }
        }
        finally
        {
            _gate.Release();
        }
    }

    private static async Task<OfflineTts> CreateAsync(string name)
    {
        var root = Path.Combine(FileSystem.AppDataDirectory, AssetFolder);
        await UnpackAsync(root, name);

        var config = new OfflineTtsConfig();
        if (name == "kokoro")
        {
            config.Model.Kokoro.Model = Path.Combine(root, "kokoro.onnx");
            config.Model.Kokoro.Voices = Path.Combine(root, "kokoro-voices.bin");
            config.Model.Kokoro.Tokens = Path.Combine(root, "kokoro-tokens.txt");
            config.Model.Kokoro.DataDir = Path.Combine(root, "espeak-ng-data");
        }
        else
        {
            config.Model.Vits.Model = Path.Combine(root, $"{name}.onnx");
            config.Model.Vits.Tokens = Path.Combine(root, $"{name}-tokens.txt");
            config.Model.Vits.DataDir = Path.Combine(root, "espeak-ng-data");
        }

        config.Model.NumThreads = 2;
        return await Task.Run(() => new OfflineTts(config));
    }

    // The speech engine needs real files, so each voice is copied from the app package to the app's own storage once.
    private static async Task UnpackAsync(string root, string name)
    {
        Directory.CreateDirectory(root);

        var dataMarker = Path.Combine(root, $"espeak-{ReadyMarker}");
        if (!File.Exists(dataMarker))
        {
            var data = Path.Combine(root, "espeak-ng-data");
            if (Directory.Exists(data))
            {
                Directory.Delete(data, true);
            }

            await using (var zipSource = await FileSystem.OpenAppPackageFileAsync($"{AssetFolder}/espeak-ng-data.zip"))
            using (var archive = new ZipArchive(zipSource))
            {
                archive.ExtractToDirectory(root, true);
            }

            await File.WriteAllTextAsync(dataMarker, ReadyMarker);
        }

        var voiceMarker = Path.Combine(root, $"{name}-{ReadyMarker}");
        if (File.Exists(voiceMarker))
        {
            return;
        }

        var files = name == "kokoro"
            ? new[] { "kokoro.onnx", "kokoro-voices.bin", "kokoro-tokens.txt" }
            : [$"{name}.onnx", $"{name}-tokens.txt"];
        foreach (var file in files)
        {
            await using var source = await FileSystem.OpenAppPackageFileAsync($"{AssetFolder}/{file}");
            await using var target = File.Create(Path.Combine(root, file));
            await source.CopyToAsync(target);
        }

        await File.WriteAllTextAsync(voiceMarker, ReadyMarker);
    }
}
