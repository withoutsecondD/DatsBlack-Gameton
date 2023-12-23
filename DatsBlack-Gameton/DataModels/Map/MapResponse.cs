using System.Text.Json.Serialization;

namespace Gameton.DataModels.Map;
#nullable disable
public record MapResponse : ResponseBase
{
    [JsonRequired] public string mapUrl { get; set; }
}