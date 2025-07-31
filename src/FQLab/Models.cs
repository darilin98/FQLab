using System.Numerics;
using FQLab.PluginContracts;

namespace FQLab;

public record AudioFrame(float[] Samples, AudioFormat Format);

public record FreqViewData(Complex[] FreqBins, AudioFormat AudioFormat, int FftSize);

public record AudioEngineFactoryResult(AudioEngine AudioEngine, FreqSpectrumView? View = null);


