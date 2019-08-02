using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HelpDesk.Api.Data
{
    public class Ticket
    {
        public Ticket()
        {
            this.Context = new Dictionary<string, object>();
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("assignedToUserId")]
        public int AssignedToUserId { get; set; }

        [JsonProperty("createdByUserId")]
        public int CreatedByUserId { get; set; }

        [JsonProperty("createdTime")]
        public DateTime CreatedTime { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("context")]
        public Dictionary<string, object> Context { get; }
    }
}
