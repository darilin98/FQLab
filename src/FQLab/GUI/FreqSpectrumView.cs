using System.Numerics;
using MathNet.Numerics.Interpolation;
using NAudio.Wave.Compression;
using Terminal.Gui.App;
using Terminal.Gui.ViewBase;

namespace FQLab;

public class FreqSpectrumView : View
{

    private readonly FreqGraphView _graphView;
    private readonly FreqAxisView _axisView;
    public FreqSpectrumView()
    {
        Width = Dim.Fill();
        Height = Dim.Fill();

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

        Add(_graphView, _axisView);
    }
    public void UpdateData(double[] magnitudes)
    {
        _graphView.UpdateData(magnitudes);
        _axisView.UpdateLabels(Frame.Width);
    }
}