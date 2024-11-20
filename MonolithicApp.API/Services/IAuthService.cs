using MonolithicApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonolithicApp.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(UserRequest request);
        Task<AuthResponse> LoginAsync(string username, string password);
        Task<bool> FindByNameAsync(string username);
        Task<AuthResponse> RefreshTokenAsync(string refreshToken);
    }
}
