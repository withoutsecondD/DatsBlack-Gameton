using System.Windows.Controls;
using Gameton.DataModels.Scan;

namespace Gameton.WPF;

public partial class EnemyShipItem : UserControl
{
    public EnemyShipItem(ShipBase enemyShip)
    {
        InitializeComponent();
        
        string hp = "HP: " + enemyShip.hp;
        string size = "Size: " + enemyShip.size;
        string speed = "Speed: " + enemyShip.speed;
        TextBlock textBlock = new TextBlock();
        textBlock.Text += hp + "\n" + size + "\n" + speed;
    }
}