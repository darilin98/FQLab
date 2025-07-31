using Terminal.Gui.App;
using Terminal.Gui.ViewBase;

namespace FQLab;

public class FreqGraphView : View
{
    private double[] _magnitudes = [];
    private double[] _trails = [];
    
    private const double MinMagnitude = 1e-10;
    private const double DecayFactor = 0.99; // How much solid bars stick around
    private const double TrailDecay = 0.985; // How fast trails fall to the ground
    private double _smoothedMax = 1.0;

    /// <summary>
    /// Draws a bar graph in real time.
    /// </summary>
    /// <param name="magnitudes">Scalars for the length of individual columns.</param>
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
        
        if (_magnitudes.Length != Frame.Width || Frame.Height == 0 || Frame.Width == 0)
            return;
        
        SetNeedsDraw();
    }

    protected override bool OnDrawingContent(DrawContext? drawContext)
    {
        if (_magnitudes.Length == 0 || _smoothedMax <= 0)
            return true;
        
        int height = Frame.Height;
        int width = Frame.Width;

        if ( height == 0 || width == 0)
            return true;

        int safeLen = Math.Min(_magnitudes.Length, Frame.Width);
        
        for (int column = 0; column < safeLen; column++)
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