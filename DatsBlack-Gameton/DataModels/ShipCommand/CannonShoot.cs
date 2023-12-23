namespace Gameton.DataModels.ShipCommand; 

public class CannonShoot {
    public int x { get; set; }
    public int y { get; set; }

    public CannonShoot(int x, int y) {
        this.x = x;
        this.y = y;
    }
}