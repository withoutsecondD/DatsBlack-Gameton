using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DrawMap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WriteableBitmap writableBitmap;

        public MainWindow()
        {
            InitializeComponent();
            MouseWheel += Image_MouseWheel;
            imageControl.MouseDown += Image_MouseDown;
            imageControl.MouseMove += Image_MouseMove;
            imageControl.MouseUp += Image_MouseUp;

            
            int width = 2000;
            int height = 2000;
            int dpiX = 96;
            int dpiY = 96;
            
            PixelFormat pixelFormat = PixelFormats.Bgr32;

            writableBitmap = new WriteableBitmap(width, height, dpiX, dpiY, pixelFormat, null);
            
            imageControl.Source = writableBitmap;
            string path = "C:\\Users\\user\\RiderProjects\\DatsBlack-Gameton\\DrawMap\\mapRender.txt";

            char[,] charArray = ReadFileToCharArray(path);
            
            DrawBitmapFromArray(charArray);
            
        }

        private Point origin;
        private Point start;
        
        private void Image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            
            double zoom = e.Delta > 0 ? .1 : -.1;
            
            ImageScale.ScaleX += zoom;
            ImageScale.ScaleY += zoom;
        }
        
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                imageControl.CaptureMouse();
                var tt = (TranslateTransform)((TransformGroup)imageControl.RenderTransform)
                    .Children.First(tr => tr is TranslateTransform);
                start = e.GetPosition(this);
                origin = new Point(tt.X, tt.Y);
            }
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            if (imageControl.IsMouseCaptured)
            {
                var tt = (TranslateTransform)((TransformGroup)imageControl.RenderTransform)
                    .Children.First(tr => tr is TranslateTransform);
                Vector v = start - e.GetPosition(this);
                tt.X = origin.X - v.X;
                tt.Y = origin.Y - v.Y;
            }
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                imageControl.ReleaseMouseCapture();
            }
        }
        
        private static char[,] ReadFileToCharArray(string filePath)
        {
            try
            {
                    
                string[] lines = File.ReadAllLines(filePath);
                    
                int rows = lines.Length;
                int columns = lines[0].Length;
                    
                char[,] charArray = new char[rows, columns];
                    
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        charArray[i, j] = lines[i][j];
                    }
                }

                return charArray;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }
        
        


        private void DrawBitmapFromArray(char[,] charArray)
        {
            int rows = charArray.GetLength(0);
            int cols = charArray.GetLength(1);
            
            int pixelSize = 1;

            int width = cols * pixelSize;
            int height = rows * pixelSize;

            Int32Rect rect = new Int32Rect(0, 0, width, height);
            int bytesPerPixel = (writableBitmap.Format.BitsPerPixel + 7) / 8;
            int stride = width * bytesPerPixel;
            byte[] pixels = new byte[stride * height];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    char currentChar = charArray[row, col];
                    Color color = GetColorForCharacter(currentChar);
                    for (int i = 0; i < pixelSize; i++)
                    {
                        for (int j = 0; j < pixelSize; j++)
                        {
                            int pixelRow = row * pixelSize + i;
                            int pixelCol = col * pixelSize + j;
                            int index = pixelRow * stride + pixelCol * bytesPerPixel;
                            CopyColorToPixel(color, pixels, index);
                        }
                    }
                }
            }

            writableBitmap.WritePixels(rect, pixels, stride, 0);
        }

        private Color GetColorForCharacter(char character)
        {
            switch (character)
            {
                case '-':
                    return Colors.DarkBlue;
                case '0':
                    return Colors.SaddleBrown;
                case 'E':
                    return Colors.Red;
                case 'T':
                    return Colors.Lime;
                default:
                    return Colors.White;
            }
        }

        private void CopyColorToPixel(Color color, byte[] pixels, int index)
        {
            byte[] colorBytes = { color.B, color.G, color.R, color.A };
            Array.Copy(colorBytes, 0, pixels, index, colorBytes.Length);
        }

        

    }

}