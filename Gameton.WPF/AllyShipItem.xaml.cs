using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Gameton.DataModels.Scan;
using Gameton.Game;

namespace Gameton.WPF;

public partial class AllyShipItem : UserControl
{
    public event EventHandler ItemClicked;

    public AllyShipItem(MyShipEntity myShip)
    {
        InitializeComponent();
        
        string id = "ID: " + myShip.id;
        string hp = "HP: "+ myShip.hp;
        string size = "Size: " +myShip.size;
        string speed = "Speed: "+ myShip.speed;
        TextBlock textBlock = new TextBlock();
        textBlock.Text += id + "\n" + hp + "\n" + size + "\n" + speed;
        
        this.MouseLeftButtonDown += AllyShipItem_MouseLeftButtonDown;
    }
    private void AllyShipItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        ItemClicked?.Invoke(this, EventArgs.Empty);
    }
}