﻿using System.Threading.Tasks;

namespace HelpDesk.Api.Data
{
    public interface IDataStore
    {
        Task<UserAccount> CreateAccountAsync(UserLogin loginInfo, string role);

        Task DeleteAccountAsync(UserLogin loginInfo);

        Task<UserAccount> GetAccountAsync(string username);

        Task<UserSession> GetSessionAsync(string username);

        Task<UserSession> LoginAsync(UserLogin loginInfo);

        Task LogoutAsync(int sessionId);

        Task<UserAccount> UpdateAccountAsync(UserLogin loginInfo, string role);
    }
}
