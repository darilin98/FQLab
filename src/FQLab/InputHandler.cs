namespace FQLab;

using NAudio.Wave;

public static class InputHandler
{
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