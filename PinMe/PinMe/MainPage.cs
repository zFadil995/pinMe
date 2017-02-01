using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PinMe
{
    class MainPage : TabbedPage
    {
        public MainPage()
        {
            Page mapPage;
            Page detailPage;
            if (Device.OS == TargetPlatform.iOS)
            {
                mapPage = new MapPage() { Title = "Map", Icon = "map.png" };
                detailPage = new DetailsPage() { Title = "Details", Icon = "details.png" };
            }
            else
            {
                mapPage = new MapPage() { Title = "Map" };
                detailPage = new DetailsPage() { Title = "Details" };
            }
            Children.Add(mapPage);
            Children.Add(detailPage);

            CurrentPageChanged += (object sender, EventArgs e) =>
            {
                //if(Device.OS == TargetPlatform.Windows)
                    //if(CurrentPage == mapPage)
                        
            };
        }
        
    }
}
