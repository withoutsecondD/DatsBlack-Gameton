namespace Gameton;

public class Ship
{
    public int id { get; set; }
    public int x { get; set; }
    public int y { get; set; }
    public int size { get; set; }
    public int hp { get; set; }
    public int maxHp { get; set; }
    public string direction { get; set; }
    public int speed { get; set; }
    public int maxSpeed { get; set; }
    public int minSpeed { get; set; }
    public int maxChangeSpeed { get; set; }
    public int cannonCooldown { get; set; }
    public int cannonCooldownLeft { get; set; }
    public int cannonRadius { get; set; }
    public int scanRadius { get; set; }
    public int cannonShootSuccessCount { get; set; }
}