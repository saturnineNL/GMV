using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using GalaxyMapView.Rendering;
using GalaxyMapView.UserControls;
using GalaxyMapView._3D;

namespace GalaxyMapView.StarSystems
{
    public class Star
    {
        private SolidColorBrush foregroundBrush = new SolidColorBrush(Color.FromArgb(0xF0, 0x3D, 0x95, 0xDE));
        private SolidColorBrush fontBrush = new SolidColorBrush(Color.FromArgb(0xFF,0x7D,0xB5,0xDE));

        private float renderA;
        private float renderR;
        private float renderG;
        private float renderB;
        private float renderD;

        public Star()
        {

            starColor = foregroundBrush;

            starGFX = new Ellipse();
            starSEL = new Ellipse();

            starCanvas = new Canvas();
            starLabel = new Label();

            starSEL.Visibility = Visibility.Hidden;
            starGFX.Fill = foregroundBrush;

            starLabel.FontFamily = new FontFamily(new Uri("pack://application:,,,/Fonts/"), "./#Euro Caps");
            starLabel.Foreground = fontBrush;

            starCanvas.Children.Add(starLabel);
            starCanvas.Children.Add(starGFX);
            starCanvas.Children.Add(starSEL);

            movePoint = new Point3D(0,0,0);
            rotaPoint = new Point3D(0,0,0);

            starCanvas.MouseLeftButtonDown += SetSelection;
            starCanvas.MouseRightButtonDown +=TargetSelection;

            starCanvas.MouseEnter += ShowInfoSelection;
            starCanvas.MouseLeave += HideInfoSelection;
            
        }

        private void HideInfoSelection(object sender, MouseEventArgs e)
        {
            starColor = fontBrush;
            ViewController.starOverlay = false;
           
        }

        private void ShowInfoSelection(object sender, MouseEventArgs e)
        {
            starColor = new SolidColorBrush(Colors.Blue);

            ViewController.starOverlayID = starID;
            ViewController.starOverlay = true;

        }

        private void TargetSelection(object sender, MouseButtonEventArgs e)
        {
            targetSystem = !targetSystem;
            selectSystem = false;

            ViewController.starSelection = true;

            if (targetSystem)
            {
                if (!ViewController.targetedStars.Contains(starID)) ViewController.targetedStars.Add(starID);
                starSEL.Visibility = Visibility.Visible;
            }

            if (!targetSystem)
            {
                int index = ViewController.targetedStars.FindIndex(fetchID => fetchID == starID);
                ViewController.targetedStars.RemoveAt(index);

                starSEL.Visibility = Visibility.Hidden;
            }

        }

        private void SetSelection(object sender, RoutedEventArgs e)
        {           

            selectSystem = !selectSystem;
            targetSystem = false;

            ViewController.starSelection = true;

            if (selectSystem)
            {
                if (!ViewController.selectedStars.Contains(starID)) ViewController.selectedStars.Add(starID);
                starSEL.Visibility = Visibility.Visible;
            }

            if (!selectSystem)
            {

                int index = ViewController.selectedStars.FindIndex(fetchID => fetchID == starID);
                ViewController.selectedStars.RemoveAt(index);

                starSEL.Visibility = Visibility.Hidden;
            }

        }

        public void InitStar()
        {
            Draw3D D3D = new Draw3D();

            Point3D projectPoint = new Point3D();

            starGFX.Width = 2;
            starGFX.Height = 2;

            scale = (centerX / zoom);

            if (scale < 1) scale = 1;

            scalePoint = new Point3D(scale, scale, scale);

            projectPoint = D3D.DrawAzimuth(currentPoint, elevation, azimuth, rotaPoint, movePoint, scalePoint);

            renderDepth = (projectPoint.Z / centerX);

            distanceDepth = (distance/zoom);

            renderD = starColor.Color.ScA * (float)(distanceDepth);

        }

        public void BuildName()
        {
            originalName = name;

            string[] splitter = name.Split(' ');

            string buildName = String.Empty;

            int length = 0;

            displayWidth = 1;
            displayHeight = 1;

            for(int wordCount=0;wordCount<splitter.Length;wordCount+=1)
            {
                if (length < 14)
                {
                    buildName += splitter[wordCount]+" ";

                    length += splitter[wordCount].Length+1;

                    if (displayWidth < length) displayWidth = length;
                }

                else
                {
                    buildName += "\n";

                    displayHeight += 1;

                    if (wordCount > 0)
                    {
                        wordCount -= 1;}

                    length = 0;
                }

            }

            displayName= buildName;
  
        }

        public void ShowNames()
        {
            starLabel.Visibility = Visibility.Hidden;

            if ((size >= 11 && renderDepth > 0 && !centerSystem) || selectSystem)
            {
                starLabel.Visibility = Visibility.Visible;

                starLabel.FontSize = size*0.75;

                starLabel.Width = (size*0.5)*displayWidth;
                starLabel.Height = (size*1.65)*displayHeight;

                starLabel.HorizontalContentAlignment = HorizontalAlignment.Left;
                starLabel.VerticalContentAlignment = VerticalAlignment.Center;

                starLabel.Margin = new Thickness(-(starGFX.Width/2) + size,-(starGFX.Height/2) - ((size/3)*displayHeight), 0, 0);

                starLabel.Content = displayName;
            }

            if (centerSystem || targetSystem)
            {
                starLabel.Visibility = Visibility.Visible;

                starLabel.FontSize = 24 * 0.75;

                starLabel.Width = 24 * displayWidth;
                starLabel.Height = (24 * 1.5) * displayHeight;

                starLabel.HorizontalContentAlignment = HorizontalAlignment.Left;
                starLabel.VerticalContentAlignment = VerticalAlignment.Top;

                starLabel.Margin = new Thickness(-(starGFX.Width / 2) + 24,-(starGFX.Height / 2) - ((24 / 3) * displayHeight), 0, 0);

                starLabel.Content = displayName;

            }

        }

        public void SetSize()
        {
           
            double zoomFactor =Math.Abs((1.75 - renderDepth)/1.5);

            size = 2 + ((scale/zoomFactor)*0.75);

            starGFX.Width = size;
            starGFX.Height = size;
            starGFX.StrokeThickness = size/6;

            starSEL.Width = starGFX.Width * 1.5;
            starSEL.Height = starGFX.Height * 1.5;

            if (centerSystem || targetSystem)
            {
                size = 18;

                starGFX.Width = size;
                starGFX.Height = size;
                starGFX.StrokeThickness = size / 6;

                starSEL.Width = starGFX.Width * 1.5;
                starSEL.Height = starGFX.Height * 1.5;

                starSEL.StrokeThickness = 3;
                starSEL.Visibility = Visibility.Visible;

            }

            else if (selectSystem)
            {
                
                starSEL.StrokeThickness = 2;
                starSEL.Visibility = Visibility.Visible;

            }

            starGFX.Margin = new Thickness(-(starGFX.Width / 2), -(starGFX.Height / 2), 0, 0);
            starSEL.Margin = new Thickness(-(starGFX.Width * 1.5) / 2, -(starGFX.Height * 1.5) / 2, 0, 0);

        }

        public void setOverlay()
        {
            starGFX.Width = size;
            starGFX.Height = size;
            starGFX.StrokeThickness = size / 6;

            starSEL.Width = starGFX.Width * 1.5;
            starSEL.Height = starGFX.Height * 1.5;

            starGFX.Margin = new Thickness(-(starGFX.Width / 2), -(starGFX.Height / 2), 0, 0);
            starSEL.Margin = new Thickness(-(starGFX.Width * 1.5) / 2, -(starGFX.Height * 1.5) / 2, 0, 0);

            starLabel.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x09, 0x08, 0x29));

        }

        public void SetColor()
        {

            renderA = starColor.Color.ScA * (float)(renderDepth + 0.35);
            renderR = starColor.Color.ScR * (float)(renderDepth + 0.35);
            renderG = starColor.Color.ScG * (float)(renderDepth + 0.35);
            renderB = starColor.Color.ScB * (float)(renderDepth + 0.35);
            
            starGFX.Fill = new SolidColorBrush(Color.FromScRgb(renderA, renderR, renderG, renderB));

            starGFX.Stroke = new SolidColorBrush(Color.FromScRgb(renderA * 0.75F, renderR * 0.25F, renderG * 0.25F, renderB * 0.25F));

            starLabel.Foreground = new SolidColorBrush(Color.FromScRgb(1.25F-(renderD*0.75F), 1.25F - (renderD * 0.75F), 1.25F - (renderD * 0.75F), 1.25F - (renderD * 0.75F)));

            if (centerSystem)
            {
                starGFX.Fill = new SolidColorBrush(Colors.AliceBlue); 
                starSEL.Stroke = new SolidColorBrush(Colors.AliceBlue); 
                starLabel.Foreground = new SolidColorBrush(Colors.AliceBlue);
    
            }

            if (selectSystem)
            {
                starGFX.Fill = new SolidColorBrush(Colors.Crimson); 
                starSEL.Stroke = new SolidColorBrush(Colors.Crimson); 
                starLabel.Foreground = new SolidColorBrush(Colors.Crimson);

                starLabel.Visibility = Visibility.Visible;
            }

            if (targetSystem)
            {
                starGFX.Fill = new SolidColorBrush(Colors.GreenYellow);
                starSEL.Stroke = new SolidColorBrush(Colors.GreenYellow);
                starLabel.Foreground = new SolidColorBrush(Colors.GreenYellow);

                starLabel.Visibility = Visibility.Visible;

            }

        }

        public Canvas Project3D()
        {
            Draw3D D3D = new Draw3D();

            projectPoint = new Point3D();

            if (zoom < 1) zoom = 1;            

            scale = (centerX / zoom);

            scalePoint = new Point3D(scale, scale, scale);

            projectPoint = D3D.DrawAzimuth(currentPoint, elevation, azimuth, rotaPoint, movePoint, scalePoint);

            renderDepth = (projectPoint.Z / centerX);

            //starGFX.Margin = new Thickness(-(starGFX.Width / 2), -(starGFX.Height / 2), 0, 0);
            
            //starSEL.Margin = new Thickness(-(starGFX.Width * 1.5) / 2, -(starGFX.Height * 1.5) / 2, 0, 0);

            starCanvas.Margin = new Thickness(centerX +projectPoint.X, centerY + projectPoint.Y, 0, 0);

            return starCanvas;
        }

        public int starID { get; set; }
        public string name { get; set; }
        public string displayName { get; set; }
        public string originalName { get; set; }
        public int displayWidth { get; set; }
        public int displayHeight { get; set; }
        public Ellipse starGFX { get; set; }
        public Ellipse starSEL { get; set; }

        public Label starLabel { get; set; }
        public Canvas starCanvas { get; set; }
        
        public Point3D currentPoint { get; set; }
        public Point3D originPoint { get; set; }
        public Point3D projectPoint { get; set; }
        public Point3D rotaPoint { get; set; }
        public Point3D movePoint { get; set; }
        public Point3D scalePoint { get; set; }

        public bool centerSystem { get; set; }
        public bool selectSystem { get; set; }
        public bool targetSystem { get; set; }

        public double distance { get; set; }
        public double distanceDepth { get; set; }
        public double renderDepth { get; set; }
        
        public SolidColorBrush starColor { get; set; }

        public double centerX { get; set; }
        public double centerY { get; set; }

        public double azimuth { get; set; }
        public double elevation { get; set; }

        public double size { get; set; }
        public double zoom { get; set; }
        public double scale { get; set; }
    }
}
