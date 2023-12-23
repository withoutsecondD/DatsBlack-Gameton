global using System;
global using System.Collections.Generic;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Text;
global using DTLib.Logging;
global using DTLib.Filesystem;
global using DTLib.Extensions;

namespace Gameton;

public static class Program {
    public static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        
        ILogger logger = new CompositeLogger(
            new ConsoleLogger(),
            new FileLogger("logs", "gameton")
        );
        logger.DebugLogEnabled = true;
        
        string? token = Environment.GetEnvironmentVariable("GAMETON_TOKEN");
        if(string.IsNullOrEmpty(token))
            throw new Exception("can't get value of environment variable GAMETON_TOKEN");
        
        var client = new GametonClient(token, logger);
        GameManager gameManager = new(client, logger);
        gameManager.StartAsync();

        await Task.Delay(-1); // infinite wait
    }
}
