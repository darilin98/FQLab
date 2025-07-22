using System.Numerics;
using Terminal.Gui.App;
using Terminal.Gui.ViewBase;

namespace FQLab;

public class FreqViewDataProcessor : IFreqDataReceiver
{
    private readonly FreqSpectrumView _spectrumView;
    private static readonly double LogMin = Math.Log10(20);
    private static readonly double LogMax = Math.Log10(20000);

    private readonly object magLock = new();
    private double[] _magnitudes = [];

    public FreqViewDataProcessor(FreqSpectrumView spectrumView)
    {
        _spectrumView = spectrumView;
        Application.AddTimeout(TimeSpan.FromMilliseconds(33), () =>
        {
            lock (magLock)
            {
                _spectrumView.UpdateData(_magnitudes);
            }
            return true;
        });
    }
    
    public void ReceiveFrequencyData(FreqViewData viewData)
    {
        lock (magLock)
        {
            _magnitudes = CalculateLogFreqBuckets(viewData);
        }
        
    }

    private int _cachedWidth = 0;
    private List<(int start, int end)> _binRanges = new();
    private double[] CalculateLogFreqBuckets(FreqViewData viewData)
    {
        int numGraphColumns = _spectrumView.Frame.Width;
        int fftSize = viewData.FftSize;
        int sampleRate = viewData.AudioFormat.SampleRate;
        int freqBinsCount = viewData.FreqBins.Length;
        
        if (_cachedWidth != numGraphColumns)
        {
            _cachedWidth = numGraphColumns;
            _binRanges.Clear();

            for (int i = 0; i < numGraphColumns; i++)
            {
                double freqStart = Math.Pow(10, LogMin + i * (LogMax - LogMin) / numGraphColumns);
                double freqEnd = Math.Pow(10, LogMin + (i + 1) * (LogMax - LogMin) / numGraphColumns);

                int startBin = (int)(freqStart * fftSize / sampleRate);
                int endBin = Math.Min((int)(freqEnd * fftSize / sampleRate), freqBinsCount - 1);

                _binRanges.Add((startBin, endBin));
            }
        }
        
        var result = new double[_binRanges.Count];
        for (int i = 0; i < _binRanges.Count; i++)
        {
            var (startBin, endBin) = _binRanges[i];
            double mag = 0;
            for (int b = startBin; b <= endBin; b++)
            {
                mag += viewData.FreqBins[b].Magnitude;
            }
            result[i] = mag / (endBin - startBin + 1);
        }
        return result.ToArray();
    }
}