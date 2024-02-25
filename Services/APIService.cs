using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Diagnostics;
using MauiWeather.Models;
using Newtonsoft.Json;

namespace MauiWeather.Services
{
    public static class APIService
    {
        //Get current weather by city
        public static async Task<Root> GetWeatherByCity(string city)
        {
            
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://open-weather13.p.rapidapi.com/city/{city}"),
                Headers =
                    {
                        { "X-RapidAPI-Key", "d16089aecbmshc1fc9e2ac0fc8b8p1f8009jsn0432cdff9b0b" },  //d825f6ccc4msh5c52f4fbeced2a9p13985fjsne7ef710dac4e
                        { "X-RapidAPI-Host", "open-weather13.p.rapidapi.com" },
                    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Root>(body);
            }
        }

        //Get current weather by lat/long
        public static async Task<Root> GetWeatherByLatLong(double latitude, double longitude)
        {
            //Convert to strings
            string lat = latitude.ToString();
            string lon = longitude.ToString();
            Root Data;

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://open-weather13.p.rapidapi.com/city/latlon/{lat}/{lon}"),
                Headers =
                    {
                        { "X-RapidAPI-Key", "d16089aecbmshc1fc9e2ac0fc8b8p1f8009jsn0432cdff9b0b" },
                        { "X-RapidAPI-Host", "open-weather13.p.rapidapi.com" },
                    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                //need to convert temperatures to F
                Data = JsonConvert.DeserializeObject<Root>(body);
                Data.main.temp = Data.main.temp * 9 / 5 - 459.67;
                return Data;
            }
        

        }

        //Get 5 day forecast
        public static async Task<FRoot> GetWeatherForecast(double latitude, double longitude)
        {
            string lat = latitude.ToString();
            string lon = longitude.ToString();
            FRoot Data;
            
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://open-weather13.p.rapidapi.com/city/fivedaysforcast/{lat}/{lon}"),
                Headers =
                {
                        { "X-RapidAPI-Key", "d16089aecbmshc1fc9e2ac0fc8b8p1f8009jsn0432cdff9b0b" },
                        { "X-RapidAPI-Host", "open-weather13.p.rapidapi.com" },
                    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Data = JsonConvert.DeserializeObject<FRoot>(body);
                return Data;
            }
        }
    }
}
