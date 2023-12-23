using System.Text.Json.Serialization;

namespace Gameton.DataModels; 
#nullable disable

public class ResponseError {
    [JsonRequired] public string message { get; set; }
}