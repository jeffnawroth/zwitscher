namespace iva_grp7_backend.Configurations;

public class JwtConfig
{
    public string Secret { get; set; }
    public TimeSpan ExpiryTimeFrame { get; set; }
}