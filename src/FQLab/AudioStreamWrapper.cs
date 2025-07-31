using FQLab.PluginContracts;

namespace FQLab;
/// <summary>
/// Wrapper class for the NAudio file reader.
/// Enables optional decoupling of NAudio from the project architecture.
/// </summary>
public class AudioStreamWrapper : IAudioStream
{
    public AudioFormat Format { get; }

    /// <summary>
    /// Internal NAudio provided reader.
    /// </summary>
    private readonly IAudioReader _reader;
    
    public AudioStreamWrapper(AudioFormat format, IAudioReader reader)
    {
        Format = format;
        _reader = reader;
    }
    
    /// <summary>
    /// Formats samples read by internal reader into consistent frames <see cref="AudioFrame"/>.
    /// </summary>
    /// <param name="size">Reads a frame of said size.</param>
    /// <returns><see cref="AudioFrame"/> of specified size.</returns>
    public AudioFrame? ReadFrame(int size)
    {
        int sampleCount = size * Format.ChannelCount;
        float[] buffer = new float[sampleCount];

        int samplesRead = _reader.Read(buffer, 0, sampleCount);
        
        if (samplesRead == 0)
        {
            return null;
        }
        
        if (samplesRead < sampleCount)
        {
            Array.Resize(ref buffer, sampleCount);
        }

        return new AudioFrame(buffer, Format);
    }
    
    public void Dispose()
    {
        _reader.Dispose();
    }
}