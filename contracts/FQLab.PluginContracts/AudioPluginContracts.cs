namespace FQLab.PluginContracts;

public interface IAudioPlugin
{
    public string Name { get; }
    void Process(ref float[] samples, AudioFormat format);
}

public record AudioFormat(int SampleRate, int ChannelCount);