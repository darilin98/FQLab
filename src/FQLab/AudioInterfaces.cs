using System.Numerics;

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

public interface IAudioPlugin
{
    
}

interface IFreqDataReceiver
{
    public void ReceiveFrequencyData(Complex[] freqBins);
}