# Creating a Plugin

If you are a developer looking to create your own plugin for the app the process is quite simple.

Create a project (class library) in any location but make sure to reference the `FQLab.PluginContracts` project from it.

This will allow you to put `using FQLab.PluginContracts` in your file and start your development.

## The contract

The referenced project gives you acces to an interface.

```csharp
public interface IAudioPlugin {
    void Process(ref float[] samples, AudioFormat format);
    ...
}
```

Make your new custom implement this interface and create the *process* method to your liking.

The `float[]` samples are normalized PCM data.

## Importing the plugin 

Once you finish your plugin, build it and put the `.dll` file in the `/plugins` directory in the cloned repo.

From there the plugin will be automatically registered and ready to be used!

