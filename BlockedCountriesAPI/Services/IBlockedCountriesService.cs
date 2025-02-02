namespace BlockedCountriesAPI.Services
{
    public interface IBlockedCountriesService
    {
        bool BlockCountry(string countryCode);
        bool UnblockCountry(string countryCode);
        List<string> GetBlockedCountries();
        bool IsBlocked(string countryCode);
    }
}
