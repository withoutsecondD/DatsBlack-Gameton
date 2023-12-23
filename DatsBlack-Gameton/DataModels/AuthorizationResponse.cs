#nullable disable

using System.Text.Json.Serialization;

namespace Gameton.DataModels; 

public class AuthorizationResponse {
    [JsonRequired] public bool success { get; set; }
    [JsonRequired] public List<ResponseError> errors { get; set; }
}