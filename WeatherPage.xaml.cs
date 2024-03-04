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
        ClearData();
        
        //Getting current location
		Location currentLocation = await WeatherCurrentLocation();
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
        IsBusy(false);
	}


    //Get location from services
    public CancellationTokenSource _cancelTokenSource;
    public bool _isCheckingLocation;
    public async Task<Location> WeatherCurrentLocation()
    {
        LocationService locationService = new LocationService();
        return await locationService.GetCurrentLocation();
    }
	
   
    //My Weather button to reload data
    private void MyLocation_Tapped(object sender, EventArgs e)
    {
        NearForecastList.Clear();
        OnAppearing();
    }

    //Search button to search for a city
    private async void SearchBtn_Clicked(object sender, EventArgs e)
    {
        var search = await DisplayPromptAsync(title: "Search by City", message: "Currently only using city name", placeholder: "Search", accept: "Search", cancel: "Cancel");
        if (search != null) 
        {
            ClearData();
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
            IsBusy(false);
        }
    }

    //Radar button to open radar page
    private async void RadarBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RadarPage());
        Application.Current.MainPage = new RadarPage();
    }   

    //Activity indicator
    public new void IsBusy(bool busy)
    {
        if (busy)
        {
            ActivityIndicator.IsVisible = true;
            ActivityIndicator.IsRunning = true;
            WindImage.IsVisible = false;
            HumidityImage.IsVisible = false;
            PressureImage.IsVisible = false;
        }
        else
        {
            ActivityIndicator.IsVisible = false;
            ActivityIndicator.IsRunning = false;
            WindImage.IsVisible = true;
            HumidityImage.IsVisible = true;
            PressureImage.IsVisible = true;
        }
    }

    public void ClearData() 
    { 
        NearForecastList.Clear();
        ForecastCollectionView.ItemsSource = null;
        CityLabel.Text = "";
        WeatherLabel.Text = "";
        WeatherImage.Source = "";
        TemperatureLabel.Text = "";
        WindLabel.Text = "";
        HumidityLabel.Text = "";
        PressureLabel.Text = "";
        IsBusy(true);
    }
}

