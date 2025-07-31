using System.Reflection;
using FQLab.PluginContracts;

namespace FQLab;
/// <summary>
/// Static helper used for extraction of audio plugins (IAudioPlugin objects).
/// </summary>
public static class PluginLoader
{
    /// <summary>
    /// Scans the specified directory for .dll files.
    /// </summary>
    /// <param name="pluginSourcePath">Directory in which plugin .dll files should be located.</param>
    /// <returns>Returns a List of initiated objects found in the files.</returns>
    public static List<PluginInstance> LoadPlugins(string pluginSourcePath)
    {
        var plugins = new List<PluginInstance>();

        if (!Directory.Exists(pluginSourcePath))
            return plugins;

        foreach (var dll in Directory.GetFiles(pluginSourcePath, "*.dll"))
        {
            try
            {
                var assembly = Assembly.LoadFrom(dll);

                var types = assembly.GetTypes()
                    .Where(t => typeof(IAudioPlugin).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass);

                foreach (var t in types)
                {
                    if (Activator.CreateInstance(t) is IAudioPlugin plugin)
                    {
                        plugins.Add(new PluginInstance(plugin));
                    }
                    
                }
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to load plugin from {dll}: {e.Message}");
            }
        }
        
        return plugins;
    }
}