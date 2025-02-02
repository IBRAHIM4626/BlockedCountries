using BlockedCountriesAPI.Models;

namespace BlockedCountriesAPI.Services
{
    public interface IGeolocationService
    {
        public Task<IPLookupResponse> GetCountryCodeByIP(string ipAddress);
    }
}
