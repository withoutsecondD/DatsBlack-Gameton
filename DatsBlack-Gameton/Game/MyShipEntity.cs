using Gameton.DataModels;
using Gameton.DataModels.Scan;
using Gameton.DataModels.ShipCommand;

namespace Gameton.Game; 

public record MyShipEntity : MyShip {
    /// <summary>
    /// Add this to ShipCommandRequest if it is not null.
    /// It is null if no changes were done to the ship.
    /// </summary>
    public ShipCommand? ShipCommand { get; private set; }
    
    private DirectionEnum Direction;
    
    public MyShipEntity(MyShip myShip) {
        Direction = Enum.Parse<DirectionEnum>(direction);
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

    public void MoveTo(int enemyX, int enemyY)
    {
        if (Direction == DirectionEnum.south || Direction == DirectionEnum.north) {
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
        if (enemyX - x > 10 || Direction != DirectionEnum.east) {
            if (Direction == DirectionEnum.south)
                return -90;
            return 90;
        }
        if (x - enemyX > 10 || Direction != DirectionEnum.west) {
            if (Direction == DirectionEnum.north)
                return -90;
            return 90;
        }
        if (enemyY - y > 10 || Direction != DirectionEnum.south) {
            if (Direction == DirectionEnum.west)
                return -90;
            return 90;
        }
        if (y - enemyY > 10 || Direction != DirectionEnum.north) {
            if (Direction == DirectionEnum.east)
                return -90;
            return 90;
        }

        return 0;
    }
    
    public void Shoot(int x, int y) {
        if(ShipCommand is null)
            ShipCommand = new();
        ShipCommand.cannonShoot = new CannonShoot(x, y);
    }

    public void Rotate(int degrees) {
        if(ShipCommand is null)
            ShipCommand = new();
        ShipCommand.rotate = degrees;
    }
    
    public void ChangeSpeed(int changeSpeed) {
        if(ShipCommand is null)
            ShipCommand = new();
        ShipCommand.changeSpeed = changeSpeed;
    }
}