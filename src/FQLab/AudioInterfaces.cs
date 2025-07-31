using System.Numerics;
using NAudio.Wave;
using FQLab.PluginContracts;

namespace FQLab;

/// <summary>
/// Final component of the audio pipeline. Responsible for playing samples on an audio source.
/// </summary>
public interface IAudioPlayer
{
    void Initialize(IAudioStream audioStream);
    /// <summary>
    /// Receives the AudioFrame and plays it.
    /// </summary>
    /// <param name="audioFrame">Frame with PCM samples.</param>
    void Play(AudioFrame audioFrame);
    /// <summary>
    /// Pauses playback.
    /// </summary>
    void Pause();
    /// <summary>
    /// Resumes playback
    /// </summary>
    void Resume();
}

/// <summary>
/// Provides methods for FFT operations.
/// </summary>
public interface IFftProcessor
{
    /// <summary>
    /// Performs forward FFT on an AudioFrame.
    /// </summary>
    /// <param name="audioFrame">PCM samples.</param>
    /// <returns>Frequency bins.</returns>
    Complex[] Forward(AudioFrame audioFrame);
    /// <summary>
    /// Performs inverse FFT on an array of frequency bins.
    /// </summary>
    /// <param name="freqBins">Frequency data.</param>
    /// <param name="format">Necessary stream metadata so the data can be reconstructed.</param>
    /// <returns>PCM samples.</returns>
    AudioFrame Inverse(Complex[] freqBins, AudioFormat format);
    
}

/// <summary>
/// Represents the decoded audio file. Enables on request fetching of data.
/// </summary>
public interface IAudioStream : IDisposable
{
    /// <summary>
    /// Stream metadata.
    /// </summary>
    AudioFormat Format { get; }
    /// <summary>
    /// Request PCM samples from the file of given size.
    /// </summary>
    /// <param name="size">Size of request.</param>
    /// <returns>PCM samples.</returns>
    AudioFrame? ReadFrame(int size);
}
/// <summary>
/// Abstracts readers to be used inside IAudioStream.
/// </summary>
public interface IAudioReader : ISampleProvider, IDisposable
{
    AudioFormat Format { get; }
}

/// <summary>
/// Enables the audio pipeline to export frequency data from within.
/// </summary>
public interface IFreqDataReceiver
{
    /// <summary>
    /// Used by pipeline after performing forward FFT. Data intended for graph visualizing.
    /// </summary>
    /// <param name="viewData"></param>
    void ReceiveFrequencyData(FreqViewData viewData);
}
/// <summary>
/// Factory interface that creates audio engines based on OS and AudioStream settings.
/// </summary>
public interface IAudioEngineFactory
{
    /// <summary>
    /// Creates an engine with a stream injected in order for their lifespans to match.
    /// </summary>
    /// <param name="audioStream">Stream of decoded file, ready for consumption.</param>
    /// <param name="withDataExport">Optional flag when user wants to use the engine with a view.</param>
    /// <param name="alternatePluginPath">Alternate location for sourcing plugins.</param>
    /// <returns>Audio engine and optional View</returns>
    AudioEngineFactoryResult Create(IAudioStream audioStream, bool withDataExport = false, string? alternatePluginPath = null);
}