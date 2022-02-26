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
            services.AddResponseCaching();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            services.AddMvc(rydooption => {
                rydooption.EnableEndpointRouting = false; // Need it to use app.UseMvc() middleware.
                AuthorizationPolicy policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build(); // This policy says to add authorise attribute to all the controllers in this project.
                rydooption.Filters.Add(new AuthorizeFilter(policy)); // Adding this policy as a filter
            }).AddXmlSerializerFormatters();

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(_rydoconfig.GetConnectionString("EmployeeDBConnection")));
            // "EmployeeDBConnection": "server=ICEN126\\SQLEXPRESS;database=EmployeeDotCoreIdentity;Trusted_Connection=true;MultipleActiveResultSets=true"

            services.AddIdentity<ApplicationUser, IdentityRole>(rydoconfigureoptions=> {
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

            // Set up Custom Urls for Login Path and AccessDeniedPath
            services.ConfigureApplicationCookie(rydooptions =>
            {
                rydooptions.LoginPath = new PathString ("/RydoTechs/Account/LogIn"); // By Default If you are trying to access an action method with Authorise Attribute on it then the return url is : /Account/LogIn. We might want to override that using below code.
                rydooptions.AccessDeniedPath = new PathString("/RydoTechs/Account/AccessDenied"); // By Default If you are trying to access an action method with Authorise Attribute on it but not fulffing the specified role mentioned with it then the return url is : /Account/AccessDeied. We might want to override that using below code.
            });


            //Claims based authorization in asp net core
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RydoDeleteRoleClaimPolicy", policy => policy.RequireClaim("Delete Role", "true"));
                options.AddPolicy("RydoAdminRolePolicy", rolepolicy => rolepolicy.RequireRole("Admin"));

                options.AddPolicy("RydoEditRoleClaimPolicy",rolepolicy=> rolepolicy.RequireClaim("Edit Role","true"));
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
            app.UseResponseCaching();

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
