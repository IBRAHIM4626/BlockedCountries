namespace BlockedCountriesAPI.Models
{
    public class BlockedLog
    {
        public string IpAddress { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
