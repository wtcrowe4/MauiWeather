using System.Diagnostics;
using MauiWeather.Services;

namespace MauiWeather
{
    public partial class RadarPage : ContentPage
    {
        public RadarPage()
        {
            InitializeComponent();
           
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            IsBusy(true);
            //Getting current location
            Location currentLocation = await RadarCurrentLocation();
            double lat = currentLocation.Latitude;
            double lon = currentLocation.Longitude;
            RadarWebView.Source = $"https://embed.windy.com/embed2.html?lat={lat}&lon={lon}&detailLat={lat}&detailLon={lon}&width=500&height=850&zoom=7&level=surface&overlay=wind&product=ecmwf&menu=&message=true&marker=true&calendar=12&pressure=&type=map&location=coordinates&detail=true&metricWind=mph&metricTemp=%C2%B0F&radarRange=-1";
            IsBusy(false);
        }


        //Get location from services
        public CancellationTokenSource _cancelTokenSource;
        public bool _isCheckingLocation;
        public async Task<Location> RadarCurrentLocation()
        {
            LocationService locationService = new LocationService();
            return await locationService.GetCurrentLocation();
        }


        public void OnBackButtonClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new WeatherPage();
        }

        public new void IsBusy(bool busy)
        {
            if (busy)
            {
                ActivityIndicator.IsVisible = true;
                ActivityIndicator.IsRunning = true;
            }
            else
            {
                ActivityIndicator.IsVisible = false;
                ActivityIndicator.IsRunning = false;
            }
        }
    }
}
