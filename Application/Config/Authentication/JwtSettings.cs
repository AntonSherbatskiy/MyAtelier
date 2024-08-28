namespace Application.Config.Authentication;

public class JwtSettings
{
    public static string SectionName = "AuthenticationSettings";
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public string Key { get; set; }
    public int ExpirationMinutes { get; set; }
}