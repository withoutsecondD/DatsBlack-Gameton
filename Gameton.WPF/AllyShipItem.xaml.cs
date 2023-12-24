using System.Windows.Controls;
using System.Windows.Input;
using Gameton.Game;

namespace Gameton.WPF;

public partial class AllyShipItem : UserControl
{
    public event EventHandler? ItemClicked;
    
    
    public MyShipEntity Ship;

    public AllyShipItem(MyShipEntity ship)
    {
        InitializeComponent();
        
        TextBlockId.Text = $"ID: {ship.id}";
        TextBlockHp.Text = $"HP: {ship.hp}/{ship.maxHp}";
        TextBlockSize.Text = $"Size: {ship.size}";
        TextBlockSpeed.Text = $"Speed: {ship.speed}";
        TextBlockDirection.Text = $"Direction: {ship.direction}";
        Ship = ship;
        MouseLeftButtonDown += AllyShipItem_MouseLeftButtonDown;
    }
    private void AllyShipItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        ItemClicked?.Invoke(this, EventArgs.Empty);
    }
}