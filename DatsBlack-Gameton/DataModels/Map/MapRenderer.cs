using System.IO;
using DTLib.Extensions;
using File = DTLib.Filesystem.File;

namespace Gameton.DataModels.Map; 

public class MapRenderer {
    public static void Render(MapResponse map) {
        char[,] mapGrid = new char[map.height, map.width];

        for (int y = 0; y < map.height; y++) {
            for (int x = 0; x < map.width; x++)
                mapGrid[x, y] = '-';
        }

        for (var islandIndex = 0; islandIndex < map.islands.Count; islandIndex++) {
            var island = map.islands[islandIndex];
            
            for (var y = 0; y < island.map.Count; y++) {
                var row = island.map[y];

                for (var x = 0; x < row.Count; x++) {
                    var column = row[x];

                    if (column == 1) {
                        var gridY = y + island.start[1];
                        var gridX = x + island.start[0];

                        if (gridY < map.height && gridX < map.width) {
                            mapGrid[gridY, gridX] = '0';
                        };
                    }
                }
            }
        }

        using var file = File.OpenWrite("mapRender.txt");
        byte[] buffer = new byte[1];
        
        for (int y = 0; y < map.height; y++)
        {
            for (int x = 0; x < map.width; x++) {
                buffer[0] = mapGrid[y, x].ToByte();
                file.Write(buffer);
            }

            buffer[0] = '\n'.ToByte();
            file.Write(buffer);
        }
    }
}