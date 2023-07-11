

namespace MauiWeather
{
    public partial class RadarPage : ContentPage
    {
        public RadarPage()
        {
            InitializeComponent();
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
