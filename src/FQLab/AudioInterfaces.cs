using System.Numerics;
using NAudio.Wave;

namespace FQLab;

interface IAudioPlayer
{
    
}

interface IFftProcessor
{
    
}

interface IAudioSource
{
    
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