using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.Api.Data
{
    public class InMemorySessionManager : ISessionManager
    {
        private string encryptionKey;
        private Guid initializationVector;
        private Dictionary<string, UserAccount> userAccounts;
        private Dictionary<int, UserSession> userSessions;

        public InMemorySessionManager()
            : this("SuperSecretKey77", Guid.Empty)
        {
        }

        public InMemorySessionManager(string encryptionKey, Guid initializationVector)
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

        public Task<UserSession> GetSessionAsync(int sessionId)
        {
            UserSession session;
            if (!this.userSessions.TryGetValue(sessionId, out session))
            {
                throw new SessionNotFoundException($"A session does not exist for id '{sessionId}'.");
            }
            return Task.FromResult(session);
        }

        public async Task<UserSession> GetSessionAsync(string username)
        {
            UserAccount account = await this.GetAccountAsync(username)
                .ConfigureAwait(false);

            UserSession session = await this.GetOrExpireUserSessionAsync(account.Id)
                .ConfigureAwait(false);

            if (session == null)
            {
                throw new SessionNotFoundException($"A session does not exist for user '{username}'.");
            }
            return session;
        }

        public async Task<UserSession> GetSessionAsync(Guid userToken)
        {
            UserSession session = await this.GetOrExpireUserSessionAsync(userToken: userToken).ConfigureAwait(false);

            if (session == null)
            {
                throw new SessionNotFoundException($"A session does not exist for user.");
            }

            return session;
        }

        public async Task<UserSession> LoginAsync(UserLogin loginInfo)
        {
            UserAccount existingAccount = await this.GetAccountAsync(loginInfo.Username).ConfigureAwait(false);

            this.ThrowIfPasswordDoesNotMatch(loginInfo, existingAccount);

            UserSession session = await this.GetOrExpireUserSessionAsync(userId: existingAccount.Id).ConfigureAwait(false);

            if (session == null)
            {
                session = new UserSession(
                    id: this.userSessions.Count + 1,
                    userId: existingAccount.Id,
                    username: loginInfo.Username,
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

        private Task<UserSession> GetOrExpireUserSessionAsync(int? userId = null, Guid? userToken = null)
        {
            UserSession userSession = null;
            if (userId != null)
            {
                userSession = this.userSessions.FirstOrDefault(s => s.Value.UserId == userId).Value;
            }
            else if (userToken != null)
            {
                userSession = this.userSessions.FirstOrDefault(s => s.Value.Token == userToken).Value;
            }

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
