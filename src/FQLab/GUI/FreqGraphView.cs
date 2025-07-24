using Terminal.Gui.App;
using Terminal.Gui.ViewBase;

namespace FQLab;

public class FreqGraphView : View
{
    private double[] _magnitudes = [];
    private double[] _trails = [];
    
    private const double MinMagnitude = 1e-10;
    private const double DecayFactor = 0.98;
    private const double TrailDecay = 0.90;
    private double _smoothedMax = 1.0;

    public void UpdateData(double[] magnitudes)
    {
        if (magnitudes.Length == 0)
            return;
        
        var frameMax = magnitudes.Max();
        if (double.IsNaN(frameMax) || double.IsInfinity(frameMax) || frameMax <= 0)
            return;

        _smoothedMax = Math.Max(frameMax, _smoothedMax * DecayFactor);
        
        if (_trails.Length != magnitudes.Length)
        {
            _trails = new double[magnitudes.Length];
        }
        
        _magnitudes = magnitudes;
        
        SetNeedsDraw();
    }

    protected override bool OnDrawingContent(DrawContext? drawContext)
    {
        if (_magnitudes.Length == 0 || _smoothedMax <= 0)
            return true;
        
        int height = Frame.Height;
        int width = Frame.Width;

        if (_magnitudes.Length != Frame.Width || height == 0 || width == 0)
            return true;

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
                if (column < 0 || column >= width || row < 0 || row >= height)
                    continue;
                Move(column, row);
                Application.Driver?.AddRune('\u2588'); // full block
            }
            
            _trails[column] = Math.Max(_trails[column] * TrailDecay, norm);
            int trailHeight = (int)(_trails[column] * height);
            trailHeight = Math.Clamp(trailHeight, 0, height);
            for (int row = height - trailHeight; row < height - blocks; row++)
            {
                if (column < 0 || column >= width || row < 0 || row >= height)
                    continue;
                Move(column, row);
                Application.Driver?.AddRune('\u2592'); // medium shade block for trail
            }
            
        }
        return true;
    }
}