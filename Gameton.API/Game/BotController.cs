using System.Linq;
using Gameton.DataModels.ShipCommand;

namespace Gameton.Game;

public class BotController : ShipController {
    public override ShipCommand? GetShipCommand() {
        var myShipEntities = GameState.myShipsEntities;
        var enemyShips = GameState.enemyShips;
        var map = GameState.Map;
        var zone = GameState.zone;
        
        MyShipEntity? nearestAlly = null;

        if (myShipEntities != null)
            nearestAlly = MyShipEntity.FindNearestShip(MyShipEntity.SearchAllies(myShipEntities));
            
        if (enemyShips != null && enemyShips.Count != 0) {
            if (myShipEntities != null)
                enemyShips.Sort((s1, s2) => s1.size - s2.size);
        
            (int enemyX, int enemyY) = enemyShips[0].PredictMovement();
        
            double distance = MyShipEntity.GetDistance(enemyX, enemyY);

            if (distance <= 30) {
                if (MyShipEntity.cannonCooldownLeft == 0)
                    MyShipEntity.Shoot(enemyX, enemyY);
            }
            else {
                MyShipEntity.Move(enemyX, enemyY, nearestAlly, map);
                return MyShipEntity.ShipCommand;;
            }
        }
        
        MyShipEntity.Move(zone.x, zone.y, nearestAlly, map);
        
        return MyShipEntity.ShipCommand;
    }

    public BotController(MyShipEntity myShipEntity, GameState gameState) : base(myShipEntity, gameState) {
        MyShipEntity = myShipEntity;
        GameState = gameState;
    }
}