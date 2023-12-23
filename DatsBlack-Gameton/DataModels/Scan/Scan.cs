using System.Text.Json.Serialization;

namespace Gameton.DataModels.Scan;
#nullable disable

public class Scan
{
    [JsonRequired] public List<Ship> myShips { get; set; }
    [JsonRequired] public List<EnemyShip> enemyShips { get; set; }
    [JsonRequired] public Zone zone { get; set; }
    [JsonRequired] public int tick { get; set; }
}