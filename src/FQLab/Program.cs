namespace FQLab;

class Program
{
    static void Main(string[] args)
    {
        if (InputHandler.TryOpenAudioStream("test.mp3", out var stream))
        {
            using (stream)
            {
                int frameSize = 1024;
                AudioFrame? frame;
                while ((frame = stream.ReadFrame(frameSize)) != null)
                {
                    
                }
            }
        }
        else
        {
            Console.WriteLine("Error decoding file");
        }
    }
}