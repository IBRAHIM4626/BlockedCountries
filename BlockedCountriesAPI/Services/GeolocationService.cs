using BlockedCountriesAPI.Models;
using Newtonsoft.Json;

namespace BlockedCountriesAPI.Services
{
    public class GeolocationService : IGeolocationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _apiBaseUrl;
        private readonly IGeolocationService _geolocationService;

        public GeolocationService(HttpClient httpClient, IConfiguration configuration, IGeolocationService geolocationService)
        {
            _httpClient = httpClient;
            _apiKey = configuration["GeoLocationAPI:Apikey"];
            _apiBaseUrl = configuration["GeoLocationAPI:BaseUrl"];
            _geolocationService = geolocationService;
        }

        public async Task<IPLookupResponse?> GetCountryCodeByIP(string ipAddress)
        {
            var url = $"{_apiBaseUrl}{ipAddress}/json/?key={_apiKey}";
            var response = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<IPLookupResponse>(response);
        }
    }
}
