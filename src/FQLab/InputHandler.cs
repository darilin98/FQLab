namespace FQLab;

public static class InputHandler
{
    public static InputResponse ParseAudioFile(string audioFilePath)
    {
        return new InputResponse(new AudioFormat(0, 0), false);
    }
}