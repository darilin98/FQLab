using System.Runtime.InteropServices;

namespace FQLab;

public class AudioEngineFactory : IAudioEngineFactory
{
    public AudioEngine Create(IAudioStream audioStream)
    {
        IAudioPlayer? player;
        IFftProcessor fftProcessor = new MathNetFftProcessor();
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            player = new NAudioPlayer();
        }
        else
        {
            // Temporary until Linux/Mac player is implemented
            player = null;
        }

        return new AudioEngine(audioStream, player, fftProcessor);
    }
}