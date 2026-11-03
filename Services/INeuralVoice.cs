namespace CustomSoundboard.Services;

public interface INeuralVoice
{
    /// <summary>
    /// Speaks the text with an AI voice that is packaged inside the app and saves it as a WAV file at the given path.
    /// </summary>
    Task SynthesizeAsync(NeuralVoiceKind voice, string text, string filePath);
}

/// <summary>
/// The packaged AI voices. The name of each is also the name of its files under Resources/Voices.
/// </summary>
public enum NeuralVoiceKind
{
    BritishFemale,
    BritishMale,
    IrishFemale,
}
