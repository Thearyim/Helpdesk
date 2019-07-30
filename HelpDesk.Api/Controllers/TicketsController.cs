using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpDesk.Api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Api.Controllers
{
    [Route("api/tickets")]
    public class TicketsController : Controller
    {
        public TicketsController(ISessionManager sessionManager, ITicketManager ticketManager)
        {
            this.SessionManager = sessionManager
                ?? throw new ArgumentException("The session manager parameter is required.", nameof(sessionManager));

            this.TicketManager = ticketManager
              ?? throw new ArgumentException("The ticket manager parameter is required.", nameof(ticketManager));
        }

        protected ISessionManager SessionManager { get; }

        protected ITicketManager TicketManager { get; }

        // POST: /api/tickets?token=F8A25763-016E-4C9D-A15B-5E1C0F50100B
        //
        // BODY:
        // {
        //   'Title': 'Any ticket title',
        //   'Description': 'A description of the help desk request',
        //   'CreatedTime': '2019-07-29T13:45:30.0000000Z',
        //   ...
        // }
        [HttpPost]
        public async Task<IActionResult> CreateTicketAsync([FromBody] TicketRequest ticket, [FromQuery] Guid? token)
        {
            //
            // Note:
            // We are mocking out the use of user authorization tokens in this project.  This would not be a secure model
            // in the real world.  We would want to implement a proper identity server to handle JWT.
            // 
            // Example:
            // https://developer.okta.com/blog/2018/03/23/token-authentication-aspnetcore-complete-guide
            // http://hamidmosalla.com/2017/12/07/policy-based-authorization-using-asp-net-core-2-identityserver4/

            IActionResult response = null;

            try
            {
                if (token == null)
                {
                    throw new SessionNotFoundException("User session token not provided.");
                }

                UserSession userSession = await this.SessionManager.GetSessionAsync(token.Value)
                    .ConfigureAwait(false);

                Ticket newTicket = new Ticket
                {
                    Description = ticket.Description,
                    Title = ticket.Title
                };

                Ticket ticketCreated = await this.TicketManager.CreateTicketAsync(userSession, newTicket)
                    .ConfigureAwait(false);

                response = this.Ok(ticketCreated);
            }
            catch (UnauthorizedAccessException exc)
            {
                response = this.Forbid(exc.Message);
            }
            catch (AccountNotFoundException)
            {
                response = this.Unauthorized();
            }
            catch (SessionNotFoundException)
            {
                response = this.Unauthorized();
            }
            catch (Exception exc)
            {
                response = new ObjectResult(exc.Message)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            return response;
        }

        // DELETE: /api/tickets/123?token=F8A25763-016E-4C9D-A15B-5E1C0F50100B
        [HttpDelete("{ticketId}")]
        public async Task<IActionResult> DeleteTicketAsync([FromRoute] int ticketId, [FromQuery] Guid? token)
        {
            IActionResult response = null;

            try
            {
                if (token == null)
                {
                    throw new SessionNotFoundException("User session token not provided.");
                }

                UserSession userSession = await this.SessionManager.GetSessionAsync(token.Value)
                    .ConfigureAwait(false);

                await this.TicketManager.DeleteTicketAsync(userSession, ticketId)
                    .ConfigureAwait(false);
            }
            catch (UnauthorizedAccessException exc)
            {
                response = this.Forbid(exc.Message);
            }
            catch (AccountNotFoundException)
            {
                response = this.Unauthorized();
            }
            catch (SessionNotFoundException)
            {
                response = this.Unauthorized();
            }
            catch (Exception exc)
            {
                response = new ObjectResult(exc.Message)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            return response;
        }

        // GET: /api/tickets/456?token=F8A25763-016E-4C9D-A15B-5E1C0F50100B
        //
        // Note:
        // If the session is associated with an admin and a specific ticket ID is not provided, then all tickets
        // are returned.
        //
        // If the session is associated with a user and a specific ticket ID is not provided then all tickets for
        // that user are returned.
        [HttpGet("{ticketId}")]
        public async Task<IActionResult> GetTicketAsync([FromRoute] int ticketId, [FromQuery] Guid? token)
        {
            IActionResult response = null;

            try
            {
                if (token == null)
                {
                    throw new SessionNotFoundException("User session token not provided.");
                }

                UserSession session = await this.SessionManager.GetSessionAsync(token.Value)
                    .ConfigureAwait(false);

                IEnumerable<Ticket> tickets = await this.TicketManager.GetTicketsAsync(session, ticketId)
                    .ConfigureAwait(false);

                response = this.Ok(tickets);
            }
            catch (UnauthorizedAccessException exc)
            {
                response = this.Forbid(exc.Message);
            }
            catch (AccountNotFoundException)
            {
                response = this.Unauthorized();
            }
            catch (SessionNotFoundException)
            {
                response = this.Unauthorized();
            }
            catch (Exception exc)
            {
                response = new ObjectResult(exc.Message)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            return response;
        }

        // GET: /api/tickets?token=F8A25763-016E-4C9D-A15B-5E1C0F50100B
        //
        // Note:
        // If the session is associated with an admin and a specific ticket ID is not provided, then all tickets
        // are returned.
        //
        // If the session is associated with a user and a specific ticket ID is not provided then all tickets for
        // that user are returned.
        [HttpGet]
        public async Task<IActionResult> GetTicketsAsync([FromQuery] Guid? token)
        {
            IActionResult response = null;

            try
            {
                if (token == null)
                {
                    throw new SessionNotFoundException("User session token not provided.");
                }

                UserSession session = await this.SessionManager.GetSessionAsync(token.Value)
                    .ConfigureAwait(false);

                IEnumerable<Ticket> tickets = await this.TicketManager.GetTicketsAsync(session)
                    .ConfigureAwait(false);

                response = this.Ok(tickets);
            }
            catch (UnauthorizedAccessException exc)
            {
                response = this.Forbid(exc.Message);
            }
            catch (AccountNotFoundException)
            {
                response = this.Unauthorized();
            }
            catch (SessionNotFoundException)
            {
                response = this.Unauthorized();
            }
            catch (Exception exc)
            {
                response = new ObjectResult(exc.Message)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            return response;
        }

        // PUT: /api/tickets?token=F8A25763-016E-4C9D-A15B-5E1C0F50100B
        //
        // BODY:
        // {
        //   'Id': 12,
        //   'Title': 'Any ticket title',
        //   'Description': 'A description of the help desk request',
        //   'CreatedTime': '2019-07-29T13:45:30.0000000Z',
        //   ...
        // }
        [HttpPut]
        public async Task<IActionResult> UpdateTicketAsync([FromBody] Ticket ticket, [FromQuery] Guid? token)
        {
            IActionResult response = null;

            try
            {
                if (token == null)
                {
                    throw new SessionNotFoundException("User session token not provided.");
                }

                UserSession session = await this.SessionManager.GetSessionAsync(token.Value)
                    .ConfigureAwait(false);

                IEnumerable<Ticket> matchingTickets = await this.TicketManager.GetTicketsAsync(session, ticket.Id)
                    .ConfigureAwait(false);

                if (matchingTickets?.Any() == true)
                {
                    Ticket updatedTicket = await this.TicketManager.UpdateTicketAsync(session, matchingTickets.First())
                        .ConfigureAwait(false);

                    response = this.Ok(updatedTicket);
                }
                else
                {
                    response = this.NotFound();
                }
                
            }
            catch (Exception exc)
            {
                response = new ObjectResult(exc.Message)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            return response;
        }

    }
}