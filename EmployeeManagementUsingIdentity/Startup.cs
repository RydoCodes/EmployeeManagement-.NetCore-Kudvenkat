using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementUsingIdentity.Models;
using EmployeeManagementUsingIdentity.Models.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmployeeManagementUsingIdentity
{
    public class Startup
    {
        private IConfiguration _rydoconfig;

        public Startup(IConfiguration _rydoconfig)
        {
            this._rydoconfig = _rydoconfig;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddMvc(rydooption => rydooption.EnableEndpointRouting = false).AddXmlSerializerFormatters();

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(_rydoconfig.GetConnectionString("EmployeeDBConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>(rydoconfigureoptions=> {
                rydoconfigureoptions.Password.RequiredLength = 10;
                rydoconfigureoptions.Password.RequiredUniqueChars = 3;
            }) // Add Identity services to the App. 
            .AddEntityFrameworkStores<AppDbContext>(); // Using Entity Framework core to retrieve user and role information from the underlying sql servr databas using EF Core.

            services.Configure<IdentityOptions>(rydoconfigureoptions =>
            {
                rydoconfigureoptions.Password.RequiredLength = 10;
                rydoconfigureoptions.Password.RequiredUniqueChars = 3;
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
              app.UseExceptionHandler("/Error");
               app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            app.UseStaticFiles();
            app.UseAuthentication(); // We want to authenticate users before the request reached the useMVC Middlewares

            app.UseMvc(rydoroute =>
            {
                rydoroute.MapRoute("default", "RydoTechs/{Controller=Home}/{Action=Index}/{id?}"); // default route is always represented by keyword "default"
            });

        }
    }
}
