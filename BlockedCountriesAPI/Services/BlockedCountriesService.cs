using BlockedCountriesAPI.Repositories;

namespace BlockedCountriesAPI.Services
{
    public class BlockedCountriesService : IBlockedCountriesService
    {
        private readonly IBlockedCountriesRepository _blockedCountriesRepository;
        public BlockedCountriesService(IBlockedCountriesRepository blockedCountriesRepository) 
        {
            _blockedCountriesRepository = blockedCountriesRepository;
        }
        public bool BlockCountry(string countryCode) => _blockedCountriesRepository.AddBlockedCountry(countryCode);

        public bool UnblockCountry(string countryCode) => _blockedCountriesRepository.RemoveBlockedCountry(countryCode);

        public List<string> GetBlockedCountries() => _blockedCountriesRepository.GetBlockedCountries();

        public bool IsBlocked(string countryCode) => _blockedCountriesRepository.IsBlockedCountry(countryCode);
    }
}
