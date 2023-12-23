global using System;
global using System.Collections.Generic;
global using System.Net.Http;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Text;
using DTLib.Logging;

namespace Gameton;

public static class Program {
    public static GametonClient Client = null!;
    
    public static readonly ILogger Logger = new CompositeLogger(
        new ConsoleLogger(),
        new FileLogger("logs", "gameton")
    );

    public static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        
        Logger.DebugLogEnabled = true;
        
        string? token = Environment.GetEnvironmentVariable("GAMETON_TOKEN");
        if(string.IsNullOrEmpty(token))
            throw new Exception("can't get value of environment variable GAMETON_TOKEN");
        
        Client = new GametonClient(token, Logger);
        GameManager gameManager = new();
        gameManager.StartAsync();

        await Task.Delay(-1); // infinite wait
    }
}
