﻿using System;
using System.Threading.Tasks;
using HelpDesk.Api.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelpDesk.Api.Controllers
{
    //
    // Note:
    // We are mocking out the use of user authorization tokens in this project.  This would not be a secure model
    // in the real world.  We would want to implement a proper identity server to handle JWT.
    // 
    // Example:
    // https://developer.okta.com/blog/2018/03/23/token-authentication-aspnetcore-complete-guide
    // http://hamidmosalla.com/2017/12/07/policy-based-authorization-using-asp-net-core-2-identityserver4/

    [Route("api/sessions")]
    public class SessionsController : Controller
    {
        public SessionsController(ISessionManager dataStore)
        {
            this.DataStore = dataStore
                ?? throw new ArgumentException("The data store parameter is required.", nameof(dataStore));
        }

        protected ISessionManager DataStore { get; }

        // GET: /api/sessions/123
        [Produces("application/json")]
        [HttpGet("{sessionId}")]
        public async Task<IActionResult> GetSessionAsync([FromRoute] int sessionId)
        {
            IActionResult response = null;

            try
            {
                // For the sake of development, allow cross-origin responses.
                // this.Response.AddCrossOriginHeaders();

                UserSession session = await this.DataStore.GetSessionAsync(sessionId)
                    .ConfigureAwait(false);

                response = this.Ok(session);
            }
            catch (AccountAuthorizationException)
            {
                response = this.UserLoginInvalid();
            }
            catch (AccountNotFoundException exc)
            {
                response = this.NotFound(exc.Message);
            }
            catch (SessionNotFoundException exc)
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


        // POST: /api/sessions
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] UserLogin loginInfo)
        {
            IActionResult response = null;

            try
            {
                //this.Response.AddCorsHeaders();
                // For the sake of development, allow cross-origin responses.
                // this.Response.AddCrossOriginHeaders();

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
                response = this.UserLoginInvalid();
            }
            catch (AccountNotFoundException exc)
            {
                response = this.NotFound(exc.Message);
            }
            catch (Exception exc)
            {
                response = this.InternalServerError(exc.Message);
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
                // For the sake of development, allow cross-origin responses.
                // this.Response.AddCrossOriginHeaders();

                // Get the user account from the data store.
                // - If an account exists, then validate the password for the account.
                //   o password matches, return OK w/session object
                //   o password does not match return Permission Denied

                await this.DataStore.LogoutAsync(sessionId).ConfigureAwait(false);
                response = this.Ok();
            }
            catch (AccountAuthorizationException)
            {
                response = this.UserLoginInvalid();
            }
            catch (AccountNotFoundException exc)
            {
                response = this.NotFound(exc.Message);
            }
            catch (SessionNotFoundException exc)
            {
                response = this.NotFound(exc.Message);
            }
            catch (Exception exc)
            {
                response = this.InternalServerError(exc.Message);
            }

            return response;
        }
    }
}
