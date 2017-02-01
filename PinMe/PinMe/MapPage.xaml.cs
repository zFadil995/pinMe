using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Position = Xamarin.Forms.Maps.Position;

namespace PinMe
{
    public partial class MapPage : ContentPage
    {
        public Map map;
        private Position currentPosition = new Position(0, 0);
        public List<Pin> allPins;
        private bool _currentLocation = false;
        public MapPage()
        {
            Init();
            map = new Map()
            {
                MapType = MapType.Street,
                HasZoomEnabled = true,
                IsShowingUser = true,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            InitGeolocator();
            InitializeComponent();
            SetPins(allPins, map);
            if (Device.OS == TargetPlatform.Android)
            {
                mapLayout.BackgroundColor = Color.FromHex("#0077FF");
            }
            else
            {
                RangeButton.BackgroundColor = Color.FromHex("#DDDDDD");
            }
            mapLayout.Children.Add(map);
        }

        public void Init()
        {
            Pin pin; allPins = new List<Pin>();
            foreach (Diner diner in Location.diners)
            {
                pin = new Pin { Type = PinType.Place, Position = new Position(diner.gpsy, diner.gpsx), Label = diner.name, Address = diner.address };
                pin.Clicked += (sender, args) => { DisplayAlert("Tapped!", "Pin was tapped.", "OK"); };
                allPins.Add(pin);
            }
        }
         private void SetPins(List<Pin> pins, Map map)
        {
            foreach (Pin pin in pins)
                map.Pins.Add(pin);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Location.MoveToPosition == new Position(0, 0))
                if (currentPosition != new Position(0, 0))
                    map.MoveToRegion(
                        MapSpan.FromCenterAndRadius(new Position(currentPosition.Latitude, currentPosition.Longitude),
                            Distance.FromKilometers(Location.range)));
                else
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(Location.latitude, Location.longitude),
                        Distance.FromKilometers(Location.range)));
            else
            {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(Location.MoveToPosition,
                    Distance.FromMeters(250)));
                Location.MoveToPosition = new Position(0, 0);
            }
        }



        public void TabSelected() { OnAppearing(); }

        private void RangeButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new RangePage(_currentLocation, map.VisibleRegion.Radius.Kilometers));
        }
        public async void InitGeolocator()
        {
            try
            {
                IGeolocator locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                Plugin.Geolocator.Abstractions.Position position = await locator.GetPositionAsync(20000);
                currentPosition = new Position(position.Latitude, position.Longitude);
                Location.CurrentPosition = currentPosition;
                map.MoveToRegion(MapSpan.FromCenterAndRadius(currentPosition, Distance.FromKilometers(5)));
                Location.InitDistance(position.Latitude, position.Longitude);
                _currentLocation = true;
            }
            catch (GeolocationException e)
            {
                await DisplayAlert("No Location!", "Geolocation Failed.", "OK");
            }
            catch (Exception e)
            {
                await DisplayAlert("Oh no..!", "God help you!", "OK");
            }

        }
       
    }
}
