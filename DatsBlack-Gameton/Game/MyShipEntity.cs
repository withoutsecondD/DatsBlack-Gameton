using Gameton.DataModels.Enum;
using Gameton.DataModels.Scan;
using Gameton.DataModels.ShipCommand;

namespace Gameton.Game; 

public class MyShipEntity : MyShip {
    public ShipCommand ShipCommand;

    public void MoveTo(int enemyX, int enemyY) {
        if (Enum.Parse<Direction>(direction) == Direction.south || Enum.Parse<Direction>(direction) == Direction.north) {
            if (enemyY - y > 5)
                ChangeSpeed(5);
            else
                ChangeSpeed(enemyY - y - size);
        }
        else {
            if (enemyX - x > 5)
                ChangeSpeed(5);
            else
                ChangeSpeed(enemyX - x - size);
        }

        Rotate(CalculateAngle(enemyX, enemyY));
    }

    public int CalculateAngle(int enemyX, int enemyY) {
        if (enemyX - x > 10 || Enum.Parse<Direction>(direction) != Direction.east) {
            if (Enum.Parse<Direction>(direction) == Direction.south)
                return -90;
            
            return 90;
        }

        if (x - enemyX > 10 || Enum.Parse<Direction>(direction) != Direction.west) {
            if (Enum.Parse<Direction>(direction) == Direction.north)
                return -90;
            
            return 90;
        }
        if (enemyY - y > 10 || Enum.Parse<Direction>(direction) != Direction.south) {
            if (Enum.Parse<Direction>(direction) == Direction.west)
                return -90;
            
            return 90;
        }
        if (y - enemyY > 10 || Enum.Parse<Direction>(direction) != Direction.north) {
            if (Enum.Parse<Direction>(direction) == Direction.east)
                return -90;
            
            return 90;
        }

        return 0;
    }
    
    public void Shoot(int x, int y) {
        ShipCommand.cannonShoot = new CannonShoot(x, y);
    }

    public void Rotate(int degrees) {
        ShipCommand.rotate = degrees;
    }
    
    public void ChangeSpeed(int changeSpeed) {
        ShipCommand.changeSpeed = changeSpeed;
    }

    public MyShipEntity(int id) {
        ShipCommand = new();
        ShipCommand.id = id;
    }

    public ShipCommand GetShipCommand() {
        return ShipCommand;
    }

    public MyShipEntity(MyShip myShip) {
        ShipCommand = new();
        x = myShip.x;
        y = myShip.y;
        hp = myShip.hp;
        maxHp = myShip.maxHp;
        size = myShip.size;
        direction = myShip.direction;
        speed = myShip.speed;
        id = myShip.id;
        maxSpeed = myShip.maxSpeed;
        minSpeed = myShip.minSpeed;
        maxChangeSpeed = myShip.maxChangeSpeed;
        cannonCooldown = myShip.cannonCooldown;
        cannonCooldownLeft = myShip.cannonCooldownLeft;
        cannonRadius = myShip.cannonRadius;
        scanRadius = myShip.scanRadius;
        cannonShootSuccessCount = myShip.cannonShootSuccessCount;
    }
}