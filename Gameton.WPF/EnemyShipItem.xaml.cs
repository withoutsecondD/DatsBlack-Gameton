using System.Windows.Controls;
using Gameton.DataModels.Scan;

namespace Gameton.WPF;

public partial class EnemyShipItem : UserControl
{
    public EnemyShipItem(ShipBase ship)
    {
        InitializeComponent();
        TextBlockHp.Text = $"HP: {ship.hp}/{ship.maxHp}";
        TextBlockSize.Text = $"Size: {ship.size}";
        TextBlockSpeed.Text = $"Speed: {ship.speed}";
        TextBlockDirection.Text = $"Direction: {ship.direction}";
    }
}