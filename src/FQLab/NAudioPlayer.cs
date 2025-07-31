using NAudio.Wave;

namespace FQLab;

/// <summary>
/// Wrapper around the NAudio WaveOutEvent.
///
/// 
/// </summary>
public class NAudioPlayer : IAudioPlayer, IDisposable
{
    private NAudioBufferedProvider? _sampleProvider;
    private WaveOutEvent _outputDevice;
    
    /// <summary>
    /// Sets up internal settings based on metadata of the stream.
    /// </summary>
    /// <param name="audioStream">Initialized audio stream instance.</param>
    public void Initialize(IAudioStream audioStream)
    {
        var waveFormat =
            WaveFormat.CreateIeeeFloatWaveFormat(audioStream.Format.SampleRate, audioStream.Format.ChannelCount);
        _sampleProvider = new NAudioBufferedProvider(waveFormat);
        _outputDevice = new WaveOutEvent();
        _outputDevice.Init(_sampleProvider);
        _outputDevice.Play();
    }

    /// <summary>
    /// Feeds samples from the frame to the player.
    /// </summary>
    /// <param name="audioFrame">Pipeline processed frame ready for playback.</param>
    public void Play(AudioFrame audioFrame)
    {
        if (_sampleProvider is null)
            return;
        _sampleProvider.AddSamples(audioFrame.Samples);
    }

    /// <summary>
    /// Pauses internal device.
    /// </summary>
    public void Pause()
    {
        _outputDevice.Pause();
    }

    /// <summary>
    /// Resumes internal device.
    /// </summary>
    public void Resume()
    {
        _outputDevice.Play();
    }

    /// <summary>
    /// Clean up after track has finished playing.
    /// </summary>
    public void Dispose()
    {
        _sampleProvider.CompleteAdding();
        _outputDevice.Stop();
        _outputDevice.Dispose();
    }
}