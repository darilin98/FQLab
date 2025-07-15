using NAudio.Wave;

namespace FQLab;

public class AudioStreamWrapper : IAudioStream
{
    public AudioFormat Format { get; }

    private AudioFileReader _reader;

    public AudioStreamWrapper(AudioFormat format, AudioFileReader reader)
    {
        Format = format;
        _reader = reader;
    }
    
    public AudioFrame? ReadFrame(int size)
    {
        throw new NotImplementedException();
    }
    
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}