using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.Api.Data
{
    public class InMemoryDataStore : IDataStore
    {
        private string encryptionKey;
        private Guid initializationVector;
        private Dictionary<string, UserAccount> userAccounts;
        private Dictionary<int, UserSession> userSessions;

        public InMemoryDataStore()
            : this("SuperSecretKey77", Guid.Empty)
        {
        }

        public InMemoryDataStore(string encryptionKey, Guid initializationVector)
        {
            this.userAccounts = new Dictionary<string, UserAccount>(StringComparer.OrdinalIgnoreCase);
            this.userSessions = new Dictionary<int, UserSession>();

            this.encryptionKey = encryptionKey;
            this.initializationVector = initializationVector;
        }

        public Task<UserAccount> CreateAccountAsync(UserLogin loginInfo, string role)
        {
            UserAccount account;
            if (this.userAccounts.TryGetValue(loginInfo.Username, out account))
            {
                throw new AccountExistsException($"An account already exists with the username '{loginInfo.Username}'.");
            }

            account = new UserAccount(
                id: this.userAccounts.Count + 1,
                username: loginInfo.Username,
                password: UserAccount.Encrypt(loginInfo.Password, this.encryptionKey, this.initializationVector),
                role: role);

            this.userAccounts.Add(loginInfo.Username, account);

            return Task.FromResult(account);
        }

        public async Task DeleteAccountAsync(UserLogin loginInfo)
        {
            UserAccount account = await this.GetAccountAsync(loginInfo.Username)
                .ConfigureAwait(false);

            this.ThrowIfPasswordDoesNotMatch(loginInfo, account);

            this.userAccounts.Remove(account.Username);
        }

        public Task<UserAccount> GetAccountAsync(string username)
        {
            UserAccount account;
            if (!this.userAccounts.TryGetValue(username, out account))
            {
                throw new AccountNotFoundException($"An account does not exist with the username '{username}'.");
            }

            return Task.FromResult(account);
        }

        public async Task<UserSession> GetSessionAsync(string username)
        {
            UserAccount account = await this.GetAccountAsync(username).ConfigureAwait(false);
            UserSession session = await this.GetOrExpireUserSessionAsync(account.Id).ConfigureAwait(false);

            if (session == null)
            {
                throw new SessionNotFoundException($"A session does not exist for user account '{account.Username}'.");
            }

            return session;
        }

        public async Task<UserSession> LoginAsync(UserLogin loginInfo)
        {
            UserAccount existingAccount = await this.GetAccountAsync(loginInfo.Username).ConfigureAwait(false);

            this.ThrowIfPasswordDoesNotMatch(loginInfo, existingAccount);

            UserSession session = await this.GetOrExpireUserSessionAsync(existingAccount.Id).ConfigureAwait(false);

            if (session == null)
            {
                session = new UserSession(
                    id: this.userSessions.Count + 1,
                    userId: existingAccount.Id,
                    userRole: "User",
                    token: Guid.NewGuid(),
                    expiration: DateTime.UtcNow.AddMinutes(30));

                this.userSessions.Add(session.Id, session);
            }

            return session;
        }

        public Task LogoutAsync(int sessionId)
        {
            this.userSessions.Remove(sessionId);
            return Task.CompletedTask;
        }

        public async Task<UserAccount> UpdateAccountAsync(UserLogin loginInfo, string role)
        {
            UserAccount existingAccount = await this.GetAccountAsync(loginInfo.Username)
                .ConfigureAwait(false);

            this.ThrowIfPasswordDoesNotMatch(loginInfo, existingAccount);

            UserAccount updatedAccount = new UserAccount(
                id: existingAccount.Id,
                username: loginInfo.Username,
                password: UserAccount.Encrypt(loginInfo.Password, this.encryptionKey, this.initializationVector),
                role: role);

            this.userAccounts[existingAccount.Username] = updatedAccount;

            return updatedAccount;
        }

        private Task<UserSession> GetOrExpireUserSessionAsync(int userId)
        {
            KeyValuePair<int, UserSession> sessionEntry = this.userSessions.FirstOrDefault(s => s.Value.UserId == userId);
            UserSession userSession = sessionEntry.Value;
            if (userSession != null && DateTime.UtcNow > userSession.Expiration)
            {
                // Expire the session
                this.userSessions.Remove(userSession.Id);
                userSession = null;
            }

            return Task.FromResult(userSession);
        }

        private void ThrowIfPasswordDoesNotMatch(UserLogin loginInfo, UserAccount account)
        {
            if (loginInfo.Password != UserAccount.Decrypt(account.Password, this.encryptionKey, this.initializationVector))
            {
                throw new AccountAuthorizationException("Invalid credentials supplied. The username or password is invalid.");
            }
        }
    }
}
