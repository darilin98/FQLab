using System.Collections.Concurrent;
using System.IO.Enumeration;
using System.Numerics;
using Terminal.Gui.App;

namespace FQLab;

public class AudioEngine
{
    private readonly IAudioStream _audioStream;
    private readonly IFftProcessor _fftProcessor;
    private readonly IAudioPlayer _audioPlayer;

    // Optional for rendering audio data
    private readonly IFreqDataReceiver? _dataReceiver;

    private EqSettings _eqSettings = new EqSettings();

    private const int FrameSize = 1024;
    private const int HopSize = FrameSize / 2;

    private readonly float[] _hannWindow = GenerateHannWindow(FrameSize);
    
    private int _channelCount;
    private float[] _inputBuffer = [];
    private float[] _overlapAddBuffer = [];
    private float[] _tailBuffer = [];
    private int _writePos = 0;
    
    private BlockingCollection<AudioFrame> _frameBuffer = new BlockingCollection<AudioFrame>(32);
    private Task _producerTask;
    private Task _consumerTask;
    private CancellationTokenSource? _cancellationTokenSource;

    public Task[] Tasks => new[] { _producerTask, _consumerTask };
    
    private bool _playing = false;
    public AudioEngine(IAudioStream audioStream, IAudioPlayer audioPlayer, IFftProcessor fftProcessor, IFreqDataReceiver? dataReceiver = null)
    {
        _audioStream = audioStream;
        _audioPlayer = audioPlayer;
        _fftProcessor = fftProcessor;
        _dataReceiver = dataReceiver;

        _eqSettings = new EqSettings() { Lows = -10, Highs = -10, Mids = 10};
        _audioPlayer.Initialize(_audioStream);
    }

    public void Run()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _producerTask = Task.Run(() => AudioProducerProcess(_cancellationTokenSource.Token));
        _consumerTask = Task.Run(() => AudioConsumerProcess(_cancellationTokenSource.Token));
    }

    public void Pause()
    {
        
    }

    public void Abort()
    {
        _cancellationTokenSource?.Cancel();
    }

    public void SetEq(EqSettings settings) => _eqSettings = settings;

    private void AudioProducerProcess(CancellationToken token)
    {
        _channelCount = _audioStream.Format.ChannelCount;
        _inputBuffer = new float[FrameSize * _channelCount];
        _overlapAddBuffer = new float[(FrameSize + HopSize) * _channelCount];
        _tailBuffer = new float[(FrameSize - HopSize) * _channelCount];

        while (!token.IsCancellationRequested)
        {
            var nextFrame = _audioStream.ReadFrame(HopSize);
            if (nextFrame is null || nextFrame.Samples.Length < HopSize * _channelCount)
                break;

            ShiftBuffers(nextFrame);
            
            var monoInput = MixToMonoFromBuffer();
            
            // Apply windowing
            for (int i = 0; i < FrameSize; i++)
                monoInput[i] *= _hannWindow[i];

            // FFT Pipeline
            var freqBins = _fftProcessor.Forward(new AudioFrame(monoInput, _audioStream.Format));

            freqBins = ApplyEq(freqBins);
            
            // Export frequency data
            Application.Invoke(() =>
            {
                _dataReceiver?.ReceiveFrequencyData(new FreqViewData(freqBins, _audioStream.Format, FrameSize));
            });
            
            var ifftFrame = _fftProcessor.Inverse(freqBins, _audioStream.Format);
            
            OverlapAddToBuffer(ifftFrame.Samples);
           
            var output = GetOutputSamplesFromBuffer();

            _writePos = (_writePos + HopSize) % (FrameSize + HopSize);

            _frameBuffer.Add(new AudioFrame(output, _audioStream.Format), token);
        }
        _frameBuffer.CompleteAdding();
    }

    private void AudioConsumerProcess(CancellationToken token)
    {
        foreach (var frame in _frameBuffer.GetConsumingEnumerable(token))
        {
            _audioPlayer.Play(frame);
        }
    }

    private static float[] GenerateHannWindow(int size)
    {
        var window = new float[size];
        for (int i = 0; i < size; i++)
            window[i] = 0.5f * (1f - MathF.Cos(2 * MathF.PI * i / (size - 1)));
        return window;
    }

    private void ShiftBuffers(AudioFrame nextFrame)
    {
        Array.Copy(_tailBuffer, 0, _inputBuffer, 0, _tailBuffer.Length);
        Array.Copy(nextFrame.Samples, 0, _inputBuffer, _tailBuffer.Length, nextFrame.Samples.Length);
            
        Array.Copy(_inputBuffer, HopSize * _channelCount, _tailBuffer, 0, _tailBuffer.Length);
    }

    private float[] MixToMonoFromBuffer()
    {
        var result = new float[FrameSize];
        for (int i = 0; i < FrameSize; i++)
        {
            float sum = 0f;
            for (int c = 0; c < _channelCount; c++)
                sum += _inputBuffer[i * _channelCount + c];
            result[i] = sum / _channelCount;
        }

        return result;
    }

    private void OverlapAddToBuffer(float[] monoIfftSamples)
    {
        for (int i = 0; i < FrameSize; i++)
        {
            for (int c = 0; c < _channelCount; c++)
            {
                int idx = ((_writePos + i) * _channelCount + c) % _overlapAddBuffer.Length;
                _overlapAddBuffer[idx] += monoIfftSamples[i]; 
            }
        }
    }

    private float[] GetOutputSamplesFromBuffer()
    {
        var result = new float[HopSize * _channelCount];
        for (int i = 0; i < HopSize; i++)
        {
            for (int c = 0; c < _channelCount; c++)
            {
                int idx = ((_writePos + i) * _channelCount + c) % _overlapAddBuffer.Length;
                result[i * _channelCount + c] = _overlapAddBuffer[idx];
                _overlapAddBuffer[idx] = 0f;
            }
        }
        return result;
    }

    private Complex[] ApplyEq(Complex[] freqBins)
    {
        var sampleRate = _audioStream.Format.SampleRate;
        var binSize = sampleRate / freqBins.Length;

        for (int i = 0; i < freqBins.Length; i++)
        {
            float freq = i * binSize;

            float gain = freq switch
            {
                >= 20f and < 160f => _eqSettings[EqRanges.Lows],
                >= 160f and < 1500f => _eqSettings[EqRanges.Mids],
                >= 1500f => _eqSettings[EqRanges.Highs],
                _ => 1f
            };

            freqBins[i] *= gain;
        }

        return freqBins;
    }

}