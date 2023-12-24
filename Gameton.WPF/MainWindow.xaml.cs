using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Gameton.DataModels;
using Gameton.Game;

namespace Gameton.WPF;

public partial class MainWindow : Window
{
    private PlayerShipController? CurrentShipController;
    
    public MainWindow(GameManager gameManager)
    {
        InitializeComponent();
        gameManager.OnUpdate += Update;
        MouseWheel += Image_MouseWheel;
        imageControl.MouseDown += Image_MouseDown;
        imageControl.MouseMove += Image_MouseMove;
        imageControl.MouseUp += Image_MouseUp;
        KeyDown += OnKeyDown;
    }
    
    private void Update(GameState gameState)
    {
        try
        {
            DrawGameMap(gameState.Map);
        }
        catch (Exception e)
        {
            App.Logger.LogError(nameof(DrawGameMap), e);
        }
    }
    
    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if(CurrentShipController == null)
            return;
        
        DirectionEnum? turnedTo = null;
        switch (e.Key)
        {
            case Key.W:
                if (CurrentShipController.TryTurn(DirectionEnum.north))
                    turnedTo = DirectionEnum.north;
                break;
            case Key.A:
                if (CurrentShipController.TryTurn(DirectionEnum.west))
                    turnedTo = DirectionEnum.west;
                break;
            case Key.S:
                if (CurrentShipController.TryTurn(DirectionEnum.south))
                    turnedTo = DirectionEnum.south;
                break;
            case Key.D:
                if (CurrentShipController.TryTurn(DirectionEnum.east))
                    turnedTo = DirectionEnum.east;
                break;
            case Key.Space:
                CurrentShipController.ChangeSpeed(0);
                break;
            case Key.Q:
                CurrentShipController.ChangeSpeed(-5);
                break;
            case Key.D1:
                CurrentShipController.ChangeSpeed(+1);
                break;
            case Key.D2:
                CurrentShipController.ChangeSpeed(+2);
                break;
            case Key.D3:
                CurrentShipController.ChangeSpeed(+3);
                break;
            case Key.D4:
                CurrentShipController.ChangeSpeed(+4);
                break;
        }
    }
    
    private void Image_MouseWheel(object sender, MouseWheelEventArgs e)
    {
        double zoom = e.Delta > 0 ? .1 : -.1;
        double minZoom = 0.1;
        ImageScale.ScaleX = Math.Max(minZoom, ImageScale.ScaleX + zoom);
        ImageScale.ScaleY = Math.Max(minZoom, ImageScale.ScaleY + zoom);
    }
    
    private Point mouseDownPoint;
    private Point translateOriginPoint;
    
    private void Image_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            imageControl.CaptureMouse();
            mouseDownPoint = e.GetPosition(this);
            translateOriginPoint = new Point(ImageTranslate.X, ImageTranslate.Y);
        }
    }

    private void Image_MouseMove(object sender, MouseEventArgs e)
    {
        if (imageControl.IsMouseCaptured)
        {
            Vector mouseMove = mouseDownPoint - e.GetPosition(this); 
            ImageTranslate.X = translateOriginPoint.X - mouseMove.X;
            ImageTranslate.Y = translateOriginPoint.Y - mouseMove.Y;
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

    private byte[]? _mapPixels = null;
    
    private void DrawGameMap(GameMap map)
    {
        int dpi = 96;
        var writableBitmap = new WriteableBitmap(map.Width, map.Height, dpi, dpi, PixelFormats.Bgr32, null);

        int bytesPerPixel = (writableBitmap.Format.BitsPerPixel + 7) / 8;
        int bufferLenght = map.Width * map.Height * bytesPerPixel;
        if(_mapPixels is null || _mapPixels.Length != bufferLenght)
            _mapPixels = new byte[bufferLenght];

        for (int y = 0; y < map.Height; y++)
        {
            for (int x = 0; x < map.Width; x++)
            {
                var currentCell = map.Data[y, x];
                Color color = GetColorForCell(currentCell);
                int index = y * map.Width * bytesPerPixel + x * bytesPerPixel;
                _mapPixels[index + 0] = color.B;
                _mapPixels[index + 1] = color.G;
                _mapPixels[index + 2] = color.R;
                _mapPixels[index + 3] = color.A;
            }
        }

        Int32Rect rect = new Int32Rect(0, 0, map.Width, map.Height);
        writableBitmap.WritePixels(rect, _mapPixels, map.Width * bytesPerPixel, 0);
        writableBitmap.Freeze();
        App.Current.Dispatcher?.Invoke(() => imageControl.Source = writableBitmap);
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