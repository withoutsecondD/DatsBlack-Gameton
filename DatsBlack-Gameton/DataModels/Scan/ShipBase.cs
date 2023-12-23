using System.Text.Json.Serialization;

namespace Gameton.DataModels.Scan;
#nullable disable

public class ShipBase
{
    [JsonRequired] public int x { get; set; }
    [JsonRequired] public int y { get; set; }
    [JsonRequired] public int hp { get; set; }
    [JsonRequired] public int maxHp { get; set; }
    [JsonRequired] public int size { get; set; }
    [JsonRequired] public string direction { get; set; }
    [JsonRequired] public int speed { get; set; }
}