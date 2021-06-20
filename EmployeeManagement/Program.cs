using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHostBuilder h = CreateHostBuilder(args);
            IHost h2 = h.Build();
            h2.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            Action<IWebHostBuilder> d1 = new Action<IWebHostBuilder>(Process);

            IHostBuilder h1 = Host.CreateDefaultBuilder(args); // CreateDefaultBuilder - Creates the web host with preconfigured defaults.
            IHostBuilder h2 = h1.ConfigureWebHostDefaults(d1);
            return h2;
        }



        public static void Process(IWebHostBuilder iwb)
        {
            iwb.UseStartup<Startup>();
        }
    }
}
