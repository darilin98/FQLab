using FQLab.PluginContracts;
using NAudio.Wave;

namespace FQLab;

public class NAudioReaderAdapter : IAudioReader
{
    private readonly AudioFileReader _reader;
    
    public AudioFormat Format { get; }

    public NAudioReaderAdapter(string filePath)
    {
        _reader = new AudioFileReader(filePath);
        Format = new AudioFormat(_reader.WaveFormat.SampleRate, _reader.WaveFormat.Channels);
    }

    public int Read(float[] buffer, int offset, int count)
    {
        return _reader.Read(buffer, offset, count);
    }

    public WaveFormat WaveFormat => _reader.WaveFormat;
    
    public void Dispose()
    {
        _reader.Dispose();
    }
    
}