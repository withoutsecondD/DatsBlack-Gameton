using System.Text.Json.Serialization;

namespace Gameton.DataModels.Scan;

public class Scan
{
    [JsonRequired] public int tick { get; set; }
    #nullable disable
    [JsonRequired] public List<MyShip> myShips { get; set; }
    [JsonRequired] public List<ShipBase> enemyShips { get; set; }
    #nullable enable
    [JsonRequired] public Zone? zone { get; set; }
}