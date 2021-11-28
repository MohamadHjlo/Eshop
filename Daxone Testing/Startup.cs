using System;
using System.Net;
using System.Security.Claims;
using Daxone_Testing.Controllers;
using Daxone_Testing.Data;
using Daxone_Testing.Data.Repositories;
using Daxone_Testing.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using PersianTranslation.Identity;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;


namespace Daxone_Testing
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddIdentity<ApplicationUser, ApplicationRole>(option =>
                {
                    option.Password.RequireUppercase=false;
                    option.Password.RequireDigit = false;
                    option.Password.RequireLowercase = false;
                    option.Password.RequireNonAlphanumeric = false;
                    option.Password.RequiredUniqueChars = 0;
                    option.User.RequireUniqueEmail = true;

                })
                .AddEntityFrameworkStores<DaxoneTestingContext>()
                .AddDefaultTokenProviders().AddErrorDescriber<PersianIdentityErrorDescriber>();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.Secure = CookieSecurePolicy.Always;
            });

            #region Db Context
            services.AddDbContext<DaxoneTestingContext>(option =>
            {
                option.UseSqlServer(@"Data Source=EBRAHIMH;Initial Catalog=HajMarjet_DB;Integrated Security=true");
            });

            #endregion

            #region Ioc //inversion of controll
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMessageSender, MessageSender>();

            #endregion


            #region Authentication
            services.ConfigureApplicationCookie(option =>
        {
            option.ExpireTimeSpan = TimeSpan.FromDays(3);
        });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddGoogle(options =>
            {
                options.ClientId = "435340451917-07dpaq9336vue4n72d9kjk2k7gqga4vh.apps.googleusercontent.com";
                options.ClientSecret = "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";
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                {
                    option.LoginPath = "/Account/Login/";
                    option.LogoutPath = "/Account/Logout";
                    option.ExpireTimeSpan = TimeSpan.FromDays(5);//چن روز کاربر لاگین بمونه
                });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthentication();//enable authentication system
            app.UseAuthorization();//controll Access level of users

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }


    }
}
