using System.Text.Json.Serialization;

namespace Gameton.DataModels.LongScan;

public class LongScanResponse {
    [JsonRequired] public int tick { get; set; }
    [JsonRequired] public bool success { get; set; }
    [JsonRequired] public List<ResponseError>? errors { get; set; }
}