using FQLab.PluginContracts;

namespace WobbleTremolo;

public class WobbleTremolo : IAudioPlugin
{
    private float _lfoPhase = 0f;
    private const float TwoPi = 2 * MathF.PI;

    private readonly float _rateHz = 5f;    
    private readonly float _depth = 0.7f;
    
    public void Process(ref float[] samples, AudioFormat format)
    {
        float sampleRate = format.SampleRate;
        float lfoIncrement = TwoPi * _rateHz / sampleRate;

        for (int i = 0; i < samples.Length; i++)
        {
            float lfo = (1 - _depth) + _depth * (0.5f * (1 + MathF.Sin(_lfoPhase)));

            samples[i] *= lfo;

            _lfoPhase += lfoIncrement;
            if (_lfoPhase > TwoPi)
                _lfoPhase -= TwoPi;
        }
    }

    public string Name => "Wobble Tremolo";
}