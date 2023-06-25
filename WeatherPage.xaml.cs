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
		var result = await APIService.GetWeatherByLatLong(47.6829, -122.1209);
		CityLabel.Text = result.name;
		WeatherLabel.Text = result.weather[0].main;
		Debug.WriteLine(result);
	}
}