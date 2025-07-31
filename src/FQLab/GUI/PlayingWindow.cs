using Terminal.Gui.Drawing;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace FQLab;

/// <summary>
/// Window containing all views related to audio playback.
/// Its lifetime is identical to that of the stream and engine.
/// </summary>
public class PlayingWindow : Window
{
    private UIController _controller;
    public PlayingWindow(UIController controller, FreqSpectrumView freqSpectrumView)
    {

        _controller = controller;

        BorderStyle = LineStyle.None;
        
        freqSpectrumView.Width = Dim.Percent(80);
        freqSpectrumView.Height = Dim.Percent(75);

        var controlPanelView = new ControlPanelView(controller)
        {
            Y = Pos.Bottom(freqSpectrumView),
            Width = freqSpectrumView.Width,
            Height = Dim.Fill()
        };
        
        var pluginListView = new PluginListView(controller)
        {
            X = Pos.Right(freqSpectrumView),
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        Add(freqSpectrumView, controlPanelView, pluginListView);

    }
}