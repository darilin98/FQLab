using System.Numerics;
using NAudio.Wave.Compression;
using Terminal.Gui.ViewBase;

namespace FQLab;

public class FreqSpectrumView : View
{

    private double[] _magnitudes;
    public void UpdateData(double[] magnitudes)
    {
        _magnitudes = magnitudes;
        SetNeedsDraw();
    }

    protected override bool OnDrawingContent(DrawContext? drawContext)
    {
        return false;
    }
}