using System.Drawing;
using System.Numerics;
using MathNet.Numerics.Interpolation;
using NAudio.Wave.Compression;
using Terminal.Gui.App;
using Terminal.Gui.Drawing;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;
using Color = Terminal.Gui.Drawing.Color;

namespace FQLab;

public class FreqSpectrumView : FrameView
{

    private readonly FreqGraphView _graphView;
    private readonly FreqAxisView _axisView;

    private bool _isReady = false;
    public FreqSpectrumView()
    {
        _graphView = new FreqGraphView()
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill() - 1
        };

        _axisView = new FreqAxisView()
        {
            X = 0,
            Y = Pos.Bottom(_graphView),
            Width = Dim.Fill(),
            Height = 1
        };

        Border.LineStyle = LineStyle.Rounded;

        Add(_graphView, _axisView);
    }
    
    protected override void OnFrameChanged(in Rectangle frame)
    {
        base.OnFrameChanged(frame);
        
        if (!_isReady && Frame.Width > 0 && Frame.Height > 0)
            _isReady = true;
    }
    
    public void UpdateData(double[] magnitudes)
    {
        if (_isReady)
        {
            _graphView.UpdateData(magnitudes);
            _axisView.UpdateLabels(Frame.Width);
        }
    }
    
}