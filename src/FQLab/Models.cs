namespace FQLab;

public record AudioFormat(int SampleRate, int ChannelCount);

public record AudioFrame(float[] Samples);


