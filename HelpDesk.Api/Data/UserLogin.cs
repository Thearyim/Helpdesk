using System;
using Newtonsoft.Json;

namespace HelpDesk.Api.Data
{
    public class UserLogin
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("newPassword")]
        public string NewPassword { get; set; }
    }
}
