using System.Reflection;

namespace FQLab;

public static class PluginLoader
{
    public static List<IAudioPlugin> LoadPlugins(string pluginSourcePath)
    {
        var plugins = new List<IAudioPlugin>();

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
                        plugins.Add(plugin);
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