using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace FQLab;

public class PluginListView : FrameView
{
    private UIController _controller;
    
    public PluginListView(UIController controller)
    {
        Title = $"Installed Plugins";
        _controller = controller;

        var plugins = controller.GetPlugins();

        
        if (plugins is not null)
        {
            foreach (var plugin in plugins)
            {
                var controlBox = new Plugin(_controller, plugin)
                {
                    X = 0,
                    Y = 0,
                };
                Add(controlBox);
               
            }
        }
    }
}

public class Plugin : FrameView
{
    private UIController _controller;
    private PluginInstance _pluginInstance;
    public Plugin(UIController controller, PluginInstance pluginInstance)
    {
        _controller = controller;
        _pluginInstance = pluginInstance;
        
        Width = Dim.Fill();
        Height = 4;

        var label = new Label()
        {
            X = Pos.Center(),
            Y = 0,
            Text = pluginInstance.Plugin.Name
        };

        var checkbox = new CheckBox()
        {
            RadioStyle = true,
            X = Pos.Center(),
            Y = Pos.Bottom(label),
            CheckedState = CheckState.UnChecked,
            
        };

        checkbox.CheckedStateChanged += (s, e) =>
        {
            _pluginInstance.Bypass = !_pluginInstance.Bypass;
        };

        Add(label, checkbox);
    }
}