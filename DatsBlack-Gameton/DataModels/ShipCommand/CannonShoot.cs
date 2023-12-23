namespace Gameton.DataModels.ShipCommand; 

public record CannonShoot {
    public int x { get; set; }
    public int y { get; set; }

    public CannonShoot(int x, int y) {
        this.x = x;
        this.y = y;
    }
}