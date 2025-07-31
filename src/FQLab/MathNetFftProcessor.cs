using System.Numerics;
using MathNet.Numerics.IntegralTransforms;
using FQLab.PluginContracts;

namespace FQLab;

/// <summary>
/// Wraps the FFT from Math.NET.
/// </summary>
public class MathNetFftProcessor : IFftProcessor
{
    /// <summary>
    /// Executes forward FFT on samples of size AudioFrame
    /// </summary>
    /// <param name="audioFrame">Defined in: <see cref="AudioFrame"/></param>
    /// <returns>Array of complex numbers representing frequency bins.</returns>
    public Complex[] Forward(AudioFrame audioFrame)
    {
        var complexSamples = audioFrame.Samples
            .Select(s => new Complex(s, 0))
            .ToArray();
        
        Fourier.Forward(complexSamples, FourierOptions.Matlab);

        return complexSamples;
    }

    /// <summary>
    /// Transforms frequency bins back to PCM (float[]) samples
    /// </summary>
    /// <param name="freqBins"></param>
    /// <param name="format"></param>
    /// <returns><see cref="AudioFrame"/> ready for playback.</returns>
    public AudioFrame Inverse(Complex[] freqBins, AudioFormat format)
    {
        var flippedDomain = (Complex[])freqBins.Clone();
        
        Fourier.Inverse(flippedDomain, FourierOptions.Matlab);

        var resultSamples = flippedDomain
            .Select(c => (float)c.Real)
            .ToArray();

        return new AudioFrame(resultSamples, format);

    }
}