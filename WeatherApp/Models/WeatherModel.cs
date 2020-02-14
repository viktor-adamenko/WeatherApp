using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace WeatherApp.Models
{
    public class WeatherModel
    {
        public IEnumerable<Weather> Weather { get; set; }
        public Main Main { get; set; }
        public Rain Rain { get; set; }
        public string Name { get; set; }

        public string Cod { get; set; }
        public string Message { get; set; }
    }

    public class Weather
    {
        public string Icon { get; set; }
    }

    public class Main
    {
        [JsonProperty("temp_min")]
        public double TempMin { get; set; }
        [JsonProperty("temp_max")]
        public double TempMax { get; set; }
    }

    public class Rain
    {
        [JsonProperty("1h")]
        public double Precipitation { get; set; }
    }
}