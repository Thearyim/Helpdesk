using System.Collections.Generic;
using HelpDesk.Api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HelpDesk.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {       
            InMemorySessionManager sessionManager = new InMemorySessionManager();
            InMemoryTicketManager ticketManager = new InMemoryTicketManager();
            this.InitializeMockData(sessionManager, ticketManager);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<ISessionManager>(sessionManager);
            services.AddSingleton<ITicketManager>(ticketManager);

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        options.Authority = "{yourAuthorizationServerAddress}";
            //        options.Audience = "{yourAudience}";
            //    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            // app.UseAuthentication();
            app.UseMvc();
        }

        private void InitializeMockData(InMemorySessionManager sessionManager, InMemoryTicketManager ticketManager)
        {
            UserLogin fakeUser = new UserLogin
            {
                Username = "user@codingchallenge.com",
                Password = "secret"
            };

            sessionManager.CreateAccountAsync(fakeUser, UserRole.Admin)
                .GetAwaiter().GetResult();

            UserSession fakeUserSession = sessionManager.LoginAsync(fakeUser)
                .GetAwaiter().GetResult();

            List<Ticket> fakeTickets = new List<Ticket>()
            {
                new Ticket
                {
                    Title = "This is ticket #1",
                    Description = "A description of ticket #1"
                }
            };

            foreach (Ticket ticket in fakeTickets)
            {
                ticketManager.CreateTicketAsync(fakeUserSession, ticket)
                    .GetAwaiter().GetResult();
            }
        }
    }
}
