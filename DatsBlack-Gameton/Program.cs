global using System;
global using System.Collections.Generic;
global using System.Net.Http;
global using System.Threading;
global using System.Threading.Tasks;
global using System.Text;
using System.Diagnostics;
using DTLib.Logging;
using System.Text.Json;
using DTLib.Filesystem;
using Gameton.DataModels.Map;
using Gameton.DataModels.ShipCommand;
using Gameton.Game;

namespace Gameton;

public static class Program {
    public static GametonClient client;
    
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
        
        client = new GametonClient(token, Logger);
        // var longScan = await client.TryRequestLongScanAsync(100, 200);

        // ShipCommandRequest shipCommandRequest = new();
        // MyShipEntity myShipEntity = new(90);
        // myShipEntity.Rotate(90);
        // myShipEntity.ChangeSpeed(0);
        // myShipEntity.Shoot(64, 1825);
        //
        // shipCommandRequest.ships = new List<ShipCommand>() { myShipEntity.GetShipCommand() };
        //
        // var shipCommand = await client.TryRequestShipCommand(shipCommandRequest);

        // string jsonString = File.ReadAllText("map.json");
        // MapResponse map = JsonSerializer.Deserialize<MapResponse>(jsonString)!;
        //
        // MapRenderer.Render(map);
        GameManager gameManager = new();
        
        while (true) {
            await gameManager.Update();
            Thread.Sleep(3000);
        }
    }
}
