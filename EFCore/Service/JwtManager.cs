using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace EFCore.Service
{
    public class JwtManager
    {
        private static readonly IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        private static readonly string jwtSecurityKey = configuration["JwtSecurityKey"]!;
        private static readonly string jwtIssuer = configuration["JwtIssuer"]!;
        private static readonly string jwtAudience = configuration["JwtAudience"]!;
        private static readonly string jwtTokenExpiresInMinutes = configuration["JwtTokenExpiresInMinutes"]!;

        public static string GenerateToken(string userName)
        {
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSecurityKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: [
                new Claim(ClaimTypes.Name, userName) ,
                new Claim(ClaimTypes.Role,"Admin")

                ],
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtTokenExpiresInMinutes)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static (bool, ClaimsPrincipal?) ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                RefreshBeforeValidation = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSecurityKey))
            };
            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                

                return (true, principal); // توکن منقضی نیست
            }
            catch (SecurityTokenExpiredException)
            {
                return (false, null); // توکن منقضی است
            }
        }



    }
}