using System.Text.Json.Serialization;

namespace Gameton.DataModels.Scan;
#nullable disable

public record ShipBase
{
    [JsonRequired] public int x { get; set; }
    [JsonRequired] public int y { get; set; }
    [JsonRequired] public int hp { get; set; }
    [JsonRequired] public int maxHp { get; set; }
    [JsonRequired] public int size { get; set; }
    [JsonRequired] public string direction { get; set; }
    [JsonRequired] public int speed { get; set; }

    public (int, int) PredictMovement() {
        int finalX = 0;
        int finalY = 0;

        switch (direction) {
            case "north":
                finalX = x;
                finalY = y - speed;
                break;
            case "south":
                finalX = x;
                finalY = y + speed;
                break;
            case "west":
                finalX = x - speed;
                finalY = y;
                break;
            case "east":
                finalX = x + speed;
                finalY = y;
                break;
        }
        
        return (finalX, finalY);
    }
    
    public double GetDistance(int x0, int y0) =>
        Math.Sqrt(Math.Pow(x - x0, 2) + Math.Pow(y - y0, 2));
}