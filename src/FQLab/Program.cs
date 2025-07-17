namespace FQLab;

class Program
{
    private static string _testFilePath = "C:\\Users\\Darek\\MFF-PROJS\\FQLab\\testaudio\\lazychill.mp3";
    static void Main(string[] args)
    {
        if (InputHandler.TryOpenAudioStream(_testFilePath, out var stream))
        {
            using (stream)
            {
                var audioEngine = new AudioEngine(stream, null, null);
                audioEngine.Run();
            }
        }
        else
        {
            Console.WriteLine("Error decoding file");
        }
    }
}