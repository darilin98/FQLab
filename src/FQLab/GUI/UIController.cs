using Terminal.Gui.App;
using Terminal.Gui.Views;

namespace FQLab;

public class UIController
{
    private Task? _playbackTask;
    private readonly IAudioEngineFactory _engineFactory;

    private AudioEngine? _audioEngine;

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

    public void UpdateEq(EqSettings settings)
    {
        _audioEngine?.SetEq(settings);
    }

    public void PauseTrack() => _audioEngine?.Pause();

    public void ResumeTrack() => _audioEngine?.Resume();
    public void KillTrack() => _audioEngine?.Abort();

    public void UpdateVolume(int option)
    {
        if (_audioEngine is not null)
            _audioEngine.VolumeFactor = (float)(option / 10f);
    }

    public int GetCurrentVolume()
    {
        if (_audioEngine is null)
            return 0;
        return (int)(_audioEngine.VolumeFactor * 10);
    }

    private async Task StartPlayback(IAudioStream audioStream)
    {
        using (audioStream)
        {
            var response = _engineFactory.Create(audioStream, withDataExport: true);
            using (_audioEngine)
            {
                _audioEngine = response.AudioEngine;
                var playingWindow = new PlayingWindow(this, response.View);
                
                Application.Top?.Add(playingWindow);
                _audioEngine.Run();
                try
                {
                    await Task.WhenAll(_audioEngine.Tasks);
                }
                catch (OperationCanceledException)
                {
                    // Playback Aborted
                }
                Application.Top?.Remove(playingWindow);
            }
        }
    }
}