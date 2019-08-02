using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HelpDesk.Api.Data
{
    public class TicketRequest
    {
        public TicketRequest(string title, string description, IDictionary<string, object> context = null)
        {
            this.Title = title;
            this.Description = description;
            this.Context = context;
        }

        [JsonProperty("title")]
        public string Title { get; }

        [JsonProperty("description")]
        public string Description { get; }

        [JsonProperty("context")]
        public IDictionary<string, object> Context { get; }
    }
}
