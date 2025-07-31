using FQLab.PluginContracts;

namespace FQLab;
/// <summary>
/// Handles creation of audio stream from a file location.
/// </summary>
public static class InputHandler
{
    /// <summary>
    /// Attempts to open an audio stream
    /// </summary>
    /// <param name="audioFilePath">Path to file.</param>
    /// <param name="audioStream">Resulting stream reference if the operation succeeds.</param>
    /// <returns>Boolean value based on operation status.</returns>
    public static bool TryOpenAudioStream(string audioFilePath, out IAudioStream audioStream)
    {
        try
        {
            // Future plan to change Reader based on OS
            // Enabled by using IAudioReader
            using var reader = new NAudioReaderAdapter(audioFilePath);
            var format = new AudioFormat(reader.WaveFormat.SampleRate, reader.WaveFormat.Channels);

            audioStream = new AudioStreamWrapper(format, reader);
            return true;

        }
        catch (Exception)
        {
            audioStream = null;
            return false;
        }
    }
}