namespace SKP.Net.Services.Authentication
{
    public static class SkpAuthenticationDefaults
    {
        public static string ClaimIssuer { get; set; } = "skpatel.net";
        public static string AuthenticationScheme { get; set; } = "Authentication";
        public static string ExternalAuthenticationScheme { get; set; } = "ExternalAuthentication";
        public static string Prefix { get; set; } = ".skp";
        public static string CustomerCookie { get; set; } = ".customer";
    }
}
