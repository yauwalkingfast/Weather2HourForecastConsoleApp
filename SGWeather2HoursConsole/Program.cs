using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SGWeather2HoursConsole
{
    class Program
    {
        static HttpClient client = new HttpClient();
        
        static void Main(string[] args)
        {
            string input = GetValidInput();
            List<Forecast> forecastAllLocations = GetWeather();
            DisplayForecast(forecastAllLocations, input);
        }
        public static string GetValidInput()
        {
            string[] areas = new string[] { "Ang Mo Kio", "Bedok", "Bishan", "Boon Lay", "Bukit Batok", "Bukit Merah", "Bukit Panjang", "Bukit Timah", "Central Water Catchment", "Changi", "Choa Chu Kang", "Clementi", "City", "Geylang", "Hougang", "Jalan Bahar", "Jurong East", "Jurong Island", "Jurong West", "Kallang", "Lim Chu Kang", "Mandai", "Marine Parade", "Novena", "Pasir Ris", "Paya Lebar", "Pioneer", "Pulau Tekong", "Pulau Ubin", "Punggol", "Queenstown", "Seletar", "Sembawang", "Sengkang", "Sentosa", "Serangoon", "Southern Islands", "Sungei Kadut", "Tampines", "Tanglin", "Tengah", "Toa Payoh", "Tuas", "Western Islands", "Western Water Catchment", "Woodlands", "Yishun" };
            bool inputExist = false;
            string input;
            do
            {
                Console.WriteLine("Please enter area: ");
                input = Console.ReadLine();
                if (!Array.Exists(areas, x => x.ToLower() == input.ToLower()))
                {
                    Console.WriteLine("Area not found. Try again\n");
                }
                else
                {
                    inputExist = true;
                }

            } while (!inputExist);
            return input;
        }

        public static List<Forecast> GetWeather()
        {
            SetClient();
            string queryToAppend = BuildQuery();
            return GetFromApi(queryToAppend);
        }
        public static void SetClient()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static string BuildQuery()
        {
            string queryDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            return "date_time" + queryDateTime;
        }

        public static List<Forecast> GetFromApi(string query)
        {
            List <Forecast> forecast = new List<Forecast>();
            UriBuilder baseUri = new UriBuilder("https://api.data.gov.sg/v1/environment/2-hour-weather-forecast");
            baseUri.Query = query; // no need ? for .Net5. auto insert if not found
            
            HttpResponseMessage response = client.GetAsync(baseUri.Uri).Result;
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                Weather weather = JsonSerializer.Deserialize<Weather>(content);

                if (weather.api_info.status == "healthy")
                {
                    forecast = weather.items[0].forecasts;
                }
                else
                {
                    Console.WriteLine("Forecast not available at the moment.");
                    Console.WriteLine("Exiting program.");
                    Environment.Exit(0);
                }
            }
            return forecast;
        }

        public static void DisplayForecast(List<Forecast> forecastAll, string input)
        {
            if (forecastAll.Count == 0)
            {
                Console.WriteLine("For some reason....weather forecast is not available.");
            }
            else
            {
                foreach (Forecast forecast in forecastAll)
                {
                    if (forecast.area.ToLower() == input.ToLower())
                    {
                        Console.WriteLine($"{forecast.area} is {forecast.forecast}");
                    }
                }
            }
        }
    }
}
