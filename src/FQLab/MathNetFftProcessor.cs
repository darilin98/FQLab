using System.Numerics;
using MathNet.Numerics.IntegralTransforms;
using FQLab.PluginContracts;

namespace FQLab;


public class MathNetFftProcessor : IFftProcessor
{
    
    public Complex[] Forward(AudioFrame audioFrame)
    {
        var complexSamples = audioFrame.Samples
            .Select(s => new Complex(s, 0))
            .ToArray();
        
        Fourier.Forward(complexSamples, FourierOptions.Matlab);

        return complexSamples;
    }

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