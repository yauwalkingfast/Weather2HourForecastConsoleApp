using System;
using System.Collections.Generic;
using System.Text;

namespace SGWeather2HoursConsole
{
    // Weather myDeserializedClass = JsonConvert.DeserializeObject<Weather>(myJsonResponse); 
    public class LabelLocation
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class AreaMetadata
    {
        public string name { get; set; }
        public LabelLocation label_location { get; set; }
    }

    public class ValidPeriod
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }
    }

    public class Forecast
    {
        public string area { get; set; }
        public string forecast { get; set; }
    }

    public class Item
    {
        public DateTime update_timestamp { get; set; }
        public DateTime timestamp { get; set; }
        public ValidPeriod valid_period { get; set; }
        public List<Forecast> forecasts { get; set; }
    }

    public class ApiInfo
    {
        public string status { get; set; }
    }

    public class Weather
    {
        public List<AreaMetadata> area_metadata { get; set; }
        public List<Item> items { get; set; }
        public ApiInfo api_info { get; set; }
    }


}
