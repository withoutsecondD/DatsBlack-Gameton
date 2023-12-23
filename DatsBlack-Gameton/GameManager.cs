using System.Linq;
using DTLib.Logging;
using Gameton.DataModels.Scan;
using Gameton.DataModels.ShipCommand;
using Gameton.Game;
using static Gameton.Program;

namespace Gameton;

public class GameManager
{
    private int _gameLoopRunning = 1;
    
    public async void StartAsync()
    {
        try
        {
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

        List<MyShipEntity> myShipsEntities = scan.scan.myShips.Select(s => new MyShipEntity(s)).ToList();
        List<ShipBase> enemyShips = scan.scan.enemyShips;
        
        ShipsAIUpdate(myShipsEntities, enemyShips);

        var shipCommandRequest = new ShipCommandRequest();
        shipCommandRequest.ships = new List<ShipCommand>();
        foreach (var myShip in myShipsEntities)
        {
            if(myShip.ShipCommand is not null)
                shipCommandRequest.ships.Add(myShip.ShipCommand);
        }

        if (shipCommandRequest.ships.Count > 0)
        {
            var response = await Client.TryRequestShipCommand(shipCommandRequest);
        }
    }

    void ShipsAIUpdate(List<MyShipEntity> myShipsEntities, List<ShipBase>? enemyShips)
    {
        if (enemyShips != null) {
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