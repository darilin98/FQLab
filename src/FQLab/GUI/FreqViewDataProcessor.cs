using System.Numerics;
using Terminal.Gui.App;
using Terminal.Gui.ViewBase;

namespace FQLab;

public class FreqViewDataProcessor : IFreqDataReceiver
{
    private readonly FreqSpectrumView _spectrumView;
    private static readonly double LogMin = Math.Log10(20);
    private static readonly double LogMax = Math.Log10(20000);

    public FreqViewDataProcessor(FreqSpectrumView spectrumView)
    {
        _spectrumView = spectrumView;
    }
    
    public void ReceiveFrequencyData(FreqViewData viewData)
    {
        Application.Invoke(() => _spectrumView.UpdateData(CalculateLogFreqBuckets(viewData)));
    }

    private double[] CalculateLogFreqBuckets(FreqViewData viewData)
    {
        var result = new List<double>();
        int numGraphColumns = _spectrumView.Frame.Width;

        for (int i = 0; i < numGraphColumns; i++)
        {
            double freqStart = Math.Pow(10, LogMin + i * (LogMax - LogMin) / numGraphColumns);
            double freqEnd = Math.Pow(10, LogMin + (i + 1) * (LogMax - LogMin) / numGraphColumns);

            int fftSize = viewData.FftSize;
            int sampleRate = viewData.AudioFormat.SampleRate;
            int freqBinsCount = viewData.FreqBins.Length;

            int startBin = (int)(freqStart * fftSize / sampleRate);
            int endBin = Math.Min((int)(freqEnd * fftSize / sampleRate), freqBinsCount - 1);

            double mag = 0;
            for (int b = startBin; b <= endBin; b++)
            {
                mag += viewData.FreqBins[b].Magnitude;
            }
            
            result.Add(mag / (endBin - startBin + 1));
        }
        return result.ToArray();
    }
}