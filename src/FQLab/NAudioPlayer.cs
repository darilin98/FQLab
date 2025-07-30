using NAudio.Wave;

namespace FQLab;

public class NAudioPlayer : IAudioPlayer, IDisposable
{
    private NAudioBufferedProvider? _sampleProvider;
    private WaveOutEvent _outputDevice;
    
    public void Initialize(IAudioStream audioStream)
    {
        var waveFormat =
            WaveFormat.CreateIeeeFloatWaveFormat(audioStream.Format.SampleRate, audioStream.Format.ChannelCount);
        _sampleProvider = new NAudioBufferedProvider(waveFormat);
        _outputDevice = new WaveOutEvent();
        _outputDevice.Init(_sampleProvider);
        _outputDevice.Play();
    }

    public void Play(AudioFrame audioFrame)
    {
        if (_sampleProvider is null)
            return;
        _sampleProvider.AddSamples(audioFrame.Samples);
    }

    public void Pause()
    {
        _outputDevice.Pause();
    }

    public void Resume()
    {
        _outputDevice.Play();
    }

    public void Dispose()
    {
        _sampleProvider.CompleteAdding();
        _outputDevice.Stop();
        _outputDevice.Dispose();
    }
}