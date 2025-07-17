using System.Collections.Concurrent;
using System.IO.Enumeration;

namespace FQLab;

public class AudioEngine
{
    private readonly IAudioStream _audioStream;
    private readonly IFftProcessor _fftProcessor;
    private readonly IAudioPlayer _audioPlayer;

    private const int frameSize = 1024;

    private BlockingCollection<AudioFrame> frameBuffer = new BlockingCollection<AudioFrame>(32);
    private Task _producerTask;
    private Task _consumerTask;

    public Task[] Tasks => new[] { _producerTask, _consumerTask };
    
    private bool playing = false;
    internal AudioEngine(IAudioStream audioStream, IAudioPlayer audioPlayer, IFftProcessor fftProcessor)
    {
        _audioStream = audioStream;
        _audioPlayer = audioPlayer;
        _fftProcessor = fftProcessor;
    }

    public void Run()
    {
        var cancellationToken = new CancellationTokenSource();
        _producerTask = Task.Run(() => AudioProducerProcess(cancellationToken.Token));
        _consumerTask = Task.Run(() => AudioConsumerProcess(cancellationToken.Token));
    }

    public void Pause()
    {
        
    }

    public void Abort()
    {
        
    }

    private void AudioProducerProcess(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            var frame = _audioStream.ReadFrame(frameSize);
            
            if (frame is null)
                break;

            // FFT Processing
            var freqBins = _fftProcessor.Forward(frame);

            var resultFrame = _fftProcessor.Inverse(freqBins, _audioStream.Format);
            
            frameBuffer.Add(frame, token);
        }
        
        frameBuffer.CompleteAdding();
    }

    private void AudioConsumerProcess(CancellationToken token)
    {
        foreach (var frame in frameBuffer.GetConsumingEnumerable(token))
        {
            _audioPlayer.Play(frame);
        }
    }
}