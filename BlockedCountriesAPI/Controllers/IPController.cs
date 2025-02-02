using BlockedCountriesAPI.Models;
using BlockedCountriesAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace BlockedCountriesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IPController : ControllerBase
    {
        private readonly GeolocationService _geolocationService;
        private readonly BlockedCountriesService _blockedCountriesService;

        private static readonly ConcurrentBag<BlockedAttempt> _blockedAttempts = new();
        private static readonly ConcurrentQueue<BlockedLog> _blockedLogs = new();

        public IPController(GeolocationService geolocationService, BlockedCountriesService blockedCountriesService)
        {
            _geolocationService = geolocationService;
            _blockedCountriesService = blockedCountriesService;
        }


        [HttpGet("lookup")]
        public async Task<IActionResult> FindCountryByIP(string? ipAddress = null)
        {
            ipAddress ??= HttpContext.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrWhiteSpace(ipAddress))
                return BadRequest("Invalid IP address.");

            var result = await _geolocationService.GetCountryCodeByIP(ipAddress);
            if (result == null)
                return BadRequest("Could not fetch IP details.");

            return Ok(new { CountryCode = result.CountryCode, CountryName = result.CountryName, ISP = result.ISP });
        }

        [HttpGet("check-block")]
        public async Task<IActionResult> CheckIfIPBlocked()
        {
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            string userAgent = Request.Headers["User-Agent"].ToString();

            if (string.IsNullOrEmpty(ip))
                return BadRequest("Could not determine IP.");

            var countryInfoResult = await FindCountryByIP(ip);

            if (countryInfoResult is BadRequestObjectResult badRequest)
                return badRequest;

            if (countryInfoResult is OkObjectResult okResult)
            {
                var countryInfo = okResult.Value as dynamic;
                string countryCode = countryInfo?.CountryCode ?? "Unknown";

                bool isBlocked = _blockedCountriesService.IsBlocked(countryCode);

                // Log the attempt
                _blockedAttempts.Add(new BlockedAttempt
                {
                    IPAddress = ip,
                    Timestamp = DateTime.UtcNow,
                    CountryCode = countryCode,
                    IsBlocked = isBlocked,
                    UserAgent = userAgent
                });

                return Ok(new { Blocked = isBlocked, Message = isBlocked ? "Access Denied" : "Access Granted" });
            }

            return BadRequest("Unexpected error while checking IP.");
        }


        [HttpGet("logs/blocked-attempts")]
        public IActionResult GetBlockedLogs(int page = 1, int pageSize = 10)
        {
            var paginatedLogs = _blockedLogs.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(new { Total = _blockedLogs.Count, Logs = paginatedLogs });
        }

        [HttpPost("temporal-block")]
        public IActionResult TemporarilyBlockCountry([FromBody] TemporalBlockRequest request)
        {
            if(request.DurationMinutes < 1 || request.DurationMinutes > 1440)
                return BadRequest("Duration must be between 1 and 1440 minutes.");

            if (_blockedCountriesService.IsBlocked(request.CountryCode))
                return Conflict("Country is already blocked.");

            _blockedCountriesService.BlockCountry(request.CountryCode);

            Task.Delay(TimeSpan.FromMinutes(request.DurationMinutes)).ContinueWith(_ =>
            {
                _blockedCountriesService.UnblockCountry(request.CountryCode);
            });

            return Ok($"{request.CountryCode} has been temporarily blocked for {request.DurationMinutes} minutes.");
        }
    }
}
