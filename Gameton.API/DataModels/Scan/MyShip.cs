using System.Text.Json.Serialization;

namespace Gameton.DataModels.Scan;

public record MyShip : ShipBase
{
    [JsonRequired] public int id { get; set; }
    [JsonRequired] public int maxSpeed { get; set; }
    [JsonRequired] public int minSpeed { get; set; }
    [JsonRequired] public int maxChangeSpeed { get; set; }
    [JsonRequired] public int cannonCooldown { get; set; }
    [JsonRequired] public int cannonCooldownLeft { get; set; }
    [JsonRequired] public int cannonRadius { get; set; }
    [JsonRequired] public int scanRadius { get; set; }
    [JsonRequired] public int cannonShootSuccessCount { get; set; }
}