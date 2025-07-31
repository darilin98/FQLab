using Terminal.Gui.App;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace FQLab;

/// <summary>
/// A core class of the program's UI. Keeps the state of the <see cref="AudioEngine"/> and processes requests to it made by individual Views.
/// Ensures that all tasks on the engine finish and all resources are disposed of properly.
/// </summary>
public class UIController
{
    private Task? _playbackTask;
    private readonly IAudioEngineFactory _engineFactory;

    private AudioEngine? _audioEngine;
    
    public UIController(IAudioEngineFactory engineFactory)
    {
        _engineFactory = engineFactory;
    }
    
    /// <summary>
    /// Takes a file and if successful starts the audio pipeline.
    /// </summary>
    /// <param name="filePath">Path to file to-be opened.</param>
    /// <returns>Boolean value based on whether the operation was successful.</returns>
    public bool TryPlayFile(string filePath)
    {
        
        if (InputHandler.TryOpenAudioStream(filePath, out var stream))
        {
            _playbackTask = Task.Run(() => StartPlayback(stream));
            return true;
        }
        
        return false;
    }

    /// <summary>
    /// Pushes EQ changes to the audio engine.
    /// </summary>
    /// <param name="settings">Exported EQ settings.</param>
    public void UpdateEq(EqSettings settings)
    {
        _audioEngine?.SetEq(settings);
    }

    /// <summary>
    /// Pauses the engine.
    /// </summary>
    public void PauseTrack() => _audioEngine?.Pause();

    /// <summary>
    /// Resumes the engine.
    /// </summary>
    public void ResumeTrack() => _audioEngine?.Resume();
    
    /// <summary>
    /// Aborts the audio pipeline.
    /// </summary>
    public void KillTrack() => _audioEngine?.Abort();

    /// <summary>
    /// Takes volume selection and handles proper conversion for the engine.
    /// </summary>
    /// <param name="option">Unconverted integer value of a slider.</param>
    public void UpdateVolume(int option)
    {
        if (_audioEngine is not null)
            _audioEngine.VolumeFactor = (float)(option / 10f);
    }

    /// <summary>
    /// Returns the current volume factor of the audio engine.
    /// </summary>
    /// <returns>Volume factor normalized for a slider.</returns>
    public int GetCurrentVolume()
    {
        if (_audioEngine is null)
            return 0;
        return (int)(_audioEngine.VolumeFactor * 10);
    }

    public List<PluginInstance>? GetPlugins()
    {
        return _audioEngine?.PluginList;
    }
    
    /// <summary>
    /// Manages lifetime of an audioStream and an audioEngine.
    /// Waits for all tasks on the engine to finish.
    /// </summary>
    /// <param name="audioStream"></param>
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