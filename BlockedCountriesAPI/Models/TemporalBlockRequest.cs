﻿namespace BlockedCountriesAPI.Models
{
    public class TemporalBlockRequest
    {
        public string CountryCode { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
    }
}
