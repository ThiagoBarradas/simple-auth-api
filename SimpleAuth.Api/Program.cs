using Microsoft.AspNetCore.Hosting;
using SimpleAuth.Api.Utilities;
using System;
using System.IO;

namespace SimpleAuth.Api
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(ApplicationUtility.GetApplicationTitle());
            
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://*:506")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
