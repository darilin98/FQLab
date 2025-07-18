using NAudio.Wave;

namespace FQLab;

public class AudioStreamWrapper : IAudioStream
{
    public AudioFormat Format { get; }

    private readonly IAudioReader _reader;

    public AudioStreamWrapper(AudioFormat format, IAudioReader reader)
    {
        Format = format;
        _reader = reader;
    }
    
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