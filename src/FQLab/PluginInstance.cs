namespace FQLab;

public class PluginInstance
{
    public bool Bypass { get; set; } = true;
    public IAudioPlugin Plugin { get; }
    public PluginInstance(IAudioPlugin plugin)
    {
        Plugin = plugin;
    }
}