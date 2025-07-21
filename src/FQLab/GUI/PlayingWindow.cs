using Terminal.Gui.Views;

namespace FQLab;

public class PlayingWindow : Window
{
    private UIController _controller;
    public PlayingWindow(UIController controller, FreqSpectrumView freqSpectrumView)
    {
        Title = $"FQLab - now playing";

        _controller = controller;

        Add(freqSpectrumView);

    }
}