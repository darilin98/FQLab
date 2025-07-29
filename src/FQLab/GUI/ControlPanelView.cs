using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace FQLab;

public class ControlPanelView : FrameView
{
    public ControlPanelView(UIController controller)
    {
        Title = "Control Panel";

        var eq = new EqSelectorView(controller)
        {
            X = Pos.Left(this),
            Y = 1,
            Width = Dim.Percent(30),
            Height = Dim.Fill()
        };

        Add(eq);
    }
}