using System.Net.Mime;
using Terminal.Gui.App;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace FQLab;

class Program
{
    static void Main(string[] args)
    {
        Application.Shutdown();
        Application.Init();
        Application.Run(new InputSelectWindow(new UIController()));
        
        Application.Shutdown ();
    }
}