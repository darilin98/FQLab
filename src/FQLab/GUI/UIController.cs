namespace FQLab;

public class UIController
{
    private Task? _playbackTask;
    private readonly IAudioEngineFactory _engineFactory;

    public UIController(IAudioEngineFactory engineFactory)
    {
        _engineFactory = engineFactory;
    }
    
    public bool TryPlayFile(string filePath)
    {
        
        if (InputHandler.TryOpenAudioStream(filePath, out var stream))
        {

            _playbackTask = Task.Run(() => StartPlayback(stream));
            return true;
        }
        
        return false;
    }

    private async Task StartPlayback(IAudioStream audioStream)
    {
        using (audioStream)
        {
            var audioEngine = _engineFactory.Create(audioStream);
            audioEngine.Run();
            await Task.WhenAll(audioEngine.Tasks);
        }
    }
}