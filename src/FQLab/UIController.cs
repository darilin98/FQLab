namespace FQLab;

public class UIController
{
    private Task? _playbackTask;
    
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
            var audioEngine = new AudioEngine(audioStream, new NAudioPlayer(), new MathNetFftProcessor());
            audioEngine.Run();
            await Task.WhenAll(audioEngine.Tasks);
        }
    }
}