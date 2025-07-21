using System.Numerics;
using NAudio.Wave;

namespace FQLab;

public interface IAudioPlayer
{
    void Initialize(IAudioStream audioStream);
    void Play(AudioFrame audioFrame);
}

public interface IFftProcessor
{
    Complex[] Forward(AudioFrame audioFrame);
    AudioFrame Inverse(Complex[] freqBins, AudioFormat format);
    
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

public interface IFreqDataReceiver
{
    void ReceiveFrequencyData(FreqViewData viewData);
}

public interface IAudioEngineFactory
{
    AudioEngine Create(IAudioStream audioStream);
}