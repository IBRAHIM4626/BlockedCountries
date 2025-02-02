using BlockedCountriesAPI.Models;
using BlockedCountriesAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlockedCountriesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockedCountriesController : ControllerBase
    {
        private readonly IBlockedCountriesService _blockedCountriesService;

        public BlockedCountriesController(IBlockedCountriesService blockedCountriesService)
        {
            _blockedCountriesService = blockedCountriesService;
        }

        [HttpPost("block")]
        public IActionResult BlockCountry([FromBody]CountryBlock request) 
        {
            if (string.IsNullOrWhiteSpace(request.CountryCode))
                return BadRequest("Invalid country code.");

            if (!_blockedCountriesService.BlockCountry(request.CountryCode))
                return Conflict("Country is already blocked.");

            return Ok($"{request.CountryCode} has been blocked.");
        }

        [HttpDelete("block/{countryCode}")]
        public IActionResult UnblockCountry(string countryCode) 
        {
            if(!_blockedCountriesService.UnblockCountry(countryCode))
                return NotFound("Country is not blocked.");

            return Ok($"{countryCode} has been unblocked.");
        }

        [HttpGet("blocked")]
        public IActionResult GetBlockCountries(int page=1, int pageSize = 10) 
        {
            var blockedCountries = _blockedCountriesService.GetBlockedCountries();
            var paginatedCountries = blockedCountries.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(new { Total = blockedCountries.Count, Countries = paginatedCountries });
        }

       
    }
}
