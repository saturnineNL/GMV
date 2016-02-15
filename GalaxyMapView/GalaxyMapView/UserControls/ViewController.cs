using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using GalaxyMapView.DataSource;
using GalaxyMapView.Rendering;
using GalaxyMapView.StarSystems;

namespace GalaxyMapView.UserControls
{

    class ViewController
    {

        RenderStarSystems renderStarSystems = new RenderStarSystems();
        RenderOverlay renderOverlay = new RenderOverlay();

        static Grid controllerGrid = new Grid();

        public static List<int> selectedStars = new List<int>();
        public static List<int> targetedStars = new List<int>(); 

        public static bool starSelection = false;
        public static bool starOverlay = false;
        public static int starOverlayID = -1;

        private StarSystemSet starSystemSet = new StarSystemSet();

        private List<StarSystem> starSystemList;
        private List<Star> starSystemCollection;
       
        private StarSystem currentSystem;
        private Point3D currentLocation;

        private bool leftMouseButton = false;
        private bool rightMouseButton = false;

        private Point mouseReferencePoint = new Point();

        private double elevation = 0;
        private double azimuth = 0;
        private double zoom = 64;

        private DispatcherTimer animator = new DispatcherTimer();

        public ViewController()
        {

            renderStarSystems.InitGalaxyDisplay(GalaxyMap.viewerCanvas);
            renderOverlay.InitOverlayDisplay(GalaxyMap.viewerCanvas);

            InitStarSystemCollection();

            renderStarSystems.currentSystem = currentSystem;            
            renderStarSystems.RenderStars(starSystemCollection);

            AnimateStars();

            MouseControls();
            
        }

        private void InitStarSystemCollection()
        {
            currentSystem = starSystemSet.GetStarSystemName("Beta Volantis");

            currentLocation = starSystemSet.GetStarSystemLocation(currentSystem);

            starSystemList = starSystemSet.UpdateStarSystemCollection(currentLocation, zoom);

            starSystemSet.UpdateStationSystemCollection(starSystemList);

            starSystemList.OrderBy(p => p.Location.Z);

            starSystemCollection = starSystemSet.BuildStarCollection(starSystemList);
        }

        private void UpdateSystemCollection()
        {
            
        }

        public void AnimateStars()
        {
            animator.Interval = TimeSpan.FromMilliseconds(50);
            animator.Tick += Animator_Tick;
            animator.Start();
        }

        public void Animator_Tick(object sender, EventArgs e)
        {
            if (!starOverlay)
            {

                RenderOverlay.overlayStar.Children.Clear();

                double rotateStep = 0.36;

                renderStarSystems.rotatorPoint.Z += rotateStep;

                if (renderStarSystems.rotatorPoint.Z > 360)
                {
                    renderStarSystems.rotatorPoint.Z = 0;
                }
            }

            if (starOverlay && starOverlayID != -1)
            {
                int index = starSystemCollection.FindIndex(fetchID => fetchID.starID == starOverlayID);

                if (index != -1)
                {
                    Star overlayStar = new Star();

                    overlayStar.starID = starSystemCollection[index].starID;
                    overlayStar.name = starSystemCollection[index].name;

                    overlayStar.centerX = starSystemCollection[index].centerX;
                    overlayStar.centerY = starSystemCollection[index].centerY;

                    overlayStar.currentPoint = starSystemCollection[index].currentPoint;
                    overlayStar.rotaPoint = starSystemCollection[index].rotaPoint;

                    overlayStar.elevation = starSystemCollection[index].elevation;
                    overlayStar.azimuth = starSystemCollection[index].azimuth;

                    overlayStar.zoom = starSystemCollection[index].zoom;

                    overlayStar.InitStar();

                    overlayStar.BuildName();

                    overlayStar.Project3D();

                    overlayStar.size = 32;

                    overlayStar.setOverlay();

                    overlayStar.ShowNames();

                    overlayStar.SetColor();

                    RenderOverlay.overlayStar.Children.Clear();
                    RenderOverlay.overlayStar.Children.Add(overlayStar.starCanvas);

                    renderOverlay.UpdateOverlay(elevation, azimuth);
                    renderStarSystems.headerLabel.Content = overlayStar.originalName;
            }

            starOverlayID = -1;

            }
            renderStarSystems.UpdateRender(starSystemCollection);

            renderOverlay.UpdateOverlay(elevation, azimuth, renderStarSystems.rotatorPoint);
            
        }

        public void MouseControls()
        {
            renderStarSystems.galaxyDisplay.MouseLeftButtonDown += GalaxyDisplay_MouseLeftButtonDown;
            renderStarSystems.galaxyDisplay.MouseLeftButtonUp += GalaxyDisplay_MouseLeftButtonUp;

            renderStarSystems.galaxyDisplay.MouseRightButtonDown += GalaxyDisplay_MouseRightButtonDown;
            renderStarSystems.galaxyDisplay.MouseRightButtonUp += GalaxyDisplay_MouseRightButtonUp;

            renderStarSystems.galaxyDisplay.MouseMove += GalaxyDisplay_MouseMove;
            renderStarSystems.galaxyDisplay.MouseWheel += GalaxyDisplay_MouseWheel;
        }

        private void GalaxyDisplay_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            leftMouseButton = false;
            starSelection = false;
            
        }

        private void GalaxyDisplay_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!leftMouseButton && !starSelection)
            {
                mouseReferencePoint.X = Mouse.GetPosition(renderStarSystems.galaxyDisplay).X - (renderStarSystems.galaxyDisplay.Width / 2);
                mouseReferencePoint.Y = Mouse.GetPosition(renderStarSystems.galaxyDisplay).Y - (renderStarSystems.galaxyDisplay.Height / 2);

                leftMouseButton = true;
            }
            
        }

        private void GalaxyDisplay_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            rightMouseButton = false;
            starSelection = false;
        }

        private void GalaxyDisplay_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!rightMouseButton && !starSelection)
            {
                mouseReferencePoint.X = Mouse.GetPosition(renderStarSystems.galaxyDisplay).X - (renderStarSystems.galaxyDisplay.Width / 2);
                mouseReferencePoint.Y = Mouse.GetPosition(renderStarSystems.galaxyDisplay).Y - (renderStarSystems.galaxyDisplay.Height / 2);

                rightMouseButton = true;

            }
        }

        private void GalaxyDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            Point movePoint = new Point();

            movePoint.X = Mouse.GetPosition(renderStarSystems.galaxyDisplay).X - (renderStarSystems.galaxyDisplay.Width / 2);
            movePoint.Y = Mouse.GetPosition(renderStarSystems.galaxyDisplay).Y - (renderStarSystems.galaxyDisplay.Width / 2);

            double el = 0;
            double az = 0;

            if (leftMouseButton && !rightMouseButton)
            {
                az = ((mouseReferencePoint.X - movePoint.X) / renderStarSystems.galaxyDisplay.Width) * 128;
                el = ((mouseReferencePoint.Y - movePoint.Y) / renderStarSystems.galaxyDisplay.Height) * 128;

                elevation += el;
                azimuth += az;

                elevation += el;
                azimuth += az;

                if (elevation + el < -90) elevation = -90;
                if (elevation + el > 90) elevation = 90;
                  
                if (azimuth + az < -180) azimuth = -180;
                if (azimuth + az > 180) azimuth = 180;

                GalaxyMap.viewerCanvas.Children.RemoveAt(1);
                GalaxyMap.viewerCanvas.Children.Insert(1,renderOverlay.UpdateOverlay(elevation,azimuth,renderStarSystems.rotatorPoint));

                renderStarSystems.elevation = elevation;
                renderStarSystems.azimuth = azimuth;

                renderStarSystems.UpdateRender(starSystemCollection);

            }

            mouseReferencePoint.X = movePoint.X;
            mouseReferencePoint.Y = movePoint.Y;
        }

        private void GalaxyDisplay_MouseWheel(object sender, MouseWheelEventArgs e)
        {

            double zoomFactor = 16;

            double deltaBase = Math.Ceiling(renderStarSystems.zoom / zoomFactor);

            double delta = e.Delta / 120;

            renderStarSystems.zoom -= delta * deltaBase;

            if (renderStarSystems.zoom > 128)
            {
                renderStarSystems.zoom = 128;
            }

            if (renderStarSystems.zoom < 4)
            {
                renderStarSystems.zoom = 4;
            }

            starSystemList = starSystemSet.UpdateStarSystemCollection(currentSystem.ID, renderStarSystems.zoom);
            starSystemList.OrderBy(p => p.Location.Z);

            starSystemCollection = starSystemSet.BuildStarCollection(starSystemList);

            renderStarSystems.galaxyDisplay.Children.Clear();

            renderStarSystems.RenderStars(starSystemCollection);

        }

    }
    
}
