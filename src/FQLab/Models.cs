using System.Numerics;
using FQLab.PluginContracts;

namespace FQLab;

/// <summary>
/// Couples together crucial audio stream metadata with float samples.
/// </summary>
/// <param name="Samples">PCM samples</param>
/// <param name="Format"><see cref="AudioFormat"/></param>
public record AudioFrame(float[] Samples, AudioFormat Format);

/// <summary>
/// Holds results of FFT. Data about current frequency distribution of the audio stream.
/// </summary>
/// <param name="FreqBins">Result of FFT.</param>
/// <param name="AudioFormat"><see cref="AudioFormat"/></param>
/// <param name="FftSize">Resolution of the FFT used.</param>
public record FreqViewData(Complex[] FreqBins, AudioFormat AudioFormat, int FftSize);

/// <summary>
/// Response of the engine factory. Used to return a tailored audio engine as well as an optional graph view.
/// </summary>
/// <param name="AudioEngine">User settings specific engine.</param>
/// <param name="View">Optional graph view.</param>
public record AudioEngineFactoryResult(AudioEngine AudioEngine, FreqSpectrumView? View = null);


