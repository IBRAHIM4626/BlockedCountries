namespace BlockedCountriesAPI.Repositories
{
    public interface IBlockedCountriesRepository
    {
        public bool AddBlockedCountry(string countryCode);
        public bool RemoveBlockedCountry(string countryCode);
        public List<string> GetBlockedCountries();
        public bool IsBlockedCountry(string countryCode);
    }
}
