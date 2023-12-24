global using System;
global using System.Collections.Generic;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Text;
global using DTLib.Logging;
global using DTLib.Filesystem;
global using DTLib.Extensions;
global using System.Windows;

namespace Gameton.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static ILogger Logger = new CompositeLogger(
        new ConsoleLogger(),
        new FileLogger("logs", "gameton"));
    
    protected override void OnStartup(StartupEventArgs e)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        
        Logger.DebugLogEnabled = true;
        
        string? token = Environment.GetEnvironmentVariable("GAMETON_TOKEN");
        if(string.IsNullOrEmpty(token))
            throw new Exception("can't get value of environment variable GAMETON_TOKEN");
        
        var client = new GametonClient(token, Logger);
        GameManager gameManager = new(client, Logger);
        
        Window mainWindow = new MainWindow(gameManager);
        mainWindow.Show();
        
        gameManager.StartAsync();
    }
}