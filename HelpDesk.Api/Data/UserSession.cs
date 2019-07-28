using System;

namespace HelpDesk.Api.Data
{
    public class UserSession
    {
        public UserSession(int id, int userId, string userRole, Guid token, DateTime expiration)
        {
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
            this.UserRole = userRole;
            this.Token = token;
            this.Expiration = expiration;
        }

        public int Id { get; }

        public int UserId { get; }

        public string UserRole { get; }

        public Guid Token { get; }

        public DateTime Expiration { get; }

    }
}
