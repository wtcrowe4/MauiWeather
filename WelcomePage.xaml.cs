namespace MauiWeather
{
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        public void WelcomeButton_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new WeatherPage();
        }
    }
}
