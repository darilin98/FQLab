using System.IO.Enumeration;

namespace FQLab;

public class AudioEngine
{
    private readonly IAudioStream _audioStream;
    private readonly IFftProcessor _fftProcessor;
    private readonly IAudioPlayer _audioPlayer;
    
    private bool playing = false;
    internal AudioEngine(IAudioStream audioStream, IAudioPlayer audioPlayer, IFftProcessor fftProcessor)
    {
        _audioStream = audioStream;
        _audioPlayer = audioPlayer;
        _fftProcessor = fftProcessor;
    }
}