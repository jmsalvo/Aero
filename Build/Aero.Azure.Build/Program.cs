using System;
using Cake.Frosting;

namespace Aero.Build
{
    class Program : IFrostingStartup
    {
        public static int Main(string[] args)
        {
            // Create the host.
            var host = new CakeHostBuilder()
                .WithArguments(args)
                .UseStartup<Program>()
                .Build();

            // Run the host.
            var exitCode = host.Run();

            if (Convert.ToBoolean(Environment.GetEnvironmentVariable("IsVisualStudio")))
            {
                Console.WriteLine("Press enter (maybe twice) to exit");
                Console.Read();
            }

            return exitCode;
        }

        public void Configure(ICakeServices services)
        {
            services.UseContext<Context>();
            services.UseLifetime<Lifetime>();
            services.UseWorkingDirectory("..");
        }
    }
}
