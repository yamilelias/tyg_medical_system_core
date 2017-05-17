using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using system_core_with_authentication.Data;
using system_core_with_authentication.Models;
using system_core_with_authentication.Services;
using Microsoft.AspNetCore.Identity;
using Treshold_Mail.Scheduler;
using Hangfire;
using Treshold_Mail.Mail;

namespace system_core_with_authentication
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            /*services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
                */
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            /*services.AddHangfire(options =>
                        options.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));*/

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            

            services.AddTransient<IMail, MailService>();


            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            //app.UseHangfireServer();
            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            context.Database.EnsureCreated();

            //DbInitializer.Initialize(context);

            if(context.Roles.ToList().Count==0)
                await CreateRoles(serviceProvider);
            //HangfireScheduler.Init(app);

        }

        /*
         * THIS METHOD CREATES THE THREE USER ROLES
         * A DEFAULT ADMIN ACCOUNT IS GIVEN THE ADMIN ROLE
         */

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] rolesNames = { "Admin", "Supervisor", "User" };
            IdentityResult result;

            foreach (var rolesName in rolesNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(rolesName);
                if (!roleExist)
                {
                    result = await roleManager.CreateAsync(new IdentityRole(rolesName));
                }
            }

            /*
             * THE DEFAULT ADMIN ACCOUNT
             * WILL BE ASSIGNED THE ADMIN ROLE
             */

            var user = new ApplicationUser
            {
                Id = "1",
                UserName = "asti@asti.com",
                Email = "asti@asti.com"
            };

            var result2 = await userManager.CreateAsync(user, "Asti2017.");
            await userManager.AddToRoleAsync(user, "Admin");

    }

    }
}
