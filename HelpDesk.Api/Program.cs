using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace HelpDesk.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseKestrel((hostContext, options) =>
            {
                options.Limits.MaxConcurrentConnections = 100;
                options.Limits.MaxConcurrentUpgradedConnections = 100;

                // Set the host HTTP listener options.  This is where you set whether the API service
                // listens to HTTP or HTTPS (or both) calls. If HTTPS, then certificates are typically
                // required.
                options.Listen(IPAddress.Any, 5000);
                // options.Listen(IPAddress.Any, 5001, listenOptions => listenOptions.UseHttps()
            })
            .UseStartup<Startup>();
    }
}
