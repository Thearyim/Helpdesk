using System;
using Newtonsoft.Json;

namespace HelpDesk.Api.Data
{
    public class UserSession
    {
        //
        // Note:
        // We are mocking out the use of user authorization tokens in this project.  This would not be a secure model
        // in the real world.  We would want to implement a proper identity server to handle JWT.
        // 
        // Example:
        // https://developer.okta.com/blog/2018/03/23/token-authentication-aspnetcore-complete-guide
        // http://hamidmosalla.com/2017/12/07/policy-based-authorization-using-asp-net-core-2-identityserver4/

        public UserSession(int id, int userId, string username, string userRole, Guid token, DateTime expiration)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("The user role parameter must be defined", nameof(username));
            }

            if (string.IsNullOrWhiteSpace(userRole))
            {
                throw new ArgumentException("The user role parameter must be defined", nameof(userRole));
            }

            if (id < 0)
            {
                throw new ArgumentException("The id parameter must be defined", nameof(id));
            }

            if (userId < 0)
            {
                throw new ArgumentException("The user id parameter must be defined", nameof(userId));
            }

            if (token == Guid.Empty)
            {
                throw new ArgumentException("The token parameter must be a valid Guid", nameof(token));
            }

            this.Id = id;
            this.UserId = userId;
            this.Username = username;
            this.UserRole = userRole;
            this.Token = token;
            this.Expiration = expiration;
        }

        [JsonProperty("id")]
        public int Id { get; }

        [JsonProperty("userId")]
        public int UserId { get; }

        [JsonProperty("username")]
        public string Username { get; }

        [JsonProperty("userRole")]
        public string UserRole { get; }

        [JsonProperty("token")]
        public Guid Token { get; }

        [JsonProperty("expiration")]
        public DateTime Expiration { get; }

    }
}
