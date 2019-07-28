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
        public AccountsController(IDataStore dataStore)
        {
            this.DataStore = dataStore
                ?? throw new ArgumentException("The data store parameter is required.", nameof(dataStore));
        }

        protected IDataStore DataStore { get; }

        // POST: /api/accounts
        //
        // BODY:
        // {
        //   'UserName': 'AnyUser',
        //   'Password': 'Pwd'
        // }
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

                UserAccount account = await this.DataStore.CreateAccountAsync(loginInfo, "User")
                    .ConfigureAwait(false);

                // Once an account exists, the user is logged in automatically.
                UserSession session = await this.DataStore.LoginAsync(loginInfo)
                    .ConfigureAwait(false);

                response = this.Ok(session);
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

        // GET: /api/accounts/{username}
        [HttpGet("{username}")]
        public async Task<IActionResult> GetAccountAsync(string username)
        {
            IActionResult response = null;

            try
            {
                UserAccount account = await this.DataStore.GetAccountAsync(username)
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
                response = new ObjectResult(exc.Message)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
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
                UserSession existingSession = await this.DataStore.GetSessionAsync(loginInfo.Username)
                    .ConfigureAwait(false);

                await this.DataStore.LogoutAsync(existingSession.Id)
                    .ConfigureAwait(false);

                await this.DataStore.DeleteAccountAsync(loginInfo).ConfigureAwait(false);
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
            catch (AccountAuthorizationException)
            {
                // User password does not match
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

        // PUT: /api/accounts
        //
        // BODY:
        // {
        //   'UserName': 'AnyUser',
        //   'Password': 'OriginalPwd',
        //   'NewPassword': 'NewPwd'
        // }
        [HttpPut]
        public async Task<IActionResult> UpdateAccountAsync([FromBody] UserLogin loginInfo)
        {
            IActionResult response = null;

            try
            {
                // Ensure user session exists
                await this.DataStore.GetSessionAsync(loginInfo.Username)
                    .ConfigureAwait(false);

                UserAccount account = await this.DataStore.UpdateAccountAsync(loginInfo, "User")
                    .ConfigureAwait(false);

                // Once an account exists, the user is logged in automatically.
                UserSession session = await this.DataStore.LoginAsync(loginInfo)
                    .ConfigureAwait(false);

                response = this.Ok(session);
            }
            catch (SessionNotFoundException exc)
            {
                this.NotFound(exc.Message);
            }
            catch (AccountNotFoundException exc)
            {
                // User account does not exist
                response = this.NotFound(exc.Message);
            }
            catch (AccountAuthorizationException)
            {
                // User password does not match
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
    }
}
