using System;
using System.Threading.Tasks;

namespace HelpDesk.Api.Data
{
    public interface ISessionManager
    {
        Task<UserAccount> CreateAccountAsync(UserLogin loginInfo, string role);

        Task DeleteAccountAsync(UserLogin loginInfo);

        Task<UserAccount> GetAccountAsync(string username);

        Task<UserSession> GetSessionAsync(int sessionId);

        Task<UserSession> GetSessionAsync(string username);

        Task<UserSession> GetSessionAsync(Guid userToken);

        Task<UserSession> LoginAsync(UserLogin loginInfo);

        Task LogoutAsync(int sessionId);

        Task<UserAccount> UpdateAccountAsync(UserLogin loginInfo, string role);
    }
}
