using System.Collections.Generic;
using HelpDesk.Api.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HelpDesk.Api
{
    public class Startup
    {
        public const string LocalDevelopmentOrigins = nameof(LocalDevelopmentOrigins);

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

            
            services.AddSingleton<ISessionManager>(sessionManager);
            services.AddSingleton<ITicketManager>(ticketManager);

            // Allow Cross Origin Resource Sharing (CORS) when running everything
            // on the local dev environment (i.e. localhost).
            services.AddCors(options =>
            {
                options.AddPolicy(Startup.LocalDevelopmentOrigins, builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .Build();
                });
            });

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        options.Authority = "{yourAuthorizationServerAddress}";
            //        options.Audience = "{yourAudience}";
            //    });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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

            // Allow Cross Origin Resource Sharing (CORS) when running everything
            // on the local dev environment (i.e. localhost).
            app.UseCors(Startup.LocalDevelopmentOrigins);

            // app.UseAuthentication();
            app.UseHttpsRedirection();          
            app.UseMvc();
        }

        private void InitializeMockData(InMemorySessionManager sessionManager, InMemoryTicketManager ticketManager)
        {
            UserLogin user1 = new UserLogin
            {
                Username = "user1",
                Password = "secret"
            };

            UserLogin user2 = new UserLogin
            {
                Username = "user2",
                Password = "secret"
            };

            UserLogin admin = new UserLogin
            {
                Username = "admin",
                Password = "secret"
            };

            sessionManager.CreateAccountAsync(admin, UserRole.Admin)
                .GetAwaiter().GetResult();

            sessionManager.CreateAccountAsync(user1, UserRole.User)
                .GetAwaiter().GetResult();

            sessionManager.CreateAccountAsync(user2, UserRole.User)
                .GetAwaiter().GetResult();

            UserSession user1Session = sessionManager.LoginAsync(user1)
                .GetAwaiter().GetResult();

            UserSession user2Session = sessionManager.LoginAsync(user2)
                .GetAwaiter().GetResult();

            for (int i = 1; i <= 3; i++)
            {
                Ticket fakeTicket = new Ticket
                {
                    Title = $"{user1.Username}: This is ticket #{i}",
                    Description = $"A description of ticket #{i}"
                };

                ticketManager.CreateTicketAsync(user1Session, fakeTicket)
                    .GetAwaiter().GetResult();
            }

            for (int i = 4; i <= 5; i++)
            {
                Ticket fakeTicket = new Ticket
                {
                    Title = $"{user2.Username}: This is ticket #{i}",
                    Description = $"A description of ticket #{i}"
                };

                ticketManager.CreateTicketAsync(user2Session, fakeTicket)
                    .GetAwaiter().GetResult();
            }

            sessionManager.LogoutAsync(user1Session.Id)
                .GetAwaiter().GetResult();

            sessionManager.LogoutAsync(user2Session.Id)
                .GetAwaiter().GetResult();
        }
    }
}
