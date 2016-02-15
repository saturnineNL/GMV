using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media.Media3D;
using GalaxyMapView.DataSource;
using GalaxyMapView.UserControls;

namespace GalaxyMapView.StarSystems
{
    public class StarSystemSet
    {
        private List<StarSystem> milkyWay;
        private List<Station> stationList;

        List<Star> starCollection = new List<Star>();

        private Point3D centerPoint = new Point3D();

        public static BuildDataSet galaxy = new BuildDataSet();
        
        public StarSystemSet()
        {
            milkyWay = galaxy.milkyWay;
            stationList = galaxy.stationList;
        }

        public StarSystem GetStarSystemName(string name)
        {
            int index = milkyWay.FindIndex(fetchname => fetchname.Name == name);

            return milkyWay[index];
        }

        public StarSystem GetStarSystemID(int systemID)
        {
            int index = milkyWay.FindIndex(fetchSystem => fetchSystem.ID == systemID);

            return milkyWay[index];
        }

        public Point3D GetStarSystemLocation(StarSystem currentSystem)
        {
            return currentSystem.Location;
        }

        public List<StarSystem> BuildStarSystemCollection(Point3D midPoint, double range)
        {
            List<StarSystem> returnList = new List<StarSystem>();

            double minX = midPoint.X - range;
            double minY = midPoint.Y - range;
            double minZ = midPoint.Z - range;
            double maxX = midPoint.X + range;
            double maxY = midPoint.Y + range;
            double maxZ = midPoint.Z + range;

            foreach (var systemlist in milkyWay
                .Where(system => system.Location.X >= minX && system.Location.X <= maxX &&
                                 system.Location.Y >= minY && system.Location.Y <= maxY &&
                                 system.Location.Z >= minZ && system.Location.Z <= maxZ))
            {
                if (calculateDistance(midPoint, new Point3D(systemlist.Location.X, systemlist.Location.Y, systemlist.Location.Z)) <= range)
                {
                    returnList.Add(systemlist);
                }
            }

            centerPoint = midPoint;

            return returnList;
        }

        public List<StarSystem> UpdateStarSystemCollection(int systemID, double range)
        {
            int index = milkyWay.FindIndex(fetchSystem => fetchSystem.ID == systemID);
            
            return BuildStarSystemCollection(milkyWay[index].Location,range);

        }

        public List<StarSystem> UpdateStarSystemCollection(Point3D location, double range)
        {
            List <StarSystem> returnList = BuildStarSystemCollection(location, range);

            return returnList;

        }

        public void UpdateStationSystemCollection(List<StarSystem> starSystemCollection)
        {

            foreach (var systems in starSystemCollection)
            {
                var fetchStations = stationList.FindAll(stationLocation => stationLocation.SystemID == systems.ID);

                if (fetchStations.Count != systems.StationList.Count)
                {
                    List<Station> currentStationList = systems.StationList;

                    foreach (var foundStations in fetchStations)
                    {
                        if (currentStationList.FindIndex(stationExcists => stationExcists.ID == foundStations.ID) == -1)
                        {
                            currentStationList.Add(foundStations);

                        }

                    }

                    systems.StationList = currentStationList;
                }
            }


        }

        public List<Star> BuildStarCollection(List<StarSystem> systemCollection)
        {
            starCollection = new List<Star>();

            double density = 1000;

            double densityStep = 0;

            densityStep = Math.Ceiling((double)(systemCollection.Count / density));

            int addCount = 0;
            int skipCount = 0;

            if (systemCollection.Count > density) skipCount = (int) density - systemCollection.Count;

            foreach (var starSystem in systemCollection)
            {
                Star star = new Star();

                star.starID = starSystem.ID;
                star.name = starSystem.Name;

                star.originPoint = new Point3D(starSystem.Location.X, starSystem.Location.Y, starSystem.Location.Z);

                star.currentPoint = new Point3D(centerPoint.X - starSystem.Location.X, centerPoint.Y - starSystem.Location.Y, centerPoint.Z - starSystem.Location.Z);

                star.distance = calculateDistance(centerPoint, star.originPoint);

                if (ViewController.selectedStars.Contains(starSystem.ID)) star.selectSystem = true;
                if (ViewController.targetedStars.Contains(starSystem.ID)) star.targetSystem = true;

                if (addCount % densityStep == 0 || star.distance == 0 || skipCount>0 || star.selectSystem || star.targetSystem)
                {
                    starCollection.Add(star);

                }

                else skipCount += 1;

                addCount += 1;
            }

        
            return starCollection;
        }
         

        private double calculateDistance(Point3D startPoint, Point3D endPoint)
        {

            double distanceX = 99999999;
            double distanceY = 99999999;
            double distanceZ = 99999999;

            if (startPoint.X <= 0 && endPoint.X < 0) distanceX = Math.Min(startPoint.X, endPoint.X) + Math.Abs(Math.Max(startPoint.X, endPoint.X));
            if (startPoint.Y <= 0 && endPoint.Y < 0) distanceY = Math.Min(startPoint.Y, endPoint.Y) + Math.Abs(Math.Max(startPoint.Y, endPoint.Y));
            if (startPoint.Z <= 0 && endPoint.Z < 0) distanceZ = Math.Min(startPoint.Z, endPoint.Z) + Math.Abs(Math.Max(startPoint.Z, endPoint.Z));

            if ((startPoint.X <= 0 && endPoint.X > 0) || (startPoint.X > 0 && endPoint.X < 0)) distanceX = Math.Abs(Math.Min(startPoint.X, endPoint.X)) + Math.Max(startPoint.X, endPoint.X);
            if ((startPoint.Y <= 0 && endPoint.Y > 0) || (startPoint.Y > 0 && endPoint.Y < 0)) distanceY = Math.Abs(Math.Min(startPoint.Y, endPoint.Y)) + Math.Max(startPoint.Y, endPoint.Y);
            if ((startPoint.Z <= 0 && endPoint.Z > 0) || (startPoint.Z > 0 && endPoint.Z < 0)) distanceZ = Math.Abs(Math.Min(startPoint.Z, endPoint.Z)) + Math.Max(startPoint.Z, endPoint.Z);

            if (startPoint.X >= 0 && endPoint.X > 0) distanceX = Math.Max(startPoint.X, endPoint.X) - Math.Min(startPoint.X, endPoint.X);
            if (startPoint.Y >= 0 && endPoint.Y > 0) distanceY = Math.Max(startPoint.Y, endPoint.Y) - Math.Min(startPoint.Y, endPoint.Y);
            if (startPoint.Z >= 0 && endPoint.Z > 0) distanceZ = Math.Max(startPoint.Z, endPoint.Z) - Math.Min(startPoint.Z, endPoint.Z);

            if (startPoint.X == endPoint.X) distanceX = 0;
            if (startPoint.Y == endPoint.Y) distanceY = 0;

            double distance = Math.Sqrt(Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2) + Math.Pow(distanceZ, 2));

            return distance;

        }

    }
}
