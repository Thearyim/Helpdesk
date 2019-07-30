using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpDesk.Api.Data
{
    public interface ITicketManager
    {
        Task<Ticket> CreateTicketAsync(UserSession session, Ticket ticket);

        Task DeleteTicketAsync(UserSession session, int ticketId);

        Task<IEnumerable<Ticket>> GetTicketsAsync(UserSession session, int? ticketId = null);

        Task<Ticket> UpdateTicketAsync(UserSession session, Ticket ticket);
    }
}
