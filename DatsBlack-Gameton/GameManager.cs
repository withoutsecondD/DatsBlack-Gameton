using System.Linq;
using Gameton.DataModels.Scan;
using Gameton.DataModels.ShipCommand;
using Gameton.Game;

namespace Gameton;

public class GameManager {
    public async Task Update() {
        ScanResponse scan = await Program.client.RequestScanAsync();

        List<MyShipEntity> myShipsEntities = scan.scan.myShips.Select(s => new MyShipEntity(s)).ToList();
        List<ShipBase> enemyShips = scan.scan.enemyShips;
        
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

    public double GetDistance(int x0, int x1, int y0, int y1) {
        return Math.Sqrt(Math.Pow(x1 - x0, 2) + Math.Pow(y1 - y0, 2));
    }

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