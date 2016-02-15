using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using GalaxyMapView.DataSource;
using GalaxyMapView.Rendering;
using GalaxyMapView.StarSystems;

namespace GalaxyMapView.UserControls
{

    public partial class GalaxyMap
    {
       // public static RenderStarSystems starSystemView = new RenderStarSystems();

        public static Canvas viewerCanvas = new Canvas();

       //// private double rotateStep = 0;

       // private Point mouseReferencePoint = new Point();

       // public static bool leftMouseButton = false;
       // public static bool rightMouseButton = false;
       // public static bool starSelection = false;

      //  public static StarSystem selectedSystem;

        public GalaxyMap()
        {

            InitializeComponent();

          //  GalaxyMapMouseControls();

           // viewerCanvas = starSystemView.InitView(viewerCanvas);

            GalaxyMapGrid.Children.Add(viewerCanvas);

            //   starSystemView.BuildOverlay();

            //    starSystemView.BuildPlane();

            //   starSystemView.RenderStars();

            //   starSystemView.AnimateStars();

            //   selectedSystem = starSystemView.currentSystem;

        }

        //public void GalaxyMapMouseControls()
        //{
        //    starSystemView.GalaxyMapCanvas.MouseLeftButtonDown += GalaxyMap_MouseLeftButtonDown;
        //    starSystemView.GalaxyMapCanvas.MouseLeftButtonUp += GalaxyMap_MouseLeftButtonUp;

        //    starSystemView.GalaxyMapCanvas.MouseRightButtonDown += GalaxyMap_MouseRightButtonDown;
        //    starSystemView.GalaxyMapCanvas.MouseRightButtonUp += GalaxyMap_MouseRightButtonUp;

        //    starSystemView.GalaxyMapCanvas.MouseMove += GalaxyMap_MouseMove;
        //    starSystemView.GalaxyMapCanvas.MouseWheel += GalaxyMap_MouseWheel;
        //}

        //private void GalaxyMap_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    leftMouseButton = false;
        //    starSelection = false;
        //}

        //private void GalaxyMap_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (!leftMouseButton && !starSelection)
        //    {
        //        mouseReferencePoint.X = Mouse.GetPosition(starSystemView.GalaxyMapCanvas).X -
        //                                (starSystemView.GalaxyMapCanvas.Width/2);
        //        mouseReferencePoint.Y = Mouse.GetPosition(starSystemView.GalaxyMapCanvas).Y -
        //                                (starSystemView.GalaxyMapCanvas.Height/2);

        //        leftMouseButton = true;
        //    }
        //}

        //private void GalaxyMap_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    rightMouseButton = false;
        //    starSelection = false;
        //}

        //private void GalaxyMap_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (!rightMouseButton && !starSelection)
        //    {
        //        mouseReferencePoint.X = Mouse.GetPosition(starSystemView.GalaxyMapCanvas).X -
        //                                (starSystemView.GalaxyMapCanvas.Width/2);
        //        mouseReferencePoint.Y = Mouse.GetPosition(starSystemView.GalaxyMapCanvas).Y -
        //                                (starSystemView.GalaxyMapCanvas.Height/2);

        //        rightMouseButton = true;

        //    }
        //}

        //private void GalaxyMap_MouseMove(object sender, MouseEventArgs e)
        //{
        //    Point movePoint = new Point();

        //    movePoint.X = Mouse.GetPosition(starSystemView.GalaxyMapCanvas).X - (starSystemView.GalaxyMapCanvas.Width/2);
        //    movePoint.Y = Mouse.GetPosition(starSystemView.GalaxyMapCanvas).Y - (starSystemView.GalaxyMapCanvas.Width/2);

        //    double el = 0;
        //    double az = 0;

        //    if (leftMouseButton && !rightMouseButton)
        //    {
        //        az = ((mouseReferencePoint.X - movePoint.X)/starSystemView.GalaxyMapCanvas.Width)*128;
        //        el = ((mouseReferencePoint.Y - movePoint.Y)/starSystemView.GalaxyMapCanvas.Height)*128;

        //        starSystemView.elevation += el;
        //        starSystemView.azimuth += az;

        //        if (starSystemView.elevation + el < -90) starSystemView.elevation = -90;
        //        if (starSystemView.elevation + el > 90) starSystemView.elevation = 90;

        //        if (starSystemView.azimuth + az < -180) starSystemView.azimuth = -180;
        //        if (starSystemView.azimuth + az > 180) starSystemView.azimuth = 180;

        //        starSystemView.OverlayDisplay.Children.Clear();

        //        starSystemView.StarDisplay.Children.Clear();

        //        starSystemView.BuildOverlay();

        //        starSystemView.BuildPlane();

        //        starSystemView.RenderStars();

        //        starSystemView.UpdateRender();

        //        selectedSystem = starSystemView.currentSystem;
        //    }

        //    double moveX = 0;
        //    double moveY = 0;
        //    double moveZ = 0;

        //    if (rightMouseButton)
        //    {
        //        if (!leftMouseButton)
        //            moveZ = ((mouseReferencePoint.Y - movePoint.Y)/starSystemView.GalaxyMapCanvas.Height)*128;

        //        else
        //        {
        //            moveX = ((mouseReferencePoint.X - movePoint.X)/starSystemView.GalaxyMapCanvas.Width)*128;
        //            moveY = ((mouseReferencePoint.Y - movePoint.Y)/starSystemView.GalaxyMapCanvas.Height)*128;
        //        }

        //        Point3D movement = new Point3D(starSystemView.currentSystem.Location.X - moveX,
        //            starSystemView.currentSystem.Location.Y - moveY, starSystemView.currentSystem.Location.Z - moveZ);

        //        starSystemView.starSystemList = starSystemView.starSystemSet.UpdateStarSystemCollection(movement,
        //            starSystemView.zoom);

        //        starSystemView.starSystemList.OrderBy(p => p.Location.Z);

        //        starSystemView.starSystemCollection = starSystemView.starSystemSet.BuildStarCollection(starSystemView.starSystemList);

        //        starSystemView.currentSystem.Location = new Point3D(starSystemView.currentSystem.Location.X - moveX,starSystemView.currentSystem.Location.Y - moveY,starSystemView.currentSystem.Location.Z - moveZ);

        //        starSystemView.OverlayDisplay.Children.Clear();

        //        starSystemView.StarDisplay.Children.Clear();

        //        starSystemView.BuildOverlay();

        //        starSystemView.BuildPlane();

        //        starSystemView.RenderStars();

        //        starSystemView.UpdateRender();

        //        selectedSystem = starSystemView.currentSystem;
        //    }

        //    mouseReferencePoint.X = movePoint.X;
        //    mouseReferencePoint.Y = movePoint.Y;
        //}

        //private void GalaxyMap_MouseWheel(object sender, MouseWheelEventArgs e)
        //{
        //    double delta = (e.Delta/(120*0.5));

        //    starSystemView.zoom -= delta;

        //    if (starSystemView.zoom > 80)
        //    {
        //        starSystemView.zoom = 80;
        //    }

        //    if (starSystemView.zoom < 4)
        //    {
        //        starSystemView.zoom = 4;
        //    }

        //    starSystemView.starSystemList =
        //        starSystemView.starSystemSet.UpdateStarSystemCollection(starSystemView.currentSystem.ID,
        //            starSystemView.zoom);

        //    starSystemView.starSystemCollection =
        //        starSystemView.starSystemSet.BuildStarCollection(starSystemView.starSystemList);

        //    starSystemView.starSystemList.OrderBy(p => p.Location.Z);

        //    starSystemView.OverlayDisplay.Children.Clear();

        //    starSystemView.StarDisplay.Children.Clear();

        //    starSystemView.BuildOverlay();

        //    starSystemView.BuildPlane();

        //    starSystemView.RenderStars();

        //    starSystemView.UpdateRender();

        //    selectedSystem = starSystemView.currentSystem;
        //}
    }
}