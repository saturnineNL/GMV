using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Windows.Threading;
using GalaxyMapView.DataSource;
using GalaxyMapView.StarSystems;
using GalaxyMapView.UserControls;
using GalaxyMapView._3D;

namespace GalaxyMapView.Rendering
{
    public class RenderStarSystems
    {

        

        public StarSystem currentSystem;
        public StarSystem moveToSystem;

        public Point3D currentLocation;
        private Point3D destinationLocation;

        public Canvas galaxyDisplay = new Canvas();
        
        public Label headerLabel = new Label();     
     
        public double elevation = 0;
        public double azimuth = 0;
        public double zoom = 64;

        public double centerX;
        public double centerY;

        public Point3D rotatorPoint = new Point3D(0,0,0);
        
        private double movementX;
        private double movementY;
        private double movementZ;

        double zoomFactor = 0;

        private bool onTheMove = false;
        private double moveCounter;

        public RenderStarSystems()
        {

            centerX = 250;
            centerY = 250;

            elevation = 0;
            azimuth = 0;
        }

        public void InitGalaxyDisplay(Canvas attachToCanvas)
        {

            galaxyDisplay.Width = 500;
            galaxyDisplay.Height = 500;
            galaxyDisplay.Margin=new Thickness(0);
            
            galaxyDisplay.HorizontalAlignment = HorizontalAlignment.Left;
            galaxyDisplay.VerticalAlignment = VerticalAlignment.Top;

            galaxyDisplay.Background=new SolidColorBrush(Color.FromArgb(0xFF, 0x09, 0x08, 0x29));               

            headerLabel.Width = galaxyDisplay.Width;
            headerLabel.Height = 30;
            headerLabel.HorizontalAlignment = HorizontalAlignment.Left;
            headerLabel.VerticalAlignment = VerticalAlignment.Top;
            headerLabel.Margin = new Thickness(0);

            headerLabel.Foreground = new SolidColorBrush(Colors.White);

            attachToCanvas.Children.Add(galaxyDisplay);       

        }


        public void RenderStars(List<Star> starSystemCollection)
        {

            galaxyDisplay.Children.Add(headerLabel);

            foreach (var star in starSystemCollection)
            {

                star.centerX = centerX;
                star.centerY = centerY;

                star.rotaPoint = rotatorPoint;

                star.elevation = elevation;
                star.azimuth = azimuth;

                star.zoom = zoom;

                star.InitStar();

                star.BuildName();

                if (star.starID == currentSystem.ID && star.distance == 0)
                {

                  if(RenderOverlay.centerStar.Children.Count == 0) RenderOverlay.centerStar.Children.Add(star.starCanvas);

                }

                else galaxyDisplay.Children.Add(star.starCanvas);

            }

            UpdateRender(starSystemCollection);
        }

        public void UpdateRender(List<Star> starSystemCollection)
        {

            zoomFactor = 0;

            foreach (var star in starSystemCollection.OrderBy(depth => depth.currentPoint.Z))
            {
                star.centerSystem = (star.starID == currentSystem.ID && star.distance == 0);

                star.rotaPoint = rotatorPoint;

                star.elevation = elevation;
                star.azimuth = azimuth;

                star.Project3D();

                star.SetSize();

                star.SetColor();

                star.ShowNames();

             //   headerLabel.Content = currentSystem.Name + " E: " + elevation.ToString("F") + " A: " + azimuth.ToString("F") + " Z: " + star.zoom + " SC: " + galaxyDisplay.Children.Count + " TC: " + starSystemCollection.Count + " DD: "  
              //      + star.distanceDepth.ToString("F") + " ST: " + currentSystem.StationList.Count.ToString("F");
            }
        }

        public void initMovement()
        {
          //  currentLocation = starSystemSet.GetStarSystemLocation(currentSystem);
          //  destinationLocation = starSystemSet.GetStarSystemLocation(moveToSystem);

            Point3D movement = new Point3D(destinationLocation.X - currentLocation.X, destinationLocation.Y - currentLocation.Y, destinationLocation.Z - currentLocation.Z);

            moveCounter = 15;

            movementX = movement.X / moveCounter;
            movementY = movement.Y / moveCounter;
            movementZ = movement.Z / moveCounter;

            onTheMove = true;
        }

        public void MoveStars()
        {
          //  starSystemList = starSystemSet.UpdateStarSystemCollection(currentLocation, zoom);

         //   starSystemCollection = starSystemSet.BuildStarCollection(starSystemList);

          //  BuildOverlay();

          //  BuildPlane();

         //   RenderStars();

         //   UpdateRender();
        }
    }
}
