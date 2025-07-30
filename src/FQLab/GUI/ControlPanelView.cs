using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace FQLab;

public class ControlPanelView : FrameView
{
    private UIController _controller;
    public ControlPanelView(UIController controller)
    {
        Title = "Control Panel";

        _controller = controller;

        var eq = new EqSelectorView(controller)
        {
            X = Pos.Left(this),
            Y = 1,
            Width = Dim.Percent(30),
            Height = Dim.Fill()
        };

        var pause = new Button()
        {
            Text = "Pause",
            X = Pos.Right(eq),
            Width = Dim.Percent(10),
            Height = Dim.Fill()
        };
        
        var play = new Button()
        {
            Text = "Play",
            X = Pos.Right(pause),
            Width = Dim.Percent(10),
            Height = Dim.Fill()
        };

        var stop = new Button()
        {
            Text = "Stop",
            X = Pos.Right(play),
            Width = Dim.Percent(10),
            Height = Dim.Fill()
        };

        pause.Accepting += (s, e) =>
        {
            _controller.PauseTrack();
            e.Handled = true;
        };
        
        play.Accepting += (s, e) =>
        {
            _controller.ResumeTrack();
            e.Handled = true;
        };
        
        stop.Accepting += (s, e) =>
        {
            _controller.KillTrack();
            e.Handled = true;
        };

        Add(eq, pause, play, stop);
    }
}