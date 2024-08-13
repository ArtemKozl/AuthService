using Microsoft.IdentityModel.Tokens;
using OnionTest.Application.Services;
using OnionTest.DataAccess.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace OnionTest.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public JwtMiddleware(RequestDelegate next, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _configuration = configuration;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var refreshTokenRepository = scope.ServiceProvider.GetRequiredService<IRefreshTokenRepository>();
            var refereshTokenService = scope.ServiceProvider.GetRequiredService<IRefreshtokenService>();

            if (context.Request.Path.StartsWithSegments("/Users/register") || context.Request.Path.StartsWithSegments("/Users/login") || context.Request.Path.StartsWithSegments("/Users/UserExistByEmail"))
            {
                await _next(context);
                return;
            }

            var token = context.Request.Cookies["tasty-cookie"];

            if (!string.IsNullOrEmpty(token))
            {
                var jwtSettings = _configuration.GetSection("JwtSettings");
                var secretKeyString = _configuration.GetValue<string>("SECRET_KEY");

                if (secretKeyString == null)
                {
                    throw new InvalidOperationException("Secret key cannot be null.");
                }

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _configuration.GetValue<string>("JwtOptions:ValidIssuer"),
                    ValidateAudience = true,
                    ValidAudience = _configuration.GetValue<string>("JwtOptions:ValidAudience"),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKeyString))
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken == null || jwtSecurityToken.Header.Alg == null)
                {
                    throw new InvalidOperationException("Invalid JWT token.");
                }

                bool isValid = true;

                if (!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
                    isValid = false;

                if (isValid)
                {
                    var expirationClaim = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "exp");
                    if (expirationClaim != null && DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(expirationClaim.Value)) <= DateTime.UtcNow)
                    {
                        var userIdClaim = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "userId");
                        if (userIdClaim != null)
                        {
                            var userId = Convert.ToInt32(userIdClaim.Value);
                            var userDbId = await refreshTokenRepository.Get(userId);
                            if (userDbId)
                            {
                                var tokens = await refereshTokenService.Refresh(userId);

                                context.Response.Cookies.Append("tasty-cookie", tokens["access"]);
                                context.Response.Cookies.Append("tastiest-cookie", tokens["refresh"]);

                                await _next(context);
                            }
                            else
                            {
                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                return;
                            }
                        }
                    }
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }
    }
}
