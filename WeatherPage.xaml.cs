using MauiWeather.Services;
using System.Diagnostics;

namespace MauiWeather;

public partial class WeatherPage : ContentPage
{
	public WeatherPage()
	{
		InitializeComponent();
		OnAppearing();
	}

	protected async override void OnAppearing()
	{
		base.OnAppearing();
		var response = await APIService.GetWeather("greenville");
		CityLabel.Text = response.name;
		WeatherLabel.Text = response.weather[0].main;
		TemperatureLabel.Text = response.main.temperature.ToString() + " F";
		WindLabel.Text = response.wind.roundedSpeed.ToString() + " mph";
		HumidityLabel.Text = response.main.humidity.ToString() + " %";
		PressureLabel.Text = response.main.pressure.ToString() + " hPa";
		Debug.WriteLine(response);
	}
}

//Greenville 35.6127, -77.3663