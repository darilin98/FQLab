using System.Collections.Concurrent;
using NAudio.Wave;

namespace FQLab;

/// <summary>
/// Custom buffer for the NAudioPlayer
///
/// Used to avoid conversion from float samples to bytes
/// </summary>
public class NAudioBufferedProvider : ISampleProvider
{
    /// <summary>
    /// Format of the audio stream.
    /// </summary>
    public WaveFormat WaveFormat { get; }
    
    /// <summary>
    /// Internal buffer which stores samples ready to be played by NAudioPlayer.
    /// </summary>
    private BlockingCollection<float> _sampleBuffer = new(128);

    public NAudioBufferedProvider(WaveFormat format)
    {
        WaveFormat = format;
    }

    /// <summary>
    /// Allows external pipeline to add processed samples in queue for playback.
    /// </summary>
    /// <param name="samples"></param>
    public void AddSamples(float[] samples)
    {
        foreach (var sample in samples)
        {
            _sampleBuffer.Add(sample);
        }
    }
    
    /// <summary>
    /// Gets data from the internal buffer based on the contract provided by NAudio
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    /// <returns>Number of samples read.</returns>
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