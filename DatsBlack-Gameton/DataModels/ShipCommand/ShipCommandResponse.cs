using System.Text.Json.Serialization;

namespace Gameton.DataModels.ShipCommand;
#nullable disable
public class ShipCommandResponse {
    [JsonRequired] public int tick { get; set; }
    [JsonRequired] public bool success { get; set; }
    [JsonRequired] public List<ResponseError> errors { get; set; }
}