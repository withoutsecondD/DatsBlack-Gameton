using System.Text.Json.Serialization;

namespace Gameton.DataModels.Map;

public class MapResponse {
    [JsonRequired] public int width { get; set; }
    [JsonRequired] public int height { get; set; }
    [JsonRequired] public string slug { get; set; }
    [JsonRequired] public List<Island> islands { get; set; }
}