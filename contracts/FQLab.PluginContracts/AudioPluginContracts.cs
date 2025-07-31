namespace FQLab.PluginContracts;

/// <summary>
/// Defines the contract for third party plugins
/// </summary>
public interface IAudioPlugin
{
    /// <summary>
    /// Gets the display name of the plugin
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// Passes a reference to the current set of samples for the plugin to modify. 
    /// </summary>
    /// <param name="samples">Audio samples to process. To be modified in-place.</param>
    /// <param name="format">Exposes the metadata of the stream that is providing samples.</param>
    void Process(ref float[] samples, AudioFormat format);
}
/// <summary>
/// Couples together crucial metadata about the audio stream.
/// </summary>
/// <param name="SampleRate">Number of samples per second.</param>
/// <param name="ChannelCount">Number of audio channels (e.g. 1 for mono, 2 for stereo).</param>
public record AudioFormat(int SampleRate, int ChannelCount);