using System.Drawing;
using System.Numerics;
using MathNet.Numerics.Interpolation;
using NAudio.Wave.Compression;
using Terminal.Gui.App;
using Terminal.Gui.Drawing;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace FQLab;

public class FreqSpectrumView : FrameView
{

    private readonly FreqGraphView _graphView;
    private readonly FreqAxisView _axisView;

    private bool _isReady = false;
    public FreqSpectrumView()
    {
        Width = Dim.Percent(90);
        Height = Dim.Percent(75);

        _graphView = new FreqGraphView();
        _axisView = new FreqAxisView();
        
        _graphView.X = 0;
        _graphView.Y = 0;
        _graphView.Width = Dim.Fill();
        _graphView.Height = Dim.Fill() - 1;

        _axisView.X = 0;
        _axisView.Y = Pos.Bottom(_graphView);
        _axisView.Width = Dim.Fill();
        _axisView.Height = 1;

        Border.LineStyle = LineStyle.Dotted;

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