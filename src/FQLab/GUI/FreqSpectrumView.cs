using System.Numerics;
using MathNet.Numerics.Interpolation;
using NAudio.Wave.Compression;
using Terminal.Gui.App;
using Terminal.Gui.ViewBase;

namespace FQLab;

public class FreqSpectrumView : View
{

    private double[] _magnitudes = [];

    private const double MinMagnitude = 1e-10;
    private const double DecayFactor = 0.98;
    private double _smoothedMax = 1.0;
    public void UpdateData(double[] magnitudes)
    {
        if (magnitudes.Length == 0)
            return;
        
        var frameMax = magnitudes.Max();
        if (double.IsNaN(frameMax) || double.IsInfinity(frameMax) || frameMax <= 0)
            return;

        _smoothedMax = Math.Max(frameMax, _smoothedMax * DecayFactor);
        
        _magnitudes = magnitudes;
        
        SetNeedsDraw();
    }

    protected override bool OnDrawingContent(DrawContext? drawContext)
    {
        if (_magnitudes.Length == 0 || _smoothedMax <= 0)
            return false;
        
        int height = Frame.Height;
        int width = Frame.Width;

        for (int column = 0; column < _magnitudes.Length && column < width; column++)
        {
            double magnitude = _magnitudes[column];

            if (double.IsNaN(magnitude) || double.IsInfinity(magnitude) || magnitude <= 0)
                continue;

            double norm = magnitude / (_smoothedMax + MinMagnitude);
            norm = Math.Clamp(norm, 0, 1);
            norm = Math.Pow(norm, 0.8); 

            int blocks = (int)(norm * height);
            blocks = Math.Clamp(blocks, 0, height);
            int startRow = height - blocks;

            for (int row = startRow; row < height; row++)
            {
                Move(column, row);
                Application.Driver.AddRune('\u2588');
            }
        }
        return true;
    }
}