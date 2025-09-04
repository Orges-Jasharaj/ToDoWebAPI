﻿namespace ToDoWebAPI.Dtos.System
{
    public class JwtSettings
    {
        public const string SectionName = "JwtSettings";
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public int ExpirationInMinutes { get; set; } = 60;

        public int RefreshTokenExpirationInDays { get; set; } = 7;
        public double AccessTokenExpirationMinutes { get; internal set; }
    }
}
