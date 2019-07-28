using System;

namespace HelpDesk.Api.Data
{
    public class UserLogin
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string NewPassword { get; set; }
    }
}
