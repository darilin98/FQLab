using FQLab.PluginContracts;
using System;

namespace HallEcho;

public class HallEcho : IAudioPlugin
{
    private float[] _delayBuffer = [];
    private int _bufferSize = 0;
    private int _writePos;

    private readonly int _delayMs = 400;
    private readonly float _feedback = 0.5f;
    private readonly float _decay = 0.7f;
    
    
    public void Process(ref float[] samples, AudioFormat format)
    {
        int sampleRate = format.SampleRate;
        int channelCount = format.ChannelCount;

        int delaySamples = (int)(sampleRate * (_delayMs / 1000f)) * channelCount;
        int requiredBufferSize = delaySamples * 2;

        if (_delayBuffer.Length != requiredBufferSize)
        {
            _bufferSize = requiredBufferSize;
            _delayBuffer = new float[_bufferSize];
            _writePos = 0;
        }

        for (int i = 0; i < samples.Length; i++)
        {
            int delayIndex = (_writePos + _bufferSize - delaySamples) % _bufferSize;

            float delayedSample = _delayBuffer[delayIndex];

            float outSample = samples[i] + delayedSample * _decay;

            _delayBuffer[_writePos] = outSample * _feedback;

            samples[i] = outSample;

            _writePos = (_writePos + 1) % _bufferSize;
        }

    }

    public string Name => "Hall Echo";
}