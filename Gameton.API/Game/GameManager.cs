using System.Linq;
using Gameton.DataModels.Map;
using Gameton.DataModels.Scan;
using Gameton.DataModels.ShipCommand;
using Gameton.Game;

namespace Gameton;

public class GameManager
{
    private GametonClient Client;
    private ILogger Logger;
    private int _gameLoopRunning = 1;
    #nullable disable
    private GameMap _mapWithIslandsOnly;
    #nullable enable

    public event Action<GameState>? OnUpdate;

    public GameManager(GametonClient client, ILogger logger)
    {
        Client = client;
        Logger = logger;
    }
    
    public async void StartAsync()
    {
        try
        {
            var mapData = await Client.TryRequestMap();
            if (mapData is null)
                throw new Exception("can't get MapData");
            _mapWithIslandsOnly = await GameMap.CreateAsync(mapData);
            
            // atomic read
            while (Interlocked.CompareExchange(ref _gameLoopRunning, 1, 1) == 1)
            {
                try
                {
                    await Update();
                }
                catch (Exception e)
                {
                    Logger.LogError("Update", e);
                }
                Thread.Sleep(3000);
            }
        }
        catch (Exception e)
        {
            Logger.LogError("StartAsync", e);
        }
    }

    public void Stop()
    {
        // atomic assign 0
        Interlocked.CompareExchange(ref _gameLoopRunning, 0, 1);
    }
    
    public async Task Update() {
        ScanResponse scan = await Client.TryRequestScanAsync()
            ?? throw new Exception("scan is null");

        var gameState = new GameState(scan, _mapWithIslandsOnly);
        
        OnUpdate?.Invoke(gameState);

        var shipCommandRequest = new ShipCommandRequest();
        shipCommandRequest.ships = new List<ShipCommand>();
        
        foreach (var myShip in gameState.myShipsEntities) {
            ShipCommand shipCommand = myShip.ShipController.GetShipCommand();
            shipCommand.id = myShip.id;
            
            if(shipCommand is not null)
                shipCommandRequest.ships.Add(shipCommand);
        }

        if (shipCommandRequest.ships.Count > 0)
        {
            var response = await Client.TryRequestShipCommand(shipCommandRequest);
        }
    }
}