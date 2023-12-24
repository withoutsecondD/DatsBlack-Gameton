using System.Text.Json.Serialization;

namespace Gameton.DataModels;

public record ResponseBase
{
    [JsonRequired] public bool success { get; set; }
    public List<ResponseError>? errors { get; set; }
}