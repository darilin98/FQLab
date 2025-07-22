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
public class SineWaveStream : IAudioStream, IDisposable
{
    private double _phase = 0;
    private readonly float _frequency;
    private readonly float _sampleRate;
    private readonly int _channels;
    
    public AudioFormat Format { get; }

    public SineWaveStream(float frequency = 440f, float sampleRate = 44100f, int channels = 1)
    {
        _frequency = frequency;
        _sampleRate = sampleRate;
        _channels = channels;
        Format = new AudioFormat((int)sampleRate, channels);
    }

    public AudioFrame ReadFrame(int sampleCount)
    {
        var samples = new float[sampleCount * _channels];
        for (int i = 0; i < sampleCount; i++)
        {
            float sample = MathF.Sin((float)(2 * Math.PI * _frequency * _phase));
            _phase += 1.0 / _sampleRate;
            
            for (int ch = 0; ch < _channels; ch++)
                samples[i * _channels + ch] = sample;
        }

        return new AudioFrame(samples, Format);
    }

    public void Dispose()
    {
        // TODO release managed resources here
    }
}
