using AdmissionPortal.Application.Validations.Identity.Interfaces;
using AdmissionPortal.Domain.Entities.Identity;
using AdmissionPortal.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Ultimate.Exceptions.Models;

namespace AdmissionPortal.Application.Validations.Identity
{
    public class IdentityManagerValidation : IIdentityManagerValidation
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IdentityManagerValidation(UserManager<ApplicationUser> userManager,
                                         SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task Validate_CreateUser(ApplicationUser user, string password, CancellationToken cancellationToken = default)
        {
            IdentityResult result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                AdmissionPortalExceptions.ThrowUnProcessableEntityException([.. result.Errors.Select(x => new ErrorDetail(x.Description))]);
            }
        }

        public async Task<ApplicationUser> Validate_GetUserByEmail(string email, CancellationToken cancellationToken = default)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                AdmissionPortalExceptions.ThrowUnAuthorizedException("Invalid email or password");
            }

            return user!;
        }

        public async Task<ApplicationUser> Validate_GetUserById(string userId, CancellationToken cancellationToken = default)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                AdmissionPortalExceptions.ThrowNotFoundException(nameof(ApplicationUser));
            }

            return user!;
        }

        public async Task Validate_LoginUser(ApplicationUser user, string password, CancellationToken cancellationToken = default)
        {
            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if(!result.Succeeded)
            {
                AdmissionPortalExceptions.ThrowUnAuthorizedException("Invalid email or password");
            }

        }
    }
}
