using Terminal.Gui.Views;

namespace FQLab;

public class PluginListView : FrameView
{
    private UIController _controller;
    
    public PluginListView(UIController controller)
    {
        Title = $"Installed Plugins";
        _controller = controller;
    }
}