using AdmissionPortal.Application.DTOs.Identity;
using AdmissionPortal.Application.Options;
using AdmissionPortal.Application.Services.Identity.Interfaces;
using AdmissionPortal.Domain.Entities.Identity;
using AdmissionPortal.Domain.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AdmissionPortal.Infrastructure.Services.Identity
{
    public class TokenService : ITokenService
    {
        private readonly JwtOptions _jwtOptions;
        private IApplicationDbContext _context;

        public TokenService(IOptions<JwtOptions> jwtOptions,
                            IApplicationDbContext context)
        {
            _jwtOptions = jwtOptions.Value;
            _context = context;
        }

        public async Task<AuthenticationResponseDto> GenerateTokensAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            var accessToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken(user.Id);

            await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new AuthenticationResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                ExpiresAt = refreshToken.ExpiresAt
            };
        }

        public string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email ?? ""),
                new(ClaimTypes.Name, user.FullName ?? "")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.TokenLifetimeMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshToken GenerateRefreshToken(string userId)
        {
            return new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenLifetimeDays),
                UserId = userId
            };
        }
    }
}
