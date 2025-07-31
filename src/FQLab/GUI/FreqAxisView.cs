using Terminal.Gui.App;
using Terminal.Gui.ViewBase;

namespace FQLab;

public class FreqAxisView : View
{
    private readonly List<(int column, string label)> _labels = [];

    public void UpdateLabels(int currentWidth)
    {
        _labels.Clear();
        // List of all frequencies to be displayed under graph.
        var frequencies = new[] { 20, 35, 65, 130, 250, 500, 1000, 2000, 4000, 10000 };

        foreach (var freq in frequencies)
        {
            // Calculate frequencies to fit with logarithmic graph representation.
            double position = (Math.Log10(freq) - Math.Log10(20)) / (Math.Log10(20000) - Math.Log10(20));
            int col = (int)(position * Frame.Width);
            _labels.Add((col, freq >= 1000 ? $"{freq / 1000}k" : freq.ToString()));
        }
        if (Frame.Height == 0 || Frame.Width == 0)
            return;
        
        SetNeedsDraw();
    }

    protected override bool OnDrawingContent(DrawContext? drawContext)
    {
        base.OnDrawingContent(drawContext);
        
        if (Frame.Width == 0)
            return true;
        
        foreach (var (col, label) in _labels)
        {
            if (col + label.Length < Frame.Width)
                Move(col, 0);
            Application.Driver?.AddStr(label);
        }

        return true;
    }
}