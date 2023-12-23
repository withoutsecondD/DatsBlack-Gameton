namespace Gameton; 

public class AuthorizationResponse {
    public bool success { get; set; }
    public List<ResponseError> errors { get; set; }
}