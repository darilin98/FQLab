using Terminal.Gui.App;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace FQLab;

public class InputSelectWindow : Window
{
    private static string _testFilePath = "C:\\Users\\Darek\\MFF-PROJS\\FQLab\\testaudio\\lazychill.mp3";
    
    private UIController _controller;
    public InputSelectWindow(UIController controller)
    {
        Title = $"FQLab - The spectral playground";

        _controller = controller;

        var playBtn = new Button()
        {
            Text = "Play",
            Y = Pos.Center(),
            X = Pos.Center(),
            IsDefault = true
        };


        playBtn.Accepting += (s, e) =>
        {
            if (!_controller.TryPlayFile(_testFilePath))
            {
                MessageBox.ErrorQuery("Decoding file", "Error file could not be decoded", "OK");
            }

            e.Handled = true;
        };

        Add(playBtn);
    }
}