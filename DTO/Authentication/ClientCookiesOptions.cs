namespace ExamNest.DTO.Authentication
{
    public static class ClientCookiesOptions
    {
        public static CookieOptions Options => new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/",
            Expires = DateTimeOffset.UtcNow.AddSeconds(30)
        };

        public static CookieOptions RefreshTokenOptions => new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/",
            Expires = DateTimeOffset.UtcNow.AddHours(24)
        };
    }
}
