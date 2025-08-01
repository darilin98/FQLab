using System.Net.Mime;
using System.Text;
using Terminal.Gui.App;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace FQLab;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Application.Shutdown();
        Application.Init();
        
        try
        {
            var controller = new UIController(new AudioEngineFactory());
            var inputWin = new InputSelectWindow(controller);
            Application.Run(inputWin);
            Application.Shutdown();
        }
        catch (Exception e)
        {
            Logger.Log(e.Message);
            Logger.Log(e.StackTrace);
            Application.Shutdown();
        }
    }
}