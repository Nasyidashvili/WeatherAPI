namespace WeatherAPI.Model
{
    public class VisualCrossingResponse
    {
        public string? Address { get; set; }
        public string? Timezone { get; set; }
        public List<VisualCrossingDay>? Days { get; set; }
    }
}
