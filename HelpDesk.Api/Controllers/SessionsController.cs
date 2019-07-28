using System;
using System.Threading.Tasks;
using HelpDesk.Api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelpDesk.Api.Controllers
{
    [Route("api/sessions")]
    public class SessionsController : Controller
    {
        public SessionsController(IDataStore dataStore)
        {
            this.DataStore = dataStore
                ?? throw new ArgumentException("The data store parameter is required.", nameof(dataStore));
        }

        protected IDataStore DataStore { get; }

        // POST: /api/sessions
        public async Task<IActionResult> LoginAsync([FromBody] UserLogin loginInfo)
        {
            IActionResult response = null;

            try
            {
                // Get the user account from the data store.
                // - If an account exists, then validate the password for the account.
                //   o password matches, return OK w/session object
                //   o password does not match return Permission Denied

                UserSession session = await this.DataStore.LoginAsync(loginInfo)
                    .ConfigureAwait(false);

                response = this.Ok(session);
            }
            catch (AccountAuthorizationException)
            {
                response = this.Unauthorized();
            }
            catch (AccountNotFoundException exc)
            {
                response = this.NotFound(exc.Message);
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

        // DELETE: /api/sessions/{sessionId}
        [HttpDelete("{sessionId}")]
        public async Task<IActionResult> LogoutAsync([FromRoute] int sessionId)
        {
            IActionResult response = null;

            try
            {
                // Get the user account from the data store.
                // - If an account exists, then validate the password for the account.
                //   o password matches, return OK w/session object
                //   o password does not match return Permission Denied

                await this.DataStore.LogoutAsync(sessionId).ConfigureAwait(false);
                response = this.Ok();
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
