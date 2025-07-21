using System.Numerics;

namespace FQLabTests;

using FQLab;

internal class MockAudioStream : IAudioStream
{
    private int readCounter = 0;

    public int TotalSamples { get; }

    public MockAudioStream(int totalSamples)
    {
        TotalSamples = totalSamples;
    }
    public void Dispose() { }

    public AudioFormat Format => new AudioFormat(44100, 2);
    public AudioFrame? ReadFrame(int size)
    {
        if (readCounter < TotalSamples)
        {
            readCounter++;
            return new AudioFrame(new float[size * Format.ChannelCount], Format);
        }
        return null;
    }
}

internal class MockAudioPlayer : IAudioPlayer
{
    public void Initialize(IAudioStream audioStream) { }

    public List<AudioFrame> Result { get; } = new List<AudioFrame>();
    public void Play(AudioFrame audioFrame)
    {
        Result.Add(audioFrame);
    }
}

internal class MockFftProcessor : IFftProcessor
{
    public Complex[] Forward(AudioFrame audioFrame)
    {
        return new Complex[audioFrame.Samples.Length];
    }

    public AudioFrame Inverse(Complex[] freqBins, AudioFormat format)
    {
        return new AudioFrame(new float[freqBins.Length], format);
    }
}

internal class MockFreqDataReceiver : IFreqDataReceiver
{
    public List<Complex[]> Result { get; } = new();
    public void ReceiveFrequencyData(FreqViewData viewData)
    {
        Result.Add(viewData.FreqBins);
    }
}

public class AudioEngineTests
{
    [Theory]
    [InlineData(8)]
    [InlineData(64)]
    [InlineData(2048)]
    public async Task PlaysCorrectNumberOfFrames(int sampleCount)
    {
        // Arrange
        var player = new MockAudioPlayer();
        var stream = new MockAudioStream(sampleCount);
        var engine = new AudioEngine(stream, player, new MockFftProcessor());
        
        // Act
        engine.Run();
        await Task.WhenAll(engine.Tasks);

        // Assert
        Assert.Equal(stream.TotalSamples, player.Result.Count);

    }

    [Fact]

    public async Task ExportsDataToReceiver()
    {
        // Arrange
        var player = new MockAudioPlayer();
        var stream = new MockAudioStream(4096);
        var receiver = new MockFreqDataReceiver();
        var engine = new AudioEngine(stream, player, new MathNetFftProcessor(), receiver);
        
        // Act
        engine.Run();
        await Task.WhenAll(engine.Tasks);

        // Assert
        Assert.Equal(4096, player.Result.Count); 
        Assert.Equal(4096, receiver.Result.Count);

        Assert.All(receiver.Result, bins =>
        {
            Assert.NotNull(bins);
            Assert.Equal(2 * 1024, bins.Length); 
        });
    }
}