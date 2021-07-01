using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementUsingIdentity.Models;
using EmployeeManagementUsingIdentity.Models.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
              app.UseExceptionHandler("/ErrorController");
               app.UseStatusCodePagesWithReExecute("/ErrorController/{0}");
            }

            app.UseStaticFiles();

            app.UseMvc(rydoroute =>
            {
                rydoroute.MapRoute("default", "RydoTechs/{Controller=Home}/{Action=Index}/{id?}"); // default route is always represented by keyword "default"
            });

        }
    }
}
