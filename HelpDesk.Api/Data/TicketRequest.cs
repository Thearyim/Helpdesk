using System;
using System.Collections.Generic;

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

        public string Title { get; }

        public string Description { get; }

        public IDictionary<string, object> Context { get; }
    }
}
