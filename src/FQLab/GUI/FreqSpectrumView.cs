using System.Numerics;
using NAudio.Wave.Compression;
using Terminal.Gui.App;
using Terminal.Gui.ViewBase;

namespace FQLab;

public class FreqSpectrumView : View
{

    private double[] _magnitudes = Array.Empty<double>();
    public void UpdateData(double[] magnitudes)
    {
        _magnitudes = magnitudes;
        SetNeedsDraw();
    }

    protected override bool OnDrawingContent(DrawContext? drawContext)
    {
        if (_magnitudes.Length == 0)
            return false;

        int height = Frame.Height;
        int width = Frame.Width;
        
        double max = _magnitudes.Max();

        if (max <= 0)
            return false;

        for (int column = 0; column < _magnitudes.Length && column < width; column++)
        {
            double val = _magnitudes[column] / max;
            int blocks = (int)(val * height);
            blocks = Math.Clamp(blocks, 0, height);

            int startRow = height - blocks;

            for (int row = startRow; row < height; row++)
            {
                Move(column, row);
                Application.Driver.AddRune('\u2588');
            }
        }
        return false;
    }
}