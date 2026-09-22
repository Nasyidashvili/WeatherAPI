namespace WeatherAPI.Model
{
    public class VisualCrossingDay
    {
        public string? Datetime { get; set; }
        public double Temp { get; set; }
        public double Humidity { get; set; }
        public string? Conditions { get; set; }
        public string? Description { get; set; }
    }
}
