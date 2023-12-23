namespace Gameton;

public class ShipCommandResponse {
    public int tick { get; set; }
    public bool success { get; set; }
    public List<ResponseError> errors { get; set; }
}