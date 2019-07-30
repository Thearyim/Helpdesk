using System;

namespace HelpDesk.Api.Data
{
    public class HelpRequest
    {
        public TicketRequest Ticket { get; set; }

        public Guid UserToken { get; set; }
    }
}
