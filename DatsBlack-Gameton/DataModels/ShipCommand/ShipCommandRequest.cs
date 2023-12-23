using System.Text.Json.Serialization;

namespace Gameton.DataModels.ShipCommand;
#nullable disable

public record ShipCommandRequest {
    [JsonRequired] public List<ShipCommand> ships { get; set; }
}