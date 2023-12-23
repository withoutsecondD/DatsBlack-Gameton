namespace Gameton;

public class ShipCommand {
    public int id { get; set; }
    public int changeSpeed { get; set; }
    public int rotate { get; set; }
    public CannonShoot cannonShoot { get; set; }
}