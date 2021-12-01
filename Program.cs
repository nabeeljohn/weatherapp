using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;

namespace ConsoleAppTest01
{
    class Program
    {
        static void Main(string[] args)
        {
            string apiUrl = $"http://dataservice.accuweather.com";
            bool check = true;
            do
            {               
                Console.Write("Enter ZipCode OR \'x\' to Exit: ");
                string zip = Console.ReadLine();

                check = !zip.Equals("x");

                if (check && !zip.Equals("x"))
                {
                    string locationUri = $"{apiUrl}/locations/v1/postalcodes/search?apikey=RUuBWKtaUKRJC4GtWkjGDuhStOZblr78&q={zip}";

                    List<Locality> locationData = MakeRequest<List<Locality>>(locationUri);

                    string weatherForecastUrl = $"{apiUrl}/forecasts/v1/daily/1day/{locationData[0].Key}?apikey=RUuBWKtaUKRJC4GtWkjGDuhStOZblr78";

                    WeatherForecast dailyForecast = MakeRequest<WeatherForecast>(weatherForecastUrl);

                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine($"City: {locationData[0].LocalizedName}{Environment.NewLine}Max Temp: {dailyForecast.DailyForecasts[0].Temperature.Maximum.Value}{dailyForecast.DailyForecasts[0].Temperature.Maximum.Unit}");
                    Console.WriteLine($"Min Temp: {dailyForecast.DailyForecasts[0].Temperature.Minimum.Value}{dailyForecast.DailyForecasts[0].Temperature.Maximum.Unit}");
                    Console.WriteLine($"Expect: {dailyForecast.Headline.Text}");
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine(Environment.NewLine);
                }
            }
            while (check);
 
        }

        protected static T MakeRequest<T>(string uri)
        {
            T result;

            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
            request.Method = "GET";

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string jsonString = reader.ReadToEnd();
                result = JsonConvert.DeserializeObject<T>(jsonString);                
            }

            return result;
        }

    }
}
