using System.Text.Json.Serialization;

namespace Gameton.DataModels.Scan;
#nullable disable

public class Ship
{
    [JsonRequired] public int id { get; set; }
    [JsonRequired] public int x { get; set; }
    [JsonRequired] public int y { get; set; }
    [JsonRequired] public int size { get; set; }
    [JsonRequired] public int hp { get; set; }
    [JsonRequired] public int maxHp { get; set; }
    [JsonRequired] public string direction { get; set; }
    [JsonRequired] public int speed { get; set; }
    [JsonRequired] public int maxSpeed { get; set; }
    [JsonRequired] public int minSpeed { get; set; }
    [JsonRequired] public int maxChangeSpeed { get; set; }
    [JsonRequired] public int cannonCooldown { get; set; }
    [JsonRequired] public int cannonCooldownLeft { get; set; }
    [JsonRequired] public int cannonRadius { get; set; }
    [JsonRequired] public int scanRadius { get; set; }
    [JsonRequired] public int cannonShootSuccessCount { get; set; }
}