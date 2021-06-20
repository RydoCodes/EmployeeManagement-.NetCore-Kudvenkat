using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));

            //Start - SetupMVC
            services.AddMvc(o => o.EnableEndpointRouting = false).AddXmlSerializerFormatters();
            //services.AddMvcCore(options => options.EnableEndpointRouting = false);

            services.AddSingleton<IEmployeeRepository,MockEmployeeRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        
        // -----------------------------------wHY tAG HELPERS--------------------
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseMvc(route =>
            {
                route.MapRoute("default", "RydoTechs/{Controller=Home}/{Action=Index}/{id?}");
            });
        }

        //---------------------------------------------------------------------------- after video of Attribute Routing
        //public void Configure(IApplicationBuilder app,IWebHostEnvironment env)
        //{
        //    if(env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();
        //    }
        //    app.UseStaticFiles();
        //    app.UseMvc(); // If you want to go for attribute routing and ignore conventional routing then remove conventional routing code and add this piece just to add a mv middleware
        //}


        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILogger<Startup> logger)
        //{

        //    if (env.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();
        //    }

        //    // Setting up MVC with custom default routes and routing rules
        //    app.UseMvc(rydoroute=> {
        //        //rydoroute.MapRoute("default", "{Controller}/{Action}/{id?}"); // {Id} is mandtory and {id?} is optional and no default controller and action method is set.
        //        rydoroute.MapRoute("default", "{Controller=Home}/{Action=Index}/{id?}");
        //    });

        //    //app.UseMvcWithDefaultRoute();

        //    //app.UseFileServer();

        //    //app.Run(async (context) =>
        //    //{
        //    //    await context.Response.WriteAsync("Hello World");
        //    //});

        //    //Example Start : -----------   https://www.youtube.com/watch?v=x8jNX1nb_og&list=PL6n9fhu94yhVkdrusLaQsfERmL_Jh4XmU&index=14

        //    //if (env.IsDevelopment())
        //    //{
        //    //    app.UseDeveloperExceptionPage();
        //    //}

        //    //app.UseStaticFiles();

        //    //app.Run(async (context) =>
        //    //{
        //    //    await context.Response.WriteAsync("Hosting Environmentis : " + env.EnvironmentName);
        //    //});

        //    //Example End
        //    // Example Start------- : https://www.youtube.com/watch?v=UGG2-oV9iQ8&list=PL6n9fhu94yhVkdrusLaQsfERmL_Jh4XmU&index=13

        //    //if (env.IsDevelopment())
        //    //{ 
        //    //    app.UseDeveloperExceptionPage();
        //    //}

        //    //app.UseFileServer();

        //    //app.Run(async (context) =>
        //    //    {
        //    //        throw new Exception("Error man");
        //    //        await context.Response.WriteAsync("I am the result");
        //    //    });
        //    // Example End------- 
        //    // Example Start------- : https://www.youtube.com/watch?v=yt6bzZoovgM&list=PL6n9fhu94yhVkdrusLaQsfERmL_Jh4XmU&index=12

        //    //FileServerOptions options = new FileServerOptions();
        //    //options.DefaultFilesOptions.DefaultFileNames.Clear();
        //    //options.DefaultFilesOptions.DefaultFileNames.Add("foo.html");

        //    //app.UseFileServer(options);

        //    //app.Run(async (context) =>
        //    //{
        //    //    await context.Response.WriteAsync("I am the result");
        //    //});

        //    // Example End-------

        //    // Example Start-------
        //    //DefaultFilesOptions options = new DefaultFilesOptions();
        //    //options.DefaultFileNames.Clear();

        //    //options.DefaultFileNames.Add("foo.html");

        //    //app.UseDefaultFiles(options);
        //    //app.UseStaticFiles();

        //    //app.Run(async (context) =>
        //    //{
        //    //    await context.Response
        //    //    .WriteAsync("Hello World");
        //    //});

        //    // Example End-------

        //    // Example Start-------
        //    //app.UseDefaultFiles();
        //    //app.UseStaticFiles();

        //    //app.Run(async (context) =>
        //    //{
        //    //    await context.Response
        //    //    .WriteAsync("Hello World");
        //    //});

        //    // Example End-------


        //    // Example Start----------- : https://www.youtube.com/watch?v=nt6anXAwfYI&list=PL6n9fhu94yhVkdrusLaQsfERmL_Jh4XmU&index=11

        //    //app.Use(async (context, next) => {
        //    //    logger.LogInformation("MW1: Incoming Request \n");
        //    //    await next();
        //    //    logger.LogInformation("MW1: Outgoing Response \n");
        //    //});

        //    //app.Use(async (context, next) => {
        //    //    logger.LogInformation("MW2: Incoming Request \n");
        //    //    await next();
        //    //    logger.LogInformation("MW2: Outgoing Response \n");
        //    //});

        //    //app.Run(async (context) => {
        //    //    await context.Response.WriteAsync("MW3: Request handled and response produced \n");
        //    //    logger.LogInformation("MW3: Request handled and response produced ]n");
        //    //});

        //    // Example End-------------------

        //    // - ----------------Example----------------------------------------------

        //    //app.Use(async (context, next) => {
        //    //    await context.Response.WriteAsync("Hello from 1st Middleware \n");
        //    //    await next();
        //    //    await context.Response.WriteAsync("Back to First Middleware \n");
        //    //});

        //    //app.Run(async (context) => {
        //    //    await context.Response.WriteAsync("Hello from 2nd Middleware \n");
        //    //});
        //    //-----------------Example---------------------------------------------------------------

        //    //app.UseRouting();

        //    //app.Run(async (context) =>
        //    //{
        //    //    await context.Response
        //    //    //.WriteAsync(System.Diagnostics.Process.GetCurrentProcess().ProcessName);
        //    //    //.WriteAsync(_config["RydoKey"]);
        //    //    .WriteAsync("Hello World");
        //    //});

        //    // To use below endpoint, you need to uncomment app.useRouting() Middleware.

        //    //app.UseEndpoints(endpoints =>
        //    //{
        //    //    endpoints.MapGet("/", async context =>
        //    //    {
        //    //        await context.Response
        //    //        .WriteAsync(System.Diagnostics.Process.GetCurrentProcess().ProcessName);
        //    //        //.WriteAsync(_config["RydoKey"]);
        //    //    });
        //    //});
        //}
    }
}
