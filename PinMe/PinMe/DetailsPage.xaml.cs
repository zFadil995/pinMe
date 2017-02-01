using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace PinMe
{
    public partial class DetailsPage : ContentPage
    {
        private List<string> diners;
        public DetailsPage()
        {
            InitializeComponent();
            Init();
            dinersList.ItemSelected += (object sender, SelectedItemChangedEventArgs e) =>
            {
                if (dinersList.SelectedItem != null)
                {
                    foreach (Diner diner in Location.diners)
                    {
                        if(diner.name.Substring(0, 5) == ((string)dinersList.SelectedItem).Substring(0, 5))
                            Location.MoveToPosition = new Position(diner.gpsy, diner.gpsx);

                    }
                    ((TabbedPage) this.Parent).SelectedItem = ((TabbedPage) this.Parent).Children.First();
                    if (Device.OS == TargetPlatform.Windows)
                        ((MapPage) ((TabbedPage) this.Parent).Children.First()).TabSelected();
                    dinersList.SelectedItem = null;
                    ((TabbedPage)this.Parent).SelectedItem = null;
                }
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Init();
        }

        private void Init()
        {
            diners = new List<string>();
            if (!Location.CurrentPosition.Latitude.Equals(0) && !Location.CurrentPosition.Longitude.Equals(0))
                foreach (Diner diner in Location.diners)
                {
                    diners.Add(diner.name + " (" + Location.BetweenString(diner.gpsy, diner.gpsx, Location.CurrentPosition.Latitude, Location.CurrentPosition.Longitude)+")");
                }
            else
                foreach (Diner diner in Location.diners)
                {
                    diners.Add(diner.name);
                }

            dinersList.ItemsSource = diners;
        }
    }
}
