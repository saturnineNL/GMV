using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using GalaxyMapView.UserControls;
using GalaxyMapView._3D;

namespace GalaxyMapView.Rendering
{
    internal class RenderOverlay
    {

        public double centerX;
        public double centerY;

        public double elevation=0;
        public double azimuth=0;

        private Axis reference = new Axis();
        private Canvas plane = new Canvas();
        private Canvas edge = new Canvas();

        public static Canvas overlayDisplay=new Canvas();
        public static Canvas centerStar = new Canvas();
        public static Canvas overlayStar = new Canvas();

        private Point3D depthPoint;
        private SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(0xF0, 0x3D, 0x95, 0xDE));

        public RenderOverlay()
        {
            centerX = 250;
            centerY = 250;

            elevation = 0;
            azimuth = 0;
        }

        public void InitOverlayDisplay(Canvas attachToCanvas)
        {
            reference = InitAxis();

            overlayDisplay.Children.Add(reference.xAxe);
            overlayDisplay.Children.Add(reference.yAxe);

            overlayDisplay.Children.Add(centerStar);
           
            overlayDisplay.Children.Add(reference.zAxe);

            overlayDisplay.Children.Add(overlayStar);

            overlayDisplay.Children.Add(DrawPlane(0, 0));
            overlayDisplay.Children.Add(DrawEdge(0, 0));

            overlayDisplay.Margin=new Thickness(0);

            attachToCanvas.Children.Add(overlayDisplay);

           
        }

        public Canvas UpdateOverlay(double elevation, double azimuth,Point3D rotationPoint = default (Point3D))
        {
            overlayDisplay.Children.Clear();

            UpdateAxis(elevation,azimuth,rotationPoint);

            overlayDisplay.Children.Add(reference.xAxe);
            overlayDisplay.Children.Add(reference.yAxe);

            overlayDisplay.Children.Add(centerStar);

            overlayDisplay.Children.Add(reference.zAxe);

            overlayDisplay.Children.Add(overlayStar);

            overlayDisplay.Children.Add(DrawPlane(elevation, azimuth));
            overlayDisplay.Children.Add(DrawEdge(0, 0));

            Canvas returnCanvas = overlayDisplay;

            return returnCanvas;

        }

        private Canvas DrawPlane(double elevation,double azimuth)
        {
            plane.Children.Clear();

            double radius = centerX;
            double degree = 0;

            Point3D start;
            Point3D end;

            for (double degreeStep = 0; degreeStep < 180; degreeStep += 1)
            {
                Line line = new Line();

                degree = -degreeStep * (2 * Math.PI / 180);
                start = new Point3D(Math.Sin(degree) * radius, Math.Cos(degree) * radius, 0);

                degree = -(degreeStep + 1)*(2*Math.PI/180);
                end = new Point3D(Math.Sin(degree) * radius, Math.Cos(degree) * radius, 0);

                line = DrawLine(start, end, elevation, azimuth);

                line.Stroke = CalculateShade(brush,depthPoint.Z/radius,0.25);
                line.StrokeThickness = 2;

                plane.Children.Add(line);

            }

            return plane;
        }

        private Canvas DrawEdge(double elevation, double azimuth)
        {
            edge.Children.Clear();

            double radius = centerX;
            double degree = 0;

            Point3D start;
            Point3D end;

            for (double degreeStep = 0; degreeStep < 180; degreeStep += 1)
            {
                Line line1 = new Line();
                Line line2 = new Line();

                degree = -degreeStep * (2 * Math.PI / 180);
                start = new Point3D(Math.Sin(degree) * radius, 0, Math.Cos(degree) * radius);

                degree = -(degreeStep + 1) * (2 * Math.PI / 180);
                end = new Point3D(Math.Sin(degree) * radius, 0, Math.Cos(degree) * radius);

                line1 = DrawLine(start, end, 0, 0);
                line2 = DrawLine(start, end, 0, 0, -1);

                line1.Stroke = CalculateShade(brush, depthPoint.Y / radius, 0.25);
                line1.StrokeThickness = 2;

                line2.Stroke = CalculateShade(brush, depthPoint.Y / radius, 0.25);
                line2.StrokeThickness = 2;

                edge.Children.Add(line1);
                edge.Children.Add(line2);

            }

            return edge;
        }

        private SolidColorBrush CalculateShade(SolidColorBrush brush, double depth, double additive)
        {
            depth += additive;

            float colorA = brush.Color.ScA * (float)depth;
            float colorR = brush.Color.ScR * (float)depth; 
            float colorG = brush.Color.ScG * (float)depth;
            float colorB = brush.Color.ScB * (float)depth; 

            SolidColorBrush returnBrush = new SolidColorBrush(Color.FromScRgb(colorA,colorR,colorG,colorB));

            return returnBrush;
        }

        private Axis InitAxis()
        {

            reference.cPoint = new Point3D(0,0,0);
            reference.xPoint = new Point3D(50,0,0);
            reference.yPoint = new Point3D(0,50,0);
            reference.zPoint = new Point3D(0,0,50);

            reference.xAxe = DrawLine(reference.cPoint, reference.xPoint, 0, 0,1);
            reference.yAxe = DrawLine(reference.cPoint, reference.yPoint, 0, 0,1);
            reference.zAxe = DrawLine(reference.cPoint, reference.zPoint, 0, 0,-1);

            reference.xAxe.Stroke = new SolidColorBrush(Colors.Blue);
            reference.yAxe.Stroke = new SolidColorBrush(Colors.DarkGreen);
            reference.zAxe.Stroke = new SolidColorBrush(Colors.Crimson);

            reference.xAxe.StrokeThickness = 3;
            reference.yAxe.StrokeThickness = 3;
            reference.zAxe.StrokeThickness = 3;

            return reference;
        }

        private void UpdateAxis(double elevation, double azimuth,Point3D rotationPoint)
        {
            reference.xAxe = DrawLine(reference.cPoint, reference.xPoint, elevation, azimuth,1,rotationPoint);
            reference.yAxe = DrawLine(reference.cPoint, reference.yPoint, elevation, azimuth,1,rotationPoint);
            reference.zAxe = DrawLine(reference.cPoint, reference.zPoint, elevation, azimuth,-1,rotationPoint);

            reference.xAxe.Stroke = new SolidColorBrush(Colors.Blue);
            reference.yAxe.Stroke = new SolidColorBrush(Colors.DarkGreen);
            reference.zAxe.Stroke = new SolidColorBrush(Colors.Crimson);

            reference.xAxe.StrokeThickness = 3;
            reference.yAxe.StrokeThickness = 3;
            reference.zAxe.StrokeThickness = 3;
        }


        private Line DrawLine(Point3D start, Point3D end,double elevation,double azimuth,double reverse =1,Point3D rotationPoint = default(Point3D))
        {
            Draw3D D3D = new Draw3D();

            Line returnLine = new Line();

            Point3D startPoint = D3D.DrawAzimuth(start, elevation, azimuth,rotationPoint);
            Point3D endPoint = D3D.DrawAzimuth(end, elevation, azimuth,rotationPoint);

            returnLine.X1 = (startPoint.X*reverse)+centerX;
            returnLine.X2 = (endPoint.X*reverse)+centerY;

            returnLine.Y1 = (startPoint.Y*reverse)+centerY;
            returnLine.Y2 = (endPoint.Y*reverse)+centerY;

            depthPoint = startPoint;

            return returnLine;   
        }

    }

    internal class Axis
    {
        public Line xAxe { get; set; }
        public Line yAxe { get; set; }
        public Line zAxe { get; set; }

        public Point3D cPoint { get; set; }
        public Point3D xPoint { get; set; }
        public Point3D yPoint { get; set; }
        public Point3D zPoint { get; set; }

        public double centerX { get; set; }
        public double centerY { get; set; }

        public double elevation { get; set; }
        public double azimuth { get; set; }

    }

}