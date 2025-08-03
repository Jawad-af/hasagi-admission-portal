namespace AdmissionPortal.Application.Options
{
    public class RefreshTokenOptions
    {
        public required string Secret { get; set; }
        public required string Audience { get; set; }
        public required string Issuer { get; set; }
        public required int TokenLifetimeDays { get; set; }
    }
}
