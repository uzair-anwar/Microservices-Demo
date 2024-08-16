using Users.Data.Models;

namespace Users.Services
{
    public interface IOktaService
    {
        Task<OktaResponse> GetTokenAsync(string username, string password);
        Task<bool> AuthenticateToken(string token);
        Task<UserInfo> GetUserAsync(string uid);
    }

}
