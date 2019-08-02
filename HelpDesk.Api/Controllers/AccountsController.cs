using System;
using System.Threading.Tasks;
using HelpDesk.Api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelpDesk.Api.Controllers
{
    [Route("api/accounts")]
    public class AccountsController : Controller
    {
        public AccountsController(ISessionManager sessionManager)
        {
            this.SessionManager = sessionManager
                ?? throw new ArgumentException("The session manager parameter is required.", nameof(sessionManager));
        }

        protected ISessionManager SessionManager { get; }

        // POST: /api/accounts
        //
        // BODY:
        // {
        //   'UserName': 'AnyUser',
        //   'Password': 'Pwd'
        // }
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> CreateAccountAsync([FromBody] UserLogin loginInfo)
        {
            IActionResult response = null;

            try
            {
                // Get the user account from the data store.
                // - If an account exists, then validate the password for the account.
                //   o password matches, return OK w/session object
                //   o password does not match return Permission Denied

                UserAccount account = await this.SessionManager.CreateAccountAsync(loginInfo, "User")
                    .ConfigureAwait(false);

                // Once an account exists, the user is logged in automatically.
                UserSession session = await this.SessionManager.LoginAsync(loginInfo)
                    .ConfigureAwait(false);

                response = this.Ok(session);
            }
            catch (Exception exc)
            {
                response = this.InternalServerError(exc.Message);
            }

            return response;
        }

        // GET: /api/accounts/{username}
        [Produces("application/json")]
        [HttpGet("{username}")]
        public async Task<IActionResult> GetAccountAsync(string username)
        {
            IActionResult response = null;

            try
            {
                UserAccount account = await this.SessionManager.GetAccountAsync(username)
                    .ConfigureAwait(false);

                // Don't share the password with the client-side
                response = this.Ok(new UserAccount(account.Id, account.Username, "secret", account.Role));
            }
            catch (AccountNotFoundException exc)
            {
                response = this.BadRequest(exc.Message);
            }
            catch (Exception exc)
            {
                response = this.InternalServerError(exc.Message);
            }

            return response;
        }

        // DELETE: /api/accounts
        //
        // BODY:
        // {
        //   'UserName': 'AnyUser',
        //   'Password': 'Pwd'
        // }
        [HttpDelete]
        public async Task<IActionResult> DeleteAccountAsync([FromBody] UserLogin loginInfo)
        {
            IActionResult response = null;

            try
            {
                // Remove the user's existing session since there will no longer be an account associated
                // with the user.
                UserSession existingSession = await this.SessionManager.GetSessionAsync(loginInfo.Username)
                    .ConfigureAwait(false);

                await this.SessionManager.LogoutAsync(existingSession.Id)
                    .ConfigureAwait(false);

                await this.SessionManager.DeleteAccountAsync(loginInfo).ConfigureAwait(false);
                response = this.Ok();
            }
            catch (SessionNotFoundException exc)
            {
                // User session does not exist
                response = this.NotFound(exc.Message);
            }
            catch (AccountNotFoundException exc)
            {
                // User account does not exist
                response = this.NotFound(exc.Message);
            }
            catch (AccountAuthorizationException exc)
            {
                // User password does not match
                response = this.Forbid(exc.Message);
            }
            catch (Exception exc)
            {
                response = this.InternalServerError(exc.Message);
            }

            return response;
        }

        // PUT: /api/accounts
        //
        // BODY:
        // {
        //   'UserName': 'AnyUser',
        //   'Password': 'OriginalPwd',
        //   'NewPassword': 'NewPwd'
        // }
        [Produces("application/json")]
        [HttpPut]
        public async Task<IActionResult> UpdateAccountAsync([FromBody] UserLogin loginInfo)
        {
            IActionResult response = null;

            try
            {
                // Ensure user session exists
                await this.SessionManager.GetSessionAsync(loginInfo.Username)
                    .ConfigureAwait(false);

                UserAccount account = await this.SessionManager.UpdateAccountAsync(loginInfo, "User")
                    .ConfigureAwait(false);

                // Once an account exists, the user is logged in automatically.
                UserSession session = await this.SessionManager.LoginAsync(loginInfo)
                    .ConfigureAwait(false);

                response = this.Ok(session);
            }
            catch (SessionNotFoundException exc)
            {
                response = this.NotFound(exc.Message);
            }
            catch (AccountNotFoundException exc)
            {
                // User account does not exist
                response = this.NotFound(exc.Message);
            }
            catch (AccountAuthorizationException exc)
            {
                // User password does not match
                response = this.Forbid(exc.Message);
            }
            catch (Exception exc)
            {
                response = this.InternalServerError(exc.Message);
            }

            return response;
        }
    }
}
