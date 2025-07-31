using FQLab.PluginContracts;

namespace FQLab;
/// <summary>
/// IAudioPlugin wrapper for enforcing bypass onto external plugins.
/// </summary>
public class PluginInstance
{
    /// <summary>
    /// Flag used to decide whether the plugin effect should be used in the chain.
    /// </summary>
    public bool Bypass { get; set; } = true;
    public IAudioPlugin Plugin { get; }
    public PluginInstance(IAudioPlugin plugin)
    {
        Plugin = plugin;
    }
}