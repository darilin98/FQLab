using System.Runtime.InteropServices;
using Terminal.Gui.ViewBase;

namespace FQLab;

public class AudioEngineFactory : IAudioEngineFactory
{
    public AudioEngineFactoryResult Create(IAudioStream audioStream, bool withDataExport = false)
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

        if (withDataExport)
        {
            var view = new FreqSpectrumView() { Width = Dim.Fill(),
                Height = Dim.Fill(),};
            IFreqDataReceiver receiver = new FreqViewDataProcessor(view);
            return new AudioEngineFactoryResult(new AudioEngine(audioStream, player, fftProcessor, receiver), view);
        }
        
        return new AudioEngineFactoryResult(new AudioEngine(audioStream, player, fftProcessor));
    }
}