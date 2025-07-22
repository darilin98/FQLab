using System.Collections.Concurrent;
using System.IO.Enumeration;
using System.Numerics;

namespace FQLab;

public class AudioEngine
{
    private readonly IAudioStream _audioStream;
    private readonly IFftProcessor _fftProcessor;
    private readonly IAudioPlayer _audioPlayer;

    // Optional for rendering audio data
    private readonly IFreqDataReceiver? _dataReceiver;

    private const int FrameSize = 1024;
    private const int HopSize = FrameSize / 2;

    private readonly float[] _hannWindow = GenerateHannWindow(FrameSize);
    private float[] _overlapAddBuffer = new float[FrameSize + HopSize];
    private int _writePos = 0;

    private BlockingCollection<AudioFrame> _frameBuffer = new BlockingCollection<AudioFrame>(32);
    private Task _producerTask;
    private Task _consumerTask;
    private CancellationTokenSource? _cancellationTokenSource;

    public Task[] Tasks => new[] { _producerTask, _consumerTask };
    
    private bool playing = false;
    public AudioEngine(IAudioStream audioStream, IAudioPlayer audioPlayer, IFftProcessor fftProcessor, IFreqDataReceiver? dataReceiver = null)
    {
        _audioStream = audioStream;
        _audioPlayer = audioPlayer;
        _fftProcessor = fftProcessor;
        _dataReceiver = dataReceiver;
        
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

    private void AudioProducerProcess(CancellationToken token)
    {
        var inputBuffer = new float[FrameSize];

        while (!token.IsCancellationRequested)
        {
            var nextFrame = _audioStream.ReadFrame(HopSize);
            if (nextFrame == null || nextFrame.Samples.Length < HopSize)
                break;

            // Shift old input
            Array.Copy(inputBuffer, HopSize, inputBuffer, 0, FrameSize - HopSize);

            // Insert new samples
            Array.Copy(nextFrame.Samples, 0, inputBuffer, FrameSize - HopSize, HopSize);

            // Apply Hann window
            var windowed = new float[FrameSize];
            for (int i = 0; i < FrameSize; i++)
                windowed[i] = inputBuffer[i] * _hannWindow[i];

            // FFT process
            var freqBins = _fftProcessor.Forward(new AudioFrame(windowed, _audioStream.Format));
            if (_dataReceiver is not null)
                _dataReceiver.ReceiveFrequencyData(new FreqViewData(freqBins, _audioStream.Format, FrameSize));

            var ifftFrame = _fftProcessor.Inverse(freqBins, _audioStream.Format);
            var ifftSamples = ifftFrame.Samples;

            // Overlap-add into rolling buffer
            for (int i = 0; i < FrameSize; i++)
                _overlapAddBuffer[(_writePos + i) % _overlapAddBuffer.Length] += ifftSamples[i];

            // Output hopSize samples
            var outputFrame = new float[HopSize];
            for (int i = 0; i < HopSize; i++)
            {
                int idx = (_writePos + i) % _overlapAddBuffer.Length;
                float sample = _overlapAddBuffer[idx];

                outputFrame[i] = sample;
                _overlapAddBuffer[idx] = 0f;
            }

            _writePos = (_writePos + HopSize) % _overlapAddBuffer.Length;

            _frameBuffer.Add(new AudioFrame(outputFrame, _audioStream.Format), token);
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
    
}