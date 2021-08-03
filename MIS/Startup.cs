using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MIS
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
            services.AddDbContext<Context>(options => options.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("Context")));



            

            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                CookieAuthenticationDefaults.AuthenticationScheme);
                defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
                options.AddPolicy("/panel/roles/create", policy => policy.RequireClaim("/panel/roles/create", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/roles", policy => policy.RequireClaim("/panel/roles", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/roles/edit", policy => policy.RequireClaim("/panel/roles/edit", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/roles/delete", policy => policy.RequireClaim("/panel/roles/delete", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/users/create", policy => policy.RequireClaim("/panel/users/create", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/users", policy => policy.RequireClaim("/panel/users", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/users/edit", policy => policy.RequireClaim("/panel/users/edit", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/users/delete", policy => policy.RequireClaim("/panel/users/delete", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/samanes/create", policy => policy.RequireClaim("/panel/samanes/create", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/samanes", policy => policy.RequireClaim("/panel/samanes", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/samanes/edit", policy => policy.RequireClaim("/panel/samanes/edit", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/samanes/delete", policy => policy.RequireClaim("/panel/samanes/delete", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/actiongroups", policy => policy.RequireClaim("/panel/actiongroups", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/actiongroups/create", policy => policy.RequireClaim("/panel/actiongroups/create", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/actiongroups/edit", policy => policy.RequireClaim("/panel/actiongroups/edit", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/actiongroups/delete", policy => policy.RequireClaim("/panel/actiongroups/delete", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/actions", policy => policy.RequireClaim("/panel/actions", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/actions/create", policy => policy.RequireClaim("/panel/actions/create", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/actions/edit", policy => policy.RequireClaim("/panel/actions/edit", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/actions/delete", policy => policy.RequireClaim("/panel/actions/delete", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/raccs", policy => policy.RequireClaim("/panel/raccs", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/uaccs", policy => policy.RequireClaim("/panel/uaccs", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/domains", policy => policy.RequireClaim("/panel/domains", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/domains/create", policy => policy.RequireClaim("/panel/domains/create", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/domains/edit", policy => policy.RequireClaim("/panel/domains/edit", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/domains/delete", policy => policy.RequireClaim("/panel/domains/delete", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/domainvalues", policy => policy.RequireClaim("/panel/domainvalues", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/domainvalues/create", policy => policy.RequireClaim("/panel/domainvalues/create", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/domainvalues/edit", policy => policy.RequireClaim("/panel/domainvalues/edit", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/domainvalues/delete", policy => policy.RequireClaim("/panel/domainvalues/delete", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/uaccdomains", policy => policy.RequireClaim("/panel/uaccdomains", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/raccdomains", policy => policy.RequireClaim("/panel/raccdomains", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/forms", policy => policy.RequireClaim("/panel/forms", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/userforms", policy => policy.RequireClaim("/panel/userforms", new string[] { "ok" }).Build());
                options.AddPolicy("/panel/forms/fillindex", policy => policy.RequireClaim("/panel/forms/fillindex", new string[] { "ok" }).Build());
            });
            
            services.AddMvc()
            .AddRazorOptions(options =>
            {
                options.ViewLocationFormats.Add("/Views/Shared/Components/{0}/Default.cshtml");
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddControllersWithViews();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "cookie";
                    options.LoginPath = "/";
                    options.LogoutPath = "/Logout";
                    options.AccessDeniedPath = "/Home/Error";
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            app.UseDeveloperExceptionPage();
            app.UseHsts();
            //if (env.IsDevelopment())
            //{

            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}
            //app.UseCors(policy =>
            //{
            //    policy.AllowAnyHeader();
            //    policy.AllowAnyMethod();
            //    policy.AllowAnyOrigin();
            //    policy.AllowCredentials();
            //});

            app.UseAuthentication();
            app.UseRouting();
            
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapControllerRoute(
                        name: "Areas",
                        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
