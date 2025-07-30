using AdmissionPortal.Domain.Abstractions;
using AdmissionPortal.Domain.Entities.Identity;

namespace AdmissionPortal.Application.Validations.Identity.Interfaces
{
    public interface IIdentityManagerValidation : IDomainService
    {
        Task Validate_CreateUser(ApplicationUser user, string password, CancellationToken cancellationToken = default);
        Task Validate_LoginUser(ApplicationUser user, string password, CancellationToken cancellationToken = default);
        Task<ApplicationUser> Validate_GetUserByEmail(string email, CancellationToken cancellationToken = default);
        Task<ApplicationUser> Validate_GetUserById(string userId, CancellationToken cancellationToken = default);
    }
}
