﻿using System.Linq;
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
    public ShipController? ShipController;

    private DirectionEnum Direction;
    private Dictionary<int, bool> DirectionLocked;
    
    public MyShipEntity(MyShip myShip, Dictionary<int, bool> directionLocked) {
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
        Direction = Enum.Parse<DirectionEnum>(direction);
        DirectionLocked = directionLocked;
    }

    public void Move(int targetX, int targetY, GameMap map) {
        if (Direction == DirectionEnum.south || Direction == DirectionEnum.north) {
            if (Math.Abs(targetY - y) > 5)
                ChangeSpeed(5);
            else
                ChangeSpeed(Math.Abs(targetY - y - size));
        }
        else {
            if (Math.Abs(targetX - x) > 5)
                ChangeSpeed(5);
            else
                ChangeSpeed(Math.Abs(targetX - x - size));
        }

        if (PredictCollision(map, Enum.Parse<DirectionEnum>(direction), Enum.Parse<DirectionEnum>(direction))) {
            Rotate(90);
            ChangeSpeed(-7);
            
            if (speed == -1)
                ChangeSpeed(2);
            
            DirectionLocked[id] = true;
            return;
        }

        if (!DirectionLocked[id]) {
            string tempDirection = direction;
            
            Rotate(CalculateAngle(targetX, targetY));
            if (PredictCollision(map, Enum.Parse<DirectionEnum>(tempDirection), Enum.Parse<DirectionEnum>(direction))) {
                Rotate(null);
                ChangeSpeed(-7);
                DirectionLocked[id] = true;
                return;
            }
            
            if (speed == -1)
                ChangeSpeed(2);
        }
        else {
            DirectionLocked[id] = false;
        }
    }

    public int? CalculateAngle(int enemyX, int enemyY) {
        if (enemyX - x > 10 && Direction != DirectionEnum.east) {
            if (Direction == DirectionEnum.south)
                return -90;
            return 90;
        }
        
        if (x - enemyX > 10 && Direction != DirectionEnum.west) {
            if (Direction == DirectionEnum.north)
                return -90;
            return 90;
        }
        
        if (enemyY - y > 10 && Direction != DirectionEnum.south) {
            if (Direction == DirectionEnum.west)
                return -90;
            return 90;
        }
        
        if (y - enemyY > 10 && Direction != DirectionEnum.north) {
            if (Direction == DirectionEnum.east)
                return -90;
            return 90;
        }

        return null;
    }

    public MyShipEntity? FindNearestShip(List<MyShipEntity> otherShips) {
        return otherShips.MinBy(s => GetDistance(s.x, s.y));
    }
    
    public List<MyShipEntity> SearchAllies(List<MyShipEntity> otherShips) {
        switch (Direction.ToString()) {
            case "north":
                return otherShips.Where(s => s.y < this.y).ToList();
                break;
            case "south":
                return otherShips.Where(s => s.y > this.y).ToList();
                break;
            case "east":
                return otherShips.Where(s => s.x > this.x).ToList();
                break;
            case "west":
                return otherShips.Where(s => s.y < this.x).ToList();
                break;
        }

        return null;
    }

    public bool PredictCollision(GameMap map, DirectionEnum currentDirection, DirectionEnum finalDirection) {
        (int predictedX, int predictedY) = PredictMovement(currentDirection.ToString());

        switch (finalDirection) {
            case DirectionEnum.east:
                for (int x = 0; x <= size + 15; x++) {
                    if (map.Data[predictedY, predictedX + x] == GameMapCell.Island || map.Data[predictedY, predictedX + x] == GameMapCell.Enemy || map.Data[predictedY, predictedX + x] == GameMapCell.Ally)
                        return true;
                }

                break;
            case DirectionEnum.west:
                for (int x = 0; x <= size + 15; x++) {
                    if (map.Data[predictedY, predictedX - x] == GameMapCell.Island || map.Data[predictedY, predictedX - x] == GameMapCell.Enemy || map.Data[predictedY, predictedX - x] == GameMapCell.Ally)
                        return true;
                }

                break;
            case DirectionEnum.north:
                for (int y = 0; y <= size + 15; y++) {
                    if (map.Data[predictedY - y, predictedX] == GameMapCell.Island || map.Data[predictedY - y, predictedX] == GameMapCell.Enemy || map.Data[predictedY - y, predictedX] == GameMapCell.Ally)
                        return true;
                }

                break;
            case DirectionEnum.south:
                for (int y = 0; y <= size + 15; y++) {
                    if (map.Data[predictedY + y, predictedX] == GameMapCell.Island || map.Data[predictedY + y, predictedX] == GameMapCell.Enemy || map.Data[predictedY + y, predictedX] == GameMapCell.Ally)
                        return true;
                }

                break;
        }

        return false;
    }   
    
    public bool PredictAllyCollision(MyShipEntity ally) {
        (int predictedX, int predictedY) = PredictMovement();
        (int predictedAllyX, int predictedAllyY) = ally.PredictMovement();

        return Math.Abs(predictedX - predictedAllyX) < 5 || Math.Abs(predictedY - predictedAllyY) < 5;
    }
    
    public void Shoot(int x, int y) {
        if(ShipCommand is null)
            ShipCommand = new();
        
        ShipCommand.cannonShoot = new CannonShoot(x, y);
    }

    public void Rotate(int? angle)
    {
        if (angle > 90 || angle < -90)
            throw new Exception($"can't turn to degree {angle}");
        
        if(ShipCommand is null)
            ShipCommand = new();
        ShipCommand.rotate = angle;
    }
    
    public void ChangeSpeed(int deltaV)
    {
        if (deltaV > 7 || deltaV < -7)
            throw new Exception($"can't change speed on {deltaV}");
        
        if(ShipCommand is null)
            ShipCommand = new();
        ShipCommand.changeSpeed = deltaV;
    }

    public int GetAngleToDirection(DirectionEnum otherDirection)
    {
        return (Direction - otherDirection) * 90;
    }
}