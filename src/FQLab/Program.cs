namespace FQLab;

class Program
{
    private static string _testFilePath = "C:\\Users\\Darek\\MFF-PROJS\\FQLab\\testaudio\\lazychill.mp3";
    static async Task Main(string[] args)
    {
        if (InputHandler.TryOpenAudioStream(_testFilePath, out var stream))
        {
            using (stream)
            {
                var audioEngine = new AudioEngine(stream, new NAudioPlayer(), new MathNetFftProcessor());
                audioEngine.Run();
                await Task.WhenAll(audioEngine.Tasks);
            }
        }
        else
        {
            Console.WriteLine("Error decoding file");
        }
    }
}