namespace BlockedCountriesAPI.Models
{
    public class IPLookupResponse
    {
        public string IP { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string ISP { get; set; }
    }
}
