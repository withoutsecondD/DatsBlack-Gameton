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
    #nullable disable
    private GameMap _mapWithIslandsOnly;
    #nullable enable
    private DTLib.Timer UpdateTimer;
    
    public event Action<GameState>? OnUpdate;

    public GameManager(GametonClient client, ILogger logger)
    {
        Client = client;
        Logger = logger;
        UpdateTimer = new(true, 3000, UpdateAsync);
    }
    
    public async void StartAsync()
    {
        try
        {
            var mapData = await Client.TryRequestMap();
            if (mapData is null)
                throw new Exception("can't get MapData");
            _mapWithIslandsOnly = await GameMap.CreateAsync(mapData);
            UpdateTimer.Start();
        }
        catch (Exception e)
        {
            Logger.LogError("StartAsync", e);
        }
    }

    public void Stop()
    {
        UpdateTimer.Stop();
    }
    
    public async void UpdateAsync() {
        try
        {
            ScanResponse scan = await Client.TryRequestScanAsync()
                ?? throw new Exception("scan is null");

            var gameState = new GameState(scan, _mapWithIslandsOnly);
            
            OnUpdate?.Invoke(gameState);

            var shipCommandRequest = new ShipCommandRequest();
            shipCommandRequest.ships = new List<ShipCommand>();
        
            foreach (var myShip in gameState.myShipsEntities) {
                ShipCommand? shipCommand = myShip.ShipController.GetShipCommand();
                if(shipCommand is not null)
                {
                    shipCommand.id = myShip.id;
                    shipCommandRequest.ships.Add(shipCommand);
                }
            }

            if (shipCommandRequest.ships.Count > 0)
            {
                var response = await Client.TryRequestShipCommand(shipCommandRequest);
            }
        }
        catch (Exception e)
        {
            Logger.LogError("Update", e);
        }
    }
}