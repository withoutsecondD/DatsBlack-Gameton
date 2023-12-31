﻿using System.Text.Json.Serialization;

namespace Gameton.DataModels.LongScan;

public record LongScanRequest {
    [JsonRequired] public int x { get; set; }
    [JsonRequired] public int y { get; set; }
}