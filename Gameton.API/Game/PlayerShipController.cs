using Gameton.DataModels;

namespace Gameton.Game;

public class PlayerShipController
{
    private MyShipEntity myShipEntity;
    
    /// <returns>is turn possible (90 degrees) or impossible (180 degrees)</returns>
    public bool TryTurn(DirectionEnum direction)
    {
        int angle = myShipEntity.GetAngleToDirection(direction);
        if (angle is 180 or -180)
            return false;

        myShipEntity.Rotate(angle);
        return true;
    }

    public void ChangeSpeed(int deltaV)
    {
        myShipEntity.ChangeSpeed(deltaV);
    }
}