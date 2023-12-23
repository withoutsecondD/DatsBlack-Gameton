using System.Text.Json.Serialization;

namespace Gameton.DataModels.LongScan;

public record LongScanResponse : ResponseBase {
    [JsonRequired] public int tick { get; set; }
}