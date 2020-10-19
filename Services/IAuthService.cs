using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ShoppingDB.Models;

namespace ShoppingDB.Services
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(User user, string password);
        Task<User> LoginAsync(string userName, string password);
        Task LogoutAsync();
        Task<bool> UserExistsAsync(string userName);
    }
}
