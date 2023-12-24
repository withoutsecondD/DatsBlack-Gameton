using System.Linq;
using Gameton.DataModels.Map;
using Gameton.DataModels.Scan;
using Gameton.DataModels.ShipCommand;
using Gameton.Game;
using Timer = DTLib.Timer;

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
        UpdateTimer = new Timer(true, 3000, UpdateAsync);
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
            foreach (var myShip in gameState.myShipsEntities)
            {
                if(myShip.ShipCommand is not null)
                    shipCommandRequest.ships.Add(myShip.ShipCommand);
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

    void ShipsAIUpdate(List<MyShipEntity> myShipsEntities, List<ShipBase>? enemyShips)
    {
        if (enemyShips != null && enemyShips.Count != 0) {
            enemyShips.Sort((s1, s2) => s1.size - s2.size);
            
            foreach (var ship in myShipsEntities) {
                (int enemyX, int enemyY) = PredictMovement(enemyShips[0]);

                double distance = GetDistance(enemyX, ship.x, enemyY, ship.y);

                if (distance <= 20) {
                    ship.Shoot(enemyX, enemyY);
                }
                else {
                    ship.MoveTo(enemyX, enemyY);
                }
            }
        }
    }
    
    public double GetDistance(int x0, int x1, int y0, int y1) => 
        Math.Sqrt(Math.Pow(x1 - x0, 2) + Math.Pow(y1 - y0, 2));

    public (int, int) PredictMovement(ShipBase ship) {
        int finalX = 0;
        int finalY = 0;

        switch (ship.direction) {
            case "north":
                finalX = ship.x;
                finalY = ship.y - ship.speed;
                break;
            case "south":
                finalX = ship.x;
                finalY = ship.y + ship.speed;
                break;
            case "west":
                finalX = ship.x - ship.speed;
                finalY = ship.y;
                break;
            case "east":
                finalX = ship.x + ship.speed;
                finalY = ship.y;
                break;
        }
        
        return (finalX, finalY);
    }
}