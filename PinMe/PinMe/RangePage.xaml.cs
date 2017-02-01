using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PinMe
{
    public partial class RangePage : ContentPage
    {
        public RangePage(bool currentLocation, double range)
        {
            InitializeComponent();
            rangeSlider.Value = (range - 1.9)/(Location.distance-2);
            if (Device.OS == TargetPlatform.iOS)
            {
                rangeButton.BackgroundColor = Color.White;
                rangeButton.WidthRequest = 50;

            }
            //rangeName.Text = currentLocation ? "5 KM" : "ALL";
        }

        private void RangeChanged(object sender, ValueChangedEventArgs e)
        {
            rangeName.Text = ((Slider)sender).Value.Equals(1)?"ALL":(int) (2 + ((Slider) sender).Value * (Location.distance - 2)) + " KM";
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private void RangeButtonClicked(object sender, EventArgs e)
        {
            Location.range = (int) (2 + rangeSlider.Value*(Location.distance - 2));
            Navigation.PopModalAsync();
        }
    }
}
