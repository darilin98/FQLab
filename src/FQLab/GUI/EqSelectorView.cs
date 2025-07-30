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

    private EqColumn[] _columns;
    
    public EqSelectorView(UIController controller)
    {
        _controller = controller;

        Width = Dim.Fill();
        Height = Dim.Fill();
        
        _columns = Enum.GetValues<EqRanges>()
            .Select(range => {
                var col = new EqColumn(range);
                
                col.ValChanged += AggregateEq;
                return col;
            })
            .ToArray();
        
        var container = new FrameView() {
            Title = "EQ",
            X = 0, Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        container.Add(_columns);
        
        foreach (var col in _columns)
        {
            col.X = Pos.Align(Alignment.Center);
            col.Y = 0;
        }

        Add(container);

    }

    private void AggregateEq()
    {
        _controller.UpdateEq(new EqSettings()
        {
            Lows = _columns[(int)EqRanges.Lows].Value, 
            Mids = _columns[(int)EqRanges.Mids].Value,
            Highs = _columns[(int)EqRanges.Highs].Value
        });
        
    }
}