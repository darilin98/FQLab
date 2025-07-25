﻿using System.Net.Mime;
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
        
        try
        {
            var controller = new UIController(new AudioEngineFactory());
            var inputWin = new InputSelectWindow(controller);
            Application.Run(inputWin);
        }
        catch (Exception e)
        {
            Logger.Log(e.Message);
            Logger.Log(e.StackTrace);
            Application.Shutdown();
        }
    }
}