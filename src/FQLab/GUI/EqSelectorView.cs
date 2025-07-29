using System.ComponentModel;
using Terminal.Gui.App;
using Terminal.Gui.Configuration;
using Terminal.Gui.Drawing;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace FQLab;

public class EqSelectorView : View
{
    private UIController _controller;
    
    public EqSelectorView(UIController controller)
    {
        _controller = controller;

        Width = Dim.Fill();
        Height = Dim.Fill();
        
        var columns = Enum.GetValues<EqRanges>()
            .Select(range => {
                var col = new EqColumn(range);
                
                //col.ValChanged += (r, v) => controller.UpdateEqBand(r, v);
                return col as View;
            })
            .ToArray();
        
        var container = new FrameView() {
            Title = "EQ Controls",
            X = 0, Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        container.Add(columns);
        
        foreach (var col in columns)
        {
            col.X = Pos.Align(Alignment.Center, AlignmentModes.AddSpaceBetweenItems);
            col.Y = Pos.Percent(15);
        }

        Add(container);

    }
}