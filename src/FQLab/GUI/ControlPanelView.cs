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
            Y = 0,
            Width = Dim.Percent(30),
            Height = Dim.Fill()
        };

        var pause = new Button()
        {
            Text = "||",
            X = Pos.Align(Alignment.Center),
            Y = Pos.Center(),
            Width = 8,
            Height = 2
        };
        
        var play = new Button()
        {
            Text = "\u25B6",
            X = Pos.Align(Alignment.Center),
            Y = Pos.Center(),
            Width = 8,
            Height = 2
        };

        var stop = new Button()
        {
            Text = "\u25A0",
            X = Pos.Align(Alignment.Center),
            Y = Pos.Center(),
            Width = 8,
            Height = 2
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

        var container = new FrameView()
        {
            X = Pos.Right(eq),
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill() - 2
        };

        container.Add(pause, play, stop);
     
        
        Add(eq, container);
    }
}