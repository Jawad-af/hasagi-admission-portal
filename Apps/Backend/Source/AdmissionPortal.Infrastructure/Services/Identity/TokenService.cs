using AdmissionPortal.Application.DTOs.Identity;
using AdmissionPortal.Application.Options;
using AdmissionPortal.Application.Services.Identity.Interfaces;
using AdmissionPortal.Domain.Entities.Identity;
using AdmissionPortal.Domain.Entities.Identity.Authentication;
using AdmissionPortal.Domain.Enums.Identity.Authentication;
using AdmissionPortal.Domain.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AdmissionPortal.Infrastructure.Services.Identity
{
    public class TokenService : ITokenService
    {
        private readonly AccessTokenOptions _accessTokenOptions;
        private readonly RefreshTokenOptions _refreshTokenOptions;
        private readonly IApplicationDbContext _context;
        private readonly TokenValidationParameters _validationParameters;
        public TokenService(IOptions<AccessTokenOptions> accessTokenOptions,
                            IOptions<RefreshTokenOptions> refreshTokenOptions,
                            IApplicationDbContext context,
                            TokenValidationParameters validationParameters)
        {
            _accessTokenOptions = accessTokenOptions.Value;
            _refreshTokenOptions = refreshTokenOptions.Value;
            _context = context;
            _validationParameters = validationParameters;
        }

        public async Task<AuthenticationResponseDto> GenerateTokensAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            var accessToken = BuildToken(user, TokenType.Access);
            var refreshToken = BuildToken(user, TokenType.Refresh);

            RefreshToken refresh = new()
            {
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(_refreshTokenOptions.TokenLifetimeDays),
                UserId = user.Id,
            };

            await _context.RefreshTokens.AddAsync(refresh, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new AuthenticationResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = refresh.ExpiresAt
            };
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string accessToken, string refreshToken)
        {
            // Disable lifetime validation so we can read expired tokens
            TokenValidationParameters validationParams = _validationParameters.Clone();
            validationParams.ValidateLifetime = false;

            var tokenHandler = new JwtSecurityTokenHandler();

            // This will succeed even if token is expired
            var principal = tokenHandler.ValidateToken(accessToken, validationParams, out var securityToken);

            // Optional: verify it really is a JWT
            if (securityToken is JwtSecurityToken jwt &&
                jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return principal;
            }

            return null;
        }

        private string BuildToken(ApplicationUser user, TokenType tokenType)
        {
            bool isAccessToken = tokenType == TokenType.Access;

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email,          user.Email ?? ""),
                new(ClaimTypes.Name,           user.FullName ?? ""),
                new("token_type",              tokenType.ToString())           // custom claim
            };

            string secret = isAccessToken ? _accessTokenOptions.Secret : _refreshTokenOptions.Secret;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: isAccessToken ? _accessTokenOptions.Issuer : _refreshTokenOptions.Issuer,
                audience: isAccessToken ? _accessTokenOptions.Audience : _refreshTokenOptions.Audience,
                claims: claims,
                expires: isAccessToken ? DateTime.UtcNow.AddMinutes(_accessTokenOptions.TokenLifetimeMinutes) :
                                         DateTime.UtcNow.AddDays(_refreshTokenOptions.TokenLifetimeDays),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
