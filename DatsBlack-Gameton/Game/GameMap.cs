using DTLib.Extensions;
using Gameton.DataModels.Map;
using Gameton.DataModels.Scan;
using File = DTLib.Filesystem.File;

namespace Gameton.Game;

public enum GameMapCell : byte
{
    Space, Island, Enemy, Ally 
}

public class GameMap
{
    public GameMapCell[,] Data;
    public int Width;
    public int Height;

    public static async Task<GameMap> CreateAsync(MapData mapData, List<MyShip> allies, List<ShipBase> enemies) =>
        await Task.Run(() =>
        {
            var gameMap = new GameMap(mapData.width, mapData.height);
            gameMap.FillWithBlankSPace();
            gameMap.DrawIslands(mapData.islands);
            gameMap.DrawAllies(allies);
            gameMap.DrawEnemies(enemies);
            return gameMap;
        });

    private GameMap(int width, int height)
    {
        Data = new GameMapCell[height, width];;
        Width = width;
        Height = height;
    }

    private void FillWithBlankSPace()
    {
        for (int y = 0; y < Height; y++)
        for (int x = 0; x < Width; x++)
            Data[x, y] = GameMapCell.Space;
    }

    private void DrawIslands(List<Island> islands)
    {
        foreach (var island in islands)
        {
            for (var y = 0; y < island.map.Count; y++)
            {
                var row = island.map[y];
                for (int x = 0; x < row.Count; x++)
                {
                    var column = row[x];
                    if (column == 1)
                    {
                        int gridY = y + island.start[1];
                        int gridX = x + island.start[0];
                        if (gridY < Height && gridX < Width) 
                            Data[gridY, gridX] = GameMapCell.Island;
                    }
                }
            }
        }
    }

    private void DrawAllies(List<MyShip> allies)
    {
        foreach (var ship in allies) 
            DrawShip(ship, GameMapCell.Ally);
    }

    private void DrawEnemies(List<ShipBase> enemies)
    {
        foreach (var ship in enemies) 
            DrawShip(ship, GameMapCell.Enemy);
    }

    private void DrawShip(ShipBase ship, GameMapCell cellType)
    {
        switch (ship.direction)
        {
            case "north":
                for (int y = 0; y < ship.size; y++)
                    Data[ship.y - y, ship.x] = cellType;
                break;
            case "south":
                for (int y = 0; y < ship.size; y++)
                    Data[ship.y + y, ship.x] = cellType;
                break;
            case "west":
                for (int x = 0; x < ship.size; x++)
                    Data[ship.y, ship.x - x] = cellType;
                break;
            case "east":
                for (int x = 0; x < ship.size; x++)
                    Data[ship.y, ship.x + x] = cellType;
                break;
            default:
                throw new Exception("unexpected direction: " + ship.direction);
        }
    }


    public void WriteToFile()
    {
        using var file = File.OpenWrite("mapRender.txt");
        byte newLine = '\n'.ToByte();
        
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++) 
                file.WriteByte((byte)(Data[y, x] + 48));
            file.WriteByte(newLine);
        }
    }
}