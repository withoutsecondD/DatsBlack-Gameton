using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Gameton.Game;

namespace Gameton.WPF;

public partial class MainWindow : Window
{
    public MainWindow(GameManager gameManager)
    {
        InitializeComponent();
        gameManager.OnUpdate += Update;
        MouseWheel += Image_MouseWheel;
        imageControl.MouseDown += Image_MouseDown;
        imageControl.MouseMove += Image_MouseMove;
        imageControl.MouseUp += Image_MouseUp;
    }

    private void Update(GameState gameState)
    {
        DrawGameMap(gameState.Map);
    }
    
    private void Image_MouseWheel(object sender, MouseWheelEventArgs e)
    {
        double zoom = e.Delta > 0 ? .1 : -.1;
        double minZoom = 0.1;
        ImageScale.ScaleX = Math.Max(minZoom, ImageScale.ScaleX + zoom);
        ImageScale.ScaleY = Math.Max(minZoom, ImageScale.ScaleY + zoom);
    }
    
    private Point mouseDownPoint;
    
    private void Image_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            imageControl.CaptureMouse();
            mouseDownPoint = e.GetPosition(this);
        }
    }

    private void Image_MouseMove(object sender, MouseEventArgs e)
    {
        if (imageControl.IsMouseCaptured)
        {
            Vector mouseMove = e.GetPosition(this) - mouseDownPoint; 
            ImageTranslate.X += mouseMove.X;
            ImageTranslate.Y += mouseMove.Y;
        }
    }

    public void MapMoveTo(int x, int y)
    {
        ImageTranslate.X = x * ImageScale.ScaleX;
        ImageTranslate.Y = y * ImageScale.ScaleY;
    }

    private void Image_MouseUp(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            imageControl.ReleaseMouseCapture();
        }
    }
        
    private void DrawGameMap(GameMap map)
    {
        int dpi = 96;
        var writableBitmap = new WriteableBitmap(map.Width, map.Height, dpi, dpi, PixelFormats.Bgr32, null);

        int bytesPerPixel = (writableBitmap.Format.BitsPerPixel + 7) / 8;
        byte[] pixels = new byte[map.Width * map.Height * bytesPerPixel];

        for (int y = 0; y < map.Height; y++)
        {
            for (int x = 0; x < map.Width; x++)
            {
                var currentCell = map.Data[y, x];
                Color color = GetColorForCell(currentCell);
                int index = y * map.Width * bytesPerPixel + x * bytesPerPixel;
                byte[] colorBytes = { color.B, color.G, color.R, color.A };
                Array.Copy(colorBytes, 0, pixels, index, colorBytes.Length);
            }
        }

        Int32Rect rect = new Int32Rect(0, 0, map.Width, map.Height);
        writableBitmap.WritePixels(rect, pixels, map.Width * bytesPerPixel, 0);
        imageControl.Source = writableBitmap;
    }

    private Color GetColorForCell(GameMapCell cell) =>
        cell switch
        {
            GameMapCell.Space => new Color { R = 10, B = 40, G = 10 },
            GameMapCell.Island => Colors.Gray,
            GameMapCell.Enemy => Colors.Red,
            GameMapCell.Ally => Colors.Lime,
            _ => Colors.Black
        };
}