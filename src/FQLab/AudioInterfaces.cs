using System.Numerics;
using NAudio.Wave;

namespace FQLab;

interface IAudioPlayer
{
    public void Initialize(IAudioStream audioStream);
    public void Play(AudioFrame audioFrame);
}

interface IFftProcessor
{
    public Complex[] Forward(AudioFrame audioFrame);
    public AudioFrame Inverse(Complex[] freqBins, AudioFormat format);
    
}

public interface IAudioStream : IDisposable
{
    AudioFormat Format { get; }
    AudioFrame? ReadFrame(int size);
}

public interface IAudioReader : ISampleProvider, IDisposable
{
    AudioFormat Format { get; }
}
public interface IAudioPlugin
{
    
}

interface IFreqDataReceiver
{
    public void ReceiveFrequencyData(Complex[] freqBins);
}