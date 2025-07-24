using Terminal.Gui.Drawing;
using Terminal.Gui.Views;

namespace FQLab;

public class PlayingWindow : Window
{
    private UIController _controller;
    public PlayingWindow(UIController controller, FreqSpectrumView freqSpectrumView)
    {

        _controller = controller;

        BorderStyle = LineStyle.None;

        Add(freqSpectrumView);

    }
}