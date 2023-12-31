﻿using System.Text.Json.Serialization;

namespace Gameton.DataModels.ShipCommand;

public record ShipCommand {
    [JsonRequired] public int id { get; set; }
    [JsonRequired] public int? changeSpeed { get; set; }
    [JsonRequired] public int? rotate { get; set; }
    [JsonRequired] public CannonShoot? cannonShoot { get; set; }
}