using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
<<<<<<< HEAD
using TYG_Medical_System_Core.DAL   ;
=======
using TYG_Medical_System_Core.DAL;
>>>>>>> InventoryCRUD
using Microsoft.EntityFrameworkCore;

namespace TYG_Medical_System_Core
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
<<<<<<< HEAD
            services.AddDbContext<DBContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

=======
            services.AddDbContext<InventoryContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
>>>>>>> InventoryCRUD
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
<<<<<<< HEAD
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, DBContext context)
=======
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, InventoryContext context)
>>>>>>> InventoryCRUD
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
<<<<<<< HEAD

            DBInitializer.Initialize(context);
=======
            context.Database.EnsureCreated();
>>>>>>> InventoryCRUD
        }
    }
}
