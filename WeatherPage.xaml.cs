using MauiWeather.Services;
using System.Diagnostics;
using MauiWeather.Models;

namespace MauiWeather;

public partial class WeatherPage : ContentPage
{
	public List<Models.List> NearForecastList;
	public WeatherPage()
	{
		InitializeComponent();
        NearForecastList = new List<Models.List>();
		OnAppearing();
	}

	protected async override void OnAppearing()
	{
		base.OnAppearing();
        
        //Getting current location
		Location currentLocation = await GetCurrentLocation();
		double lat = currentLocation.Latitude;
		double lon = currentLocation.Longitude;

        //API calls
        //Greenville 35.6127, -77.3663
        //var response = await APIService.GetWeatherByCity("greenville");
        var current = await APIService.GetWeatherByLatLong(lat, lon);
		
		//Getting forecast data for collection view
		var forecast = await APIService.GetWeatherForecast(lat, lon);
        NearForecastList = forecast.list;
        
        //Convert to F
        for (var i = 0; i < NearForecastList.Count; i++)
        {
            NearForecastList[i].main.temperatureF = (int)Math.Round(NearForecastList[i].main.temp* 9 / 5 - 459.67);
        }
        
        //Assigning forecast values
        ForecastCollectionView.ItemsSource = NearForecastList;
		
        //Assigning main values
		CityLabel.Text = current.name;
		WeatherLabel.Text = current.weather[0].main;
		WeatherImage.Source = current.weather[0].fullIconUrl;
		TemperatureLabel.Text = current.main.temperature.ToString() + " °F";
		WindLabel.Text = current.wind.roundedSpeed.ToString() + " mph";
		HumidityLabel.Text = current.main.humidity.ToString() + " %";
		PressureLabel.Text = current.main.pressure.ToString() + " hPa";
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

    //My Weather button to reload data
    private void MyLocation_Tapped(object sender, EventArgs e)
    {
        OnAppearing();
    }

    //Search button to search for a city
    private async void SearchBtn_Clicked(object sender, EventArgs e)
    {
        var search = await DisplayPromptAsync(title: "Search by City", message: "Currently only using city name", placeholder: "Search", accept: "Search", cancel: "Cancel");
        if (search != null) 
        {
            NearForecastList.Clear();
            ForecastCollectionView.ItemsSource = null;
            Debug.WriteLine(search);
            var current = await APIService.GetWeatherByCity(search);
            var forecast = await APIService.GetWeatherForecast(current.coord.lat, current.coord.lon);
            NearForecastList = forecast.list;
            //Convert to F
            for (var i = 0; i < NearForecastList.Count; i++)
            {
                NearForecastList[i].main.temperatureF = (int)Math.Round(NearForecastList[i].main.temp * 9 / 5 - 459.67);
            }
            ForecastCollectionView.ItemsSource = NearForecastList;
            //Assigning main values
            CityLabel.Text = current.name;
            WeatherLabel.Text = current.weather[0].main;
            WeatherImage.Source = current.weather[0].fullIconUrl;
            TemperatureLabel.Text = current.main.temperature.ToString() + " °F";
            WindLabel.Text = current.wind.roundedSpeed.ToString() + " mph";
            HumidityLabel.Text = current.main.humidity.ToString() + " %";
            PressureLabel.Text = current.main.pressure.ToString() + " hPa";

        }
    }
}

