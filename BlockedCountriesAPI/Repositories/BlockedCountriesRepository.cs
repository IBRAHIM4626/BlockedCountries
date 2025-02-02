using BlockedCountriesAPI.Models;
using System.Collections.Concurrent;

namespace BlockedCountriesAPI.Repositories
{
    public class BlockedCountriesRepository : IBlockedCountriesRepository
    {
        private readonly ConcurrentDictionary<string, bool> _blockedcountries = new();
        public bool AddBlockedCountry(string countryCode) 
        {
            return _blockedcountries.TryAdd(countryCode.ToUpper(), true);
        }
        public bool RemoveBlockedCountry(string countryCode) 
        {
            return _blockedcountries.TryRemove(countryCode.ToUpper(), out _);
        }
        public List<string> GetBlockedCountries() 
        {
            return _blockedcountries.Keys.ToList();
        }
        public bool IsBlockedCountry(string countryCode) 
        {
            return _blockedcountries.ContainsKey(countryCode.ToUpper());
        }
    }
}
