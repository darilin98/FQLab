namespace FQLab;

public class LoggerAudioPlayer : IAudioPlayer
{
    public void Initialize(IAudioStream audioStream)
    {
        return;
    }

    public void Play(AudioFrame audioFrame)
    {
        foreach (var sample in audioFrame.Samples)
        {
            Console.WriteLine(sample);
        }
    }
}

public static class Logger
{
    public static void Log(string message)
    {
        File.AppendAllText("debug.log", $"[{DateTime.Now}] {message} \n");
    }
}