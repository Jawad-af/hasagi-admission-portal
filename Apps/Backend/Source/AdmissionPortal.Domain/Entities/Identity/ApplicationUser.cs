using Microsoft.AspNetCore.Identity;

namespace AdmissionPortal.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = default!;
    }
}
