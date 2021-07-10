using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementUsingIdentity.Models;
using EmployeeManagementUsingIdentity.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
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
            services.AddMvc(rydooption => {
                rydooption.EnableEndpointRouting = false; // Need it to use app.UseMvc() middleware.
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build(); // This policy says to add authorise attribute to all the controllers in this project.
                rydooption.Filters.Add(new AuthorizeFilter(policy)); // Adding this policy as a filter
            }).AddXmlSerializerFormatters();

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(_rydoconfig.GetConnectionString("EmployeeDBConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>(rydoconfigureoptions=> {
                rydoconfigureoptions.Password.RequiredLength = 2;
                rydoconfigureoptions.Password.RequiredUniqueChars = 0;
                rydoconfigureoptions.Password.RequireUppercase = false;
                rydoconfigureoptions.Password.RequireNonAlphanumeric = false;
            }) // Add Identity services to the App. 
            .AddEntityFrameworkStores<AppDbContext>(); // Using Entity Framework core to retrieve user and role information from the underlying sql servr databas using EF Core.

            //services.Configure<IdentityOptions>(rydoconfigureoptions =>
            //{
            //    rydoconfigureoptions.Password.RequiredLength = 10;
            //    rydoconfigureoptions.Password.RequiredUniqueChars = 3;
            //});

            services.ConfigureApplicationCookie(rydooptions => rydooptions.LoginPath = "/RydoTechs/Account/LogIn");
            // By Default If you are trying to access an action method with Authorise Attribute on it then the return url is : /Account/LogIn. We might want to override that using below code.
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

           // app.UseAuthorization();

            app.UseMvc(rydoroute =>
            {
                rydoroute.MapRoute("default", "RydoTechs/{Controller=Home}/{Action=Index}/{id?}"); // default route is always represented by keyword "default"
            });

        }
    }
}

// 
