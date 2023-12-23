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

    public GameState(ScanResponse scan, GameMap initialMap)
    {
        enemyShips = scan.scan.enemyShips;
        myShips = scan.scan.myShips;
        myShipsEntities = myShips.Select(s => new MyShipEntity(s)).ToList();
        Map = initialMap.Copy();
        Map.DrawAllies(myShips);
        Map.DrawEnemies(enemyShips);
    }
}