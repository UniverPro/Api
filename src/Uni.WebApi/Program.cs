using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Uni.Infrastructure;

namespace Uni.WebApi
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Initializer.Initialize();
            CreateWebHostBuilder(args).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                // Needed for using scoped services (for example DbContext) in validators
                .UseDefaultServiceProvider(options => options.ValidateScopes = false)
                .UseStartup<Startup>();
        }
    }
}
