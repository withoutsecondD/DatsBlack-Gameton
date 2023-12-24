using System.Linq;
using Gameton.DataModels.Scan;
using Gameton.Game;

namespace Gameton;

public class GameState
{
    public GameMap Map;
    public List<MyShip> myShips;
    public List<MyShipEntity> myShipsEntities;
    public List<ShipBase> enemyShips;
    public Zone zone;

    public GameState(ScanResponse scan, GameMap initialMap, List<MyShipEntity> myShipEntities)
    {
        enemyShips = scan.scan.enemyShips;
        myShips = scan.scan.myShips;
        myShipsEntities = myShipEntities;
        zone = scan.scan.zone;
        Map = initialMap.Copy();
        Map.DrawAllies(myShips);
        Map.DrawEnemies(enemyShips);
    }
}