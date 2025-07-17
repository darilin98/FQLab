using System.Collections.Concurrent;
using NAudio.Wave;

namespace FQLab;

public class NAudioBufferedProvider : ISampleProvider
{
    public WaveFormat WaveFormat { get; }

    private BlockingCollection<float> _sampleBuffer = new(128);

    public NAudioBufferedProvider(WaveFormat format)
    {
        WaveFormat = format;
    }

    public void AddSamples(float[] samples)
    {
        foreach (var sample in samples)
        {
            _sampleBuffer.Add(sample);
        }
    }
    
    public int Read(float[] buffer, int offset, int count)
    {
        int read = 0;
        while (read < count)
        {
            if (_sampleBuffer.TryTake(out float sample, TimeSpan.FromMilliseconds(100)))
            {
                buffer[offset + read] = sample;
                read++;
            }
            else
            {
                break;
            }
        }

        return read;
    }

    public void CompleteAdding() => _sampleBuffer.CompleteAdding();

}