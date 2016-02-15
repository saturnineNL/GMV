using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Newtonsoft.Json;

namespace GalaxyMapView.DataSource
{
    public class BuildDataSet
    {

        private String systemDataSet;
        private String stationDataSet;
        private String commodityDefSet;
        private String commoditySet;

        private List<StarSystem> _milkyWay = new List<StarSystem>();
        private List<Station> _stationList = new List<Station>();

        public BuildDataSet()
        {

            GetData();

            BuildSystemData();
            BuildStationData();
            BuildCommodityDefinitionData();

        }

        private void GetData()
        {
            systemDataSet = ReadData.ReadSystem();
            stationDataSet = ReadData.ReadStation();
            commodityDefSet = ReadData.ReadCommoditiesDefinition();
 
        }

        private void BuildSystemData()
        {
            dynamic readSystem = JsonConvert.DeserializeObject(systemDataSet);

            int counter = 0;

            foreach (var systemData in readSystem)
            {
                _milkyWay.Add(new StarSystem());

                _milkyWay[counter].ID = systemData.id;
                _milkyWay[counter].Name = systemData.name;
                _milkyWay[counter].Location = new Point3D((double) systemData.x, (double) systemData.y,(double) systemData.z);
                _milkyWay[counter].Faction = systemData.faction;
                _milkyWay[counter].Population = systemData.population;
                _milkyWay[counter].Government = systemData.government;
                _milkyWay[counter].Allegiance = systemData.allegiance;
                _milkyWay[counter].State = systemData.state;
                _milkyWay[counter].Security = systemData.security;
                _milkyWay[counter].Economy = systemData.primary_economy;
                _milkyWay[counter].Power = systemData.power;
                _milkyWay[counter].PowerState = systemData.power_state;
                _milkyWay[counter].Updated = systemData.updated_at;

                _milkyWay[counter].Permit = false;
                if (systemData.needs_permit == 1) _milkyWay[counter].Permit = true;

                _milkyWay[counter].StationList = new List<Station>();

                counter += 1;
            }

            readSystem = null;
        }

        private void BuildStationData()
        {

            dynamic readStation = JsonConvert.DeserializeObject(stationDataSet);

            var counter = 0;

            foreach (var stationData in readStation)
            {

                _stationList.Add(new Station());

                _stationList[counter].ID = stationData.id;
                _stationList[counter].Name = stationData.name;
                _stationList[counter].SystemID = stationData.system_id;
                _stationList[counter].LandingPad = stationData.max_landing_pad_size;
                _stationList[counter].Distance = stationData.distance_to_star;
                _stationList[counter].Faction = stationData.faction;
                _stationList[counter].Government = stationData.government;
                _stationList[counter].Allegiance = stationData.allegiance;
                _stationList[counter].State = stationData.state;
                _stationList[counter].TypeID = stationData.type_id;
                _stationList[counter].Type = stationData.type;
                _stationList[counter].StationUpdate = stationData.updated_at;
                _stationList[counter].MarketUpdate = stationData.market_updated_at;

                _stationList[counter].BlackMarket = false;
                _stationList[counter].Refuel = false;
                _stationList[counter].Repair = false;
                _stationList[counter].Outfitting = false;
                _stationList[counter].Shipyard = false;
                _stationList[counter].Commodities = false;
                _stationList[counter].Planetary = false;

                _stationList[counter].Import = new List<string>();
                _stationList[counter].Export = new List<string>();
                _stationList[counter].Prohibited = new List<string>();
                _stationList[counter].Economies = new List<string>();

                _stationList[counter].CommodityList = new List<Commodities>();

                counter += 1;
            }

            readStation = null;
        }

        private void BuildCommodityDefinitionData()
        {

            dynamic readCommodityDefSet = JsonConvert.DeserializeObject(commodityDefSet);

            List<CommodityDef> comDefList = new List<CommodityDef>();

            int counter = 0;

            foreach (var commodityDefData in readCommodityDefSet)
            {
                comDefList.Add(new CommodityDef());

                comDefList[counter].ID = commodityDefData.id;
                comDefList[counter].Name = commodityDefData.name;
                comDefList[counter].AveragePrice = commodityDefData.average_price;

                comDefList[counter].CategoryID = commodityDefData.category_id;
                comDefList[counter].Category = new List<string>();

            }
        }



        // List<string> csvList=new List<string>();

        // csvList = ReadData.ReadCommodities();

        public List<StarSystem> milkyWay
        {
            get {return _milkyWay; }            
        }

        public List<Station> stationList
        {
            get { return _stationList;}
        }  
    }


    public class StarSystem
    {
     
       public int ID { get; set; }
       public string Name { get; set; }
       public Point3D Location { get; set; }
       public string Faction { get; set; }
       public string Population { get; set; }
       public string Government { get; set; }
       public string Allegiance { get; set; }
       public string State { get; set; }
       public string Security { get; set; }
       public string Economy { get; set; }
       public string Power { get; set; }
       public string PowerState { get; set; }
       public bool Permit { get; set; }
       public int Updated { get; set; }

       public List<Station> StationList { get; set; } 

    }

    public class Station
    {   

        public int ID { get; set; }
        public string Name { get; set; }
        public int SystemID { get; set; }
        public string LandingPad { get; set; }
        public string Distance { get; set; }
        public string Faction { get; set; }
        public string Government { get; set; }
        public string Allegiance { get; set; }
        public string State { get; set; }
        public int TypeID { get; set; }
        public string Type { get; set; }
        public bool BlackMarket { get; set; }
        public bool Refuel { get; set; }
        public bool Repair { get; set; }
        public bool Outfitting { get; set; }
        public bool Shipyard { get; set; }
        public bool Commodities { get; set; }
        public bool Planetary { get; set; }

        public List<string> Import { get; set; }
        public List<string> Export { get; set; }
        public List<string> Prohibited { get; set; }
        public List<string> Economies { get; set; }

        public string StationUpdate { get; set; }
        public int ShipyardUpdate { get; set; }
        public int OutfittingUpdate { get; set; }
        public string MarketUpdate { get; set; }

        public List<Commodities> CommodityList { get; set; }

    }

    public class Commodities
    {
     
        public int ID { get; set; }
        public int StationID { get; set; }
        public int CommodityID { get; set; }
        public int Supply { get; set; }
        public int BuyPrice { get; set; }
        public int SellPrice { get; set; }
        public int Demand { get; set; }
        public int Collected { get; set; }
        public int updateCount { get; set; }

    }

    class CommodityDef
    {
  
        public int ID { get; set; }
        public string Name { get; set; }
        public int CategoryID { get; set; }
        public int AveragePrice { get; set; }
        public List<string> Category { get; set; } 
    }

}
