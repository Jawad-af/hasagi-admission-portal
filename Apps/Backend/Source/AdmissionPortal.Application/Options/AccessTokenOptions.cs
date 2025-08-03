namespace AdmissionPortal.Application.Options
{
    public class AccessTokenOptions
    {
        public required string Secret { get; set; }
        public required string Audience { get; set; }
        public required string Issuer { get; set; }
        public required int TokenLifetimeMinutes { get; set; }
    }
}
