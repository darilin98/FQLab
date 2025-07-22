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
        int channelCount = _audioStream.Format.ChannelCount;
        var inputBuffer = new float[FrameSize * channelCount];
        var overlapAddBuffer = new float[(FrameSize + HopSize) * channelCount];
        var tailBuffer = new float[(FrameSize - HopSize) * channelCount];

        int writePos = 0;

        while (!token.IsCancellationRequested)
        {
            var nextFrame = _audioStream.ReadFrame(HopSize);
            if (nextFrame is null || nextFrame.Samples.Length < HopSize * channelCount)
                break;

            // Shift for overlap
            Array.Copy(tailBuffer, 0, inputBuffer, 0, tailBuffer.Length);
            Array.Copy(nextFrame.Samples, 0, inputBuffer, tailBuffer.Length, nextFrame.Samples.Length);
            
            Array.Copy(inputBuffer, HopSize * channelCount, tailBuffer, 0, tailBuffer.Length);

            // Mix stereo to mono
            var monoInput = new float[FrameSize];
            for (int i = 0; i < FrameSize; i++)
            {
                float sum = 0f;
                for (int c = 0; c < channelCount; c++)
                    sum += inputBuffer[i * channelCount + c];
                monoInput[i] = sum / channelCount;
            }
            
            for (int i = 0; i < FrameSize; i++)
                monoInput[i] *= _hannWindow[i];

            // FFT Pipeline
            var freqBins = _fftProcessor.Forward(new AudioFrame(monoInput, _audioStream.Format));
            
            // Export frequency data
            _dataReceiver?.ReceiveFrequencyData(new FreqViewData(freqBins, _audioStream.Format, FrameSize));
            
            var ifftFrame = _fftProcessor.Inverse(freqBins, _audioStream.Format);
            var monoIfftSamples = ifftFrame.Samples;

            // Overlap-add into stereo buffer
            for (int i = 0; i < FrameSize; i++)
            {
                for (int c = 0; c < channelCount; c++)
                {
                    int idx = ((writePos + i) * channelCount + c) % overlapAddBuffer.Length;
                    overlapAddBuffer[idx] += monoIfftSamples[i]; 
                }
            }
            
            var output = new float[HopSize * channelCount];
            for (int i = 0; i < HopSize; i++)
            {
                for (int c = 0; c < channelCount; c++)
                {
                    int idx = ((writePos + i) * channelCount + c) % overlapAddBuffer.Length;
                    output[i * channelCount + c] = overlapAddBuffer[idx];
                    overlapAddBuffer[idx] = 0f;
                }
            }

            writePos = (writePos + HopSize) % (FrameSize + HopSize);

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
    
}