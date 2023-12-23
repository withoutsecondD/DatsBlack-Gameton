using System.Text.Json.Serialization;

namespace Gameton.DataModels.ShipCommand;

public record ShipCommandResponse : ResponseBase {
    [JsonRequired] public int tick { get; set; }
}