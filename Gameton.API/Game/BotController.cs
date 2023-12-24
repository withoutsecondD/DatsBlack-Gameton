using System.Linq;
using Gameton.DataModels.Scan;
using Gameton.DataModels.ShipCommand;

namespace Gameton.Game;

public class BotController : ShipController {
    public override ShipCommand? GetShipCommand() {
        var myShipEntities = GameState.myShipsEntities;
        var enemyShips = GameState.enemyShips;
        var map = GameState.Map;
        var zone = GameState.zone;
        ShipBase targetShip = null;
        
        MyShipEntity? nearestAlly = null;

        if (enemyShips != null && enemyShips.Count != 0) {
            if (myShipEntities != null) {
                targetShip = enemyShips.MinBy(s => MyShipEntity.GetDistance(s.x, s.y));
            }

            (int enemyX, int enemyY) = targetShip.PredictMovement();
        
            double distance = MyShipEntity.GetDistance(enemyX, enemyY);

            if (distance <= 30) {
                if (MyShipEntity.cannonCooldownLeft == 0)
                    MyShipEntity.Shoot(enemyX, enemyY);
            }
            else {
                MyShipEntity.Move(enemyX, enemyY, map);
            }
            
            return MyShipEntity.ShipCommand;
        }
        
        MyShipEntity.Move(MyShipEntity.x, MyShipEntity.y, map);
        return MyShipEntity.ShipCommand;
    }

    public BotController(MyShipEntity myShipEntity, GameState gameState) : base(myShipEntity, gameState) {
        MyShipEntity = myShipEntity;
        GameState = gameState;
    }
}