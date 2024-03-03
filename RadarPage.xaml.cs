using System.Diagnostics;

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
            Location currentLocation = await GetCurrentLocation();
            double lat = currentLocation.Latitude;
            double lon = currentLocation.Longitude;
            RadarWebView.Source = $"https://embed.windy.com/embed2.html?lat={lat}&lon={lon}&detailLat={lat}&detailLon={lon}&width=500&height=850&zoom=5&level=surface&overlay=wind&product=ecmwf&menu=&message=true&marker=true&calendar=12&pressure=&type=map&location=coordinates&detail=true&metricWind=mph&metricTemp=%C2%B0F&radarRange=-1";
            IsBusy(false);
        }


        //Getting device location
        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;

        public async Task<Location> GetCurrentLocation()
        {
            try
            {
                _isCheckingLocation = true;

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();

                Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                if (location != null)
                    Debug.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");

                //CheckMock();


                return location;
            }
            // Catch one of the following exceptions:
            //   FeatureNotSupportedException
            //   FeatureNotEnabledException
            //   PermissionException
            catch (Exception ex)
            {
                // Unable to get location
                Debug.WriteLine($"Unable to get location: {ex.Message}");
                return null;
            }
            finally
            {
                _isCheckingLocation = false;
            }
        }

        public void CancelRequest()
        {
            if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
                _cancelTokenSource.Cancel();
        }
        //Check if location is from mock provider
        public async Task CheckMock()
        {
            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium);
            Location location = await Geolocation.Default.GetLocationAsync(request);

            if (location != null && location.IsFromMockProvider)
            {
                Debug.WriteLine("This location is from a mock provider.");
            }
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
