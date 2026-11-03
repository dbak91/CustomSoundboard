using System.IO.Compression;
using SherpaOnnx;

namespace CustomSoundboard.Services;

// AI voices (Piper) that are packaged inside the app, so they work offline. Only the voice in use is kept in memory.
//   BritishFemale: southern_english_female   BritishMale: northern_english_male   IrishFemale: jenny_dioco
public sealed class NeuralVoice : INeuralVoice
{
    private const string AssetFolder = "voices";
    private const string ReadyMarker = "ready-v5";

    private readonly SemaphoreSlim _gate = new(1, 1);
    private OfflineTts? _tts;
    private NeuralVoiceKind? _loaded;

    private static string FileName(NeuralVoiceKind voice) => voice switch
    {
        NeuralVoiceKind.BritishFemale => "british-female",
        NeuralVoiceKind.BritishMale => "british-male",
        _ => "irish-female",
    };

    public async Task SynthesizeAsync(NeuralVoiceKind voice, string text, string filePath)
    {
        await _gate.WaitAsync();
        try
        {
            if (_tts is null || _loaded != voice)
            {
                _tts?.Dispose();
                _tts = null;
                _tts = await CreateAsync(voice);
                _loaded = voice;
            }

            var tts = _tts;
            var audio = await Task.Run(() => tts.Generate(text, 1.0f, 0));
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

    private static async Task<OfflineTts> CreateAsync(NeuralVoiceKind voice)
    {
        var name = FileName(voice);
        var root = Path.Combine(FileSystem.AppDataDirectory, AssetFolder);
        await UnpackAsync(root, name);

        var config = new OfflineTtsConfig();
        config.Model.Vits.Model = Path.Combine(root, $"{name}.onnx");
        config.Model.Vits.Tokens = Path.Combine(root, $"{name}-tokens.txt");
        config.Model.Vits.DataDir = Path.Combine(root, "espeak-ng-data");
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

        foreach (var file in new[] { $"{name}.onnx", $"{name}-tokens.txt" })
        {
            await using var source = await FileSystem.OpenAppPackageFileAsync($"{AssetFolder}/{file}");
            await using var target = File.Create(Path.Combine(root, file));
            await source.CopyToAsync(target);
        }

        await File.WriteAllTextAsync(voiceMarker, ReadyMarker);
    }
}
