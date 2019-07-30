using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpDesk.Api.Data
{
    public class InMemoryTicketManager : ITicketManager
    {
        private Dictionary<int, Ticket> tickets;

        public InMemoryTicketManager()
        {
            this.tickets = new Dictionary<int, Ticket>();
        }

        public Task<Ticket> CreateTicketAsync(UserSession session, Ticket ticket)
        {
            ticket.Id = this.tickets.Count + 1;
            ticket.Status = TicketStatus.New;
            ticket.CreatedByUserId = session.UserId;
            ticket.CreatedTime = DateTime.UtcNow;

            this.tickets.Add(ticket.Id, ticket);

            return Task.FromResult(ticket);
        }

        public Task DeleteTicketAsync(UserSession session, int ticketId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Ticket>> GetTicketsAsync(UserSession session, int? ticketId = null)
        {
            List<Ticket> matchingTickets = new List<Ticket>();

            if (ticketId != null)
            {
                Ticket ticket;
                if (this.tickets.TryGetValue(ticketId.Value, out ticket))
                {
                    if (!session.CanModify(ticket))
                    {
                        throw new UnauthorizedAccessException(
                            $"The ticket with ID {ticketId} is not owned by user for the session with ID '{session.UserId}'.");
                    }

                    matchingTickets.Add(ticket);
                }
            }
            else if (session.IsAdmin())
            {
                matchingTickets.AddRange(this.tickets.Select(entry => entry.Value).OrderBy(t => t.CreatedByUserId));
            }
            else
            {
                matchingTickets.AddRange(this.tickets.Select(entry => entry.Value).Where(t => t.CreatedByUserId == session.UserId));
            }

            return Task.FromResult(matchingTickets as IEnumerable<Ticket>);
        }

        public Task<Ticket> UpdateTicketAsync(UserSession session, Ticket ticket)
        {
            throw new NotImplementedException();
        }
    }
}
