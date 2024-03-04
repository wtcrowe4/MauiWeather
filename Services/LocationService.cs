using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MauiWeather.Services
{
    class LocationService
    {

        //Getting device location
        public CancellationTokenSource _cancelTokenSource;
        public bool _isCheckingLocation;

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

    }
}
