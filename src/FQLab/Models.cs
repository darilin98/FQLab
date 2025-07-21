using System.Numerics;

namespace FQLab;

public record AudioFormat(int SampleRate, int ChannelCount);

public record AudioFrame(float[] Samples, AudioFormat Format);

public record FreqViewData(Complex[] FreqBins, AudioFormat AudioFormat, int FftSize);


