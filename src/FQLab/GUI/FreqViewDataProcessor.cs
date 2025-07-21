using System.Numerics;
using Terminal.Gui.ViewBase;

namespace FQLab;

public class FreqViewDataProcessor : IFreqDataReceiver
{
    private readonly FreqSpectrumView _spectrumView;
    
    public FreqViewDataProcessor(FreqSpectrumView spectrumView)
    {
        _spectrumView = spectrumView;
    }
    
    public void ReceiveFrequencyData(FreqViewData viewData)
    {
        _spectrumView.UpdateData(CalculateLogFreqBuckets(viewData));
    }

    private double[] CalculateLogFreqBuckets(FreqViewData viewData)
    {
        var result = new List<double>();
        return result.ToArray();
    }
}