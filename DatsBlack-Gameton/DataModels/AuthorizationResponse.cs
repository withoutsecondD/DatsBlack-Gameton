﻿using System.Text.Json.Serialization;

namespace Gameton.DataModels; 

public record AuthorizationResponse {
    [JsonRequired] public bool success { get; set; }
    [JsonRequired] public List<ResponseError>? errors { get; set; }
}