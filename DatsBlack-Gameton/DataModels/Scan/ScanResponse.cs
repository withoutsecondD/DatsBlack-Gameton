using System.Text.Json.Serialization;

namespace Gameton.DataModels.Scan;
public class ScanResponse
{
    [JsonRequired] public bool success { get; set; }
    [JsonRequired] public Scan? scan { get; set; }
}
