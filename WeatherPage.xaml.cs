using MauiWeather.Services;
using System.Diagnostics;

namespace MauiWeather;

public partial class WeatherPage : ContentPage
{
	public List<Models.List> WeatherList;
	public WeatherPage()
	{
		InitializeComponent();
		OnAppearing();
	}

	protected async override void OnAppearing()
	{
		base.OnAppearing();
		
		//API call
		//var response = await APIService.GetWeather("greenville");
		var current = await APIService.GetWeatherByLatLong(35.6127, -77.3663);
		
		//Getting forecast data for collection view
		//var forecast = await APIService.GetWeatherForecast(35.6127, -77.3663);


		//Assigning main values
		CityLabel.Text = current.name;
		WeatherLabel.Text = current.weather[0].main;
		WeatherImage.Source = current.weather[0].fullIconUrl;
		TemperatureLabel.Text = current.main.temperature.ToString() + " F";
		WindLabel.Text = current.wind.roundedSpeed.ToString() + " mph";
		HumidityLabel.Text = current.main.humidity.ToString() + " %";
		PressureLabel.Text = current.main.pressure.ToString() + " hPa";
		Debug.WriteLine(current);
	}
}

//Greenville 35.6127, -77.3663