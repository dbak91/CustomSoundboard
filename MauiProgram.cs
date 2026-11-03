using CustomSoundboard.Services;

namespace CustomSoundboard;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.UseMauiApp<App>();

        builder.Services.AddSingleton<ISoundPlayer, AndroidSoundPlayer>();
#if SOUNDBOARD_FULL
        builder.Services.AddSingleton<IAudioRecorder, AndroidAudioRecorder>();
#endif
        builder.Services.AddSingleton<ISpeechSynthesizer, AndroidSpeechSynthesizer>();
        builder.Services.AddSingleton<MainPage>();

        return builder.Build();
    }
}
