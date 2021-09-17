using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace CacheWebApiClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Authenticate a user against DB....
            // Done. User is authenticated. Load user's profile
            // User's profile is loaded into a UserProfile object
            var userProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                Email = "milad@humber.ca",
                Province = "ON",
                Roles = new [] { "Teacher", "Manager" }
            };
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userProfile.Id.ToString()),
                new Claim(ClaimTypes.Email, userProfile.Email),
                new Claim("Province", userProfile.Province),
            };
            foreach (var role in userProfile.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            byte[] keyBytes = Encoding.ASCII.GetBytes("qwertyuiopasdfghjklzxcvbnm123456");
            var signingKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(
                "Distributed Dev Organization",
                "www.humber.ca",
                claims,
                DateTime.Now,
                DateTime.Now.AddYears(1),
                credentials
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            Console.WriteLine(jwt);
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");
            httpClient.DefaultRequestHeaders.Add("custom-header", "DistDevRequest");
            var response = await httpClient.GetAsync("https://localhost:6001/api/cache/v1/mykey2?culture=en");
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
    }

    class UserProfile
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Province { get; set; }
        public string[] Roles { get; set; }
    }
}
