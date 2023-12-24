using Gameton.DataModels.Scan;
using Gameton.DataModels.ShipCommand;

namespace Gameton.Game; 

public abstract class ShipController {
    public MyShipEntity MyShipEntity;
    public GameState GameState;

    public virtual ShipCommand? GetShipCommand() {
        return MyShipEntity.ShipCommand;
    } 
    
    
    
    public ShipController(MyShipEntity myShipEntity, GameState gameState) {
        MyShipEntity = myShipEntity;
        GameState = gameState;
    }
}