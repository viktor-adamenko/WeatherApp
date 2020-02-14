using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using WeatherApp.Models;

namespace WeatherApp.BusinessLogic
{
    public class WeatherApiProcessor
    {
        private readonly string _apiKey;

        public WeatherApiProcessor(string apiKey)
        {
            _apiKey = apiKey;
        }

        public WeatherModel GetWeatherData(string cityName)
        {
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(BuildRequestString(cityName));

                var response = (HttpWebResponse) request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                var result = JsonConvert.DeserializeObject<WeatherModel>(responseString);

                return result;
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var responseString = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    var result = JsonConvert.DeserializeObject<WeatherModel>(responseString);

                    return result;
                }

                return new WeatherModel {Cod = "500", Message = ex.Message};
            }
        }

        private string BuildRequestString(string cityName)
        {
            string url = "https://api.openweathermap.org/data/2.5/weather?";

            StringBuilder sb = new StringBuilder(url);
            sb.Append($"q={cityName}&");
            sb.Append($"appid={_apiKey}&");
            sb.Append($"units=metric");

            return sb.ToString();
        }
    }
}