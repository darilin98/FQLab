using System.Numerics;
using NAudio.Wave;

namespace FQLab;

public interface IAudioPlayer
{
    void Initialize(IAudioStream audioStream);
    void Play(AudioFrame audioFrame);
    void Pause();
    void Resume();
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
    public string Name { get; }
    void Process(ref float[] samples, AudioFormat format);
}

public interface IFreqDataReceiver
{
    void ReceiveFrequencyData(FreqViewData viewData);
}

public interface IAudioEngineFactory
{
    AudioEngineFactoryResult Create(IAudioStream audioStream, bool withDataExport = false, string? alternatePluginPath = null);
}