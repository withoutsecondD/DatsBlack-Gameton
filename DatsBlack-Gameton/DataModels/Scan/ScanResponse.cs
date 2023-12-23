using System.Text.Json.Serialization;

namespace Gameton.DataModels.Scan;
#nullable disable
public record ScanResponse : ResponseBase
{
    [JsonRequired] public Scan scan { get; set; }
}
