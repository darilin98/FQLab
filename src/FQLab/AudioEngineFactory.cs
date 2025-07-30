using System.Runtime.InteropServices;
using Terminal.Gui.ViewBase;

namespace FQLab;

public class AudioEngineFactory : IAudioEngineFactory
{
    private string _defaultPluginPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Plugins"));
    public AudioEngineFactoryResult Create(IAudioStream audioStream, bool withDataExport = false, string? alternatePluginPath = null)
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
        
        if (alternatePluginPath is not null)
            _defaultPluginPath = alternatePluginPath;

        var plugins = PluginLoader.LoadPlugins(_defaultPluginPath);

        if (withDataExport)
        {
            var view = new FreqSpectrumView();
            IFreqDataReceiver receiver = new FreqViewDataProcessor(view);
            return new AudioEngineFactoryResult(new AudioEngine(audioStream, player, fftProcessor, receiver, plugins), view);
        }
        
        return new AudioEngineFactoryResult(new AudioEngine(audioStream, player, fftProcessor, audioPlugins: plugins));
    }
}