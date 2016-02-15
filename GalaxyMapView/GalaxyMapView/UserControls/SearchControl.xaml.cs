using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GalaxyMapView.DataSource;
using GalaxyMapView.StarSystems;

namespace GalaxyMapView.UserControls
{
     
    public partial class SearchControl : UserControl
    {

        public static Grid ControlGrid = new Grid();

        //private int row = 0;
        //private int col = 0;
        //private int sel = 0;

        //private int globalMode = 0;

        //private string[] globalOptions = new string[3];

        //private string[] menuItems = new string[6];

        //private bool showSystem = true;

   //     private StarSystem select = GalaxyMap.selectedSystem;

        public SearchControl()
        {

            InitializeComponent();

            MainGrid.Children.Add(ControlGrid);

        }

        //    menuItems[0] = "Global";
        //    menuItems[1] = "Search"; 
        //    menuItems[2] = "Filter"; 
        //    menuItems[3] = "Show";
        //    menuItems[4] = "Details";
        //    menuItems[5] = "Selection";

        //    globalOptions[0] = "Global";
        //    globalOptions[1] = "Local";
        //    globalOptions[2] = "Personal";

        //    SetUpGrid();
            
        //}

        //public void SetUpGrid()
        //{

            

        //    ModeSelector.Children.Clear();
        //    SelectionGrid.Children.Clear();

        //    for (int sel = 0; sel < 6; sel += 1)
        //    {
        //        Label label = new Label();

        //        label.Name = "option_" + sel;

        //        label.FontFamily = new FontFamily(new Uri("pack://application:,,,/Fonts/"), "./#Euro Caps");
        //        label.FontSize = 10;
        //        label.Content = menuItems[sel];
        //        label.HorizontalContentAlignment = HorizontalAlignment.Center;
        //        label.VerticalContentAlignment = VerticalAlignment.Center;

        //        label.Foreground = new SolidColorBrush(Color.FromArgb(0xF0, 0x3D, 0x95, 0xDE));
        //        label.BorderBrush = new SolidColorBrush(Color.FromArgb(0xF0, 0x3D, 0x95, 0xDE));

        //        if (menuItems[sel].Length>0) label.BorderThickness = new Thickness(1, 1, 1, 1);

        //        label.MouseDown += OptionMouseDownHandler;

        //        ModeSelector.Children.Add(label);

        //        Grid.SetRow(label, 0);
        //        Grid.SetColumn(label, sel);
        //    }

        //    if (!showSystem)
        //    {
        //        for (int row = 0; row < 4; row += 1)
        //        {

        //            for (int col = 0; col < 6; col += 1)
        //            {
        //                Label label = new Label();

        //                label.Name = "selection_" + row + "_" + col;

        //                label.FontFamily = new FontFamily(new Uri("pack://application:,,,/Fonts/"), "./#Euro Caps");
        //                label.FontSize = 10;
        //                label.Content = "Col 285 Sector\n IW-B C1-3 ";
        //                label.HorizontalContentAlignment = HorizontalAlignment.Left;
        //                label.VerticalContentAlignment = VerticalAlignment.Center;

        //                label.Foreground = new SolidColorBrush(Color.FromArgb(0xF0, 0x3D, 0x95, 0xDE));
        //                label.BorderBrush = new SolidColorBrush(Color.FromArgb(0xF0, 0x3D, 0x95, 0xDE));
        //                label.BorderThickness = new Thickness(1, 1, 1, 1);

        //                label.MouseLeftButtonDown += SelectionMouseDownHandler;

        //                SelectionGrid.Children.Add(label);

        //                Grid.SetRow(label, row);
        //                Grid.SetColumn(label, col);
        //            }
        //        }
        //    }

        //    if (showSystem)
        //    {

        //       // select = GalaxyMap.selectedSystem;

        //        TextBox detail = new TextBox();
        //        detail.FontFamily = new FontFamily(new Uri("pack://application:,,,/Fonts/"), "./#Euro Caps");
        //        detail.FontSize = 14;
                
        //        detail.HorizontalContentAlignment = HorizontalAlignment.Left;
        //        detail.VerticalContentAlignment = VerticalAlignment.Top;

        //        detail.Foreground = new SolidColorBrush(Color.FromArgb(0xF0, 0x3D, 0x95, 0xDE));
        //        detail.Background = null;
        //        detail.BorderBrush = new SolidColorBrush(Color.FromArgb(0xF0, 0x3D, 0x95, 0xDE));
        //        detail.BorderThickness = new Thickness(1, 1, 1, 1);

        //        Grid.SetColumnSpan(detail,3);
        //        Grid.SetRowSpan(detail,4);

        //        SelectionGrid.Children.Add(detail);

        //        /*
        //        _milkyWay[counter].ID = systemData.id;
        //        _milkyWay[counter].Name = systemData.name;
        //        _milkyWay[counter].Location = new Point3D((double) systemData.x, (double) systemData.y,(double) systemData.z);
        //        _milkyWay[counter].Faction = systemData.faction;
        //        _milkyWay[counter].Population = systemData.population;
        //        _milkyWay[counter].Government = systemData.government;
        //        _milkyWay[counter].Allegiance = systemData.allegiance;
        //        _milkyWay[counter].State = systemData.state;
        //        _milkyWay[counter].Security = systemData.security;
        //        _milkyWay[counter].Economy = systemData.primary_economy;
        //        _milkyWay[counter].Power = systemData.power;
        //        _milkyWay[counter].PowerState = systemData.power_state;
        //        _milkyWay[counter].Updated = systemData.updated_at;
        //        */


        //        //detail.Text = select.Name + Environment.NewLine;
        //        //detail.Text += select.Allegiance+Environment.NewLine;
        //        //detail.Text += select.Population + Environment.NewLine;
        //        //detail.Text += select.Faction + Environment.NewLine;
        //        //detail.Text += select.Economy + Environment.NewLine;
        //        //detail.Text += select.Government += Environment.NewLine;

                
        //    }
        //}

        //private void SelectionMouseDownHandler(object sender, MouseButtonEventArgs e)
        //{
        //    Label selection = sender as Label;

        //    string[] fetch = selection.Name.Split('_');

        //    row =Convert.ToInt32(fetch[1]);
        //    col = Convert.ToInt32(fetch[2]);

        //    inputBox.Text = "selction row:"+row+" selected column:"+col;

        //    UpdateGrid(sel,row,col);
        //}

        //private void OptionMouseDownHandler(object sender, MouseButtonEventArgs e)
        //{

        //    Label option = sender as Label;
        //    string[] fetch = option.Name.Split('_');

        //    sel = Convert.ToInt32(fetch[1]);

        //    if (sel == 0)
        //    {
        //        globalMode += 1;
        //        if (globalMode > 2) globalMode = 0;
        //    }


        //    inputBox.Text = "selection: " + sel;

        //    UpdateGrid(sel, row, col);
        //}

        //private void UpdateGrid(int sel,int row, int col)
        //{
        //    // set global - local - personal

        //        menuItems[0] = globalOptions[globalMode];

        //        SetUpGrid();
        
        //}
    }
}
