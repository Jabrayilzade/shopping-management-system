using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShoppingDB.DataContext;
using ShoppingDB.Helpers;
using ShoppingDB.Models;
using ShoppingDB.Services;

namespace ShoppingDB.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDataContext context;
        public AuthService(AppDataContext context)
        {
            this.context = context;
        }
        public async Task<User> LoginAsync(string userName, string password)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
                return null;
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            user.Token = GenerateToken(user);
            return user;
        }

        public async Task<User> RegisterAsync(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;


            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UserExistsAsync(string userName)
        {
            if (await context.Users.AnyAsync(u => u.UserName == userName))
                return true;
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPasswordHash(string password, byte[] userPasswordHash, byte[] userPasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(userPasswordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int a = 0; a < computedHash.Length; a++)
                    if (computedHash[a] != userPasswordHash[a])
                        return false;
                return true;
            }
        }

        public string GenerateToken(User user)
        {
            var key = AuthOptions.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.UserRole),
            };

            var token = new JwtSecurityToken
                (
                    issuer: null,
                    audience: null,
                    expires: DateTime.Now.AddMinutes(30),
                    claims: claims,
                    signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }
}
