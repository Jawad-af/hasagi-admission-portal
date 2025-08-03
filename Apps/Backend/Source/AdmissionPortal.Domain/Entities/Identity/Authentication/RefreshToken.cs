namespace AdmissionPortal.Domain.Entities.Identity.Authentication
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public string UserId { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;
    }
}
