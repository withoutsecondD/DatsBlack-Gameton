using System.Text.Json.Serialization;

namespace Gameton.DataModels.Scan;

public record Zone
{
    [JsonRequired] public int x { get; set; }
    [JsonRequired] public int y { get; set; }
    [JsonRequired] public int radius { get; set; }
}