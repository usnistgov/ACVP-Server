using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Web.Admin.Auth;
using Web.Admin.Auth.Models;

namespace Web.Admin
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
            ConfigureAuth(services);
            
            services.AddControllersWithViews()
                .AddJsonOptions(
                    options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });
        }

        private void ConfigureAuth(IServiceCollection services)
        {
            SsoConfig ssoConfig = new SsoConfig();
            Configuration.Bind(nameof(SsoConfig), ssoConfig);

            if (ssoConfig.UseSso)
            {
                services.AddAuthentication(sharedOptions =>
                    {
                        sharedOptions.DefaultScheme = WsFederationDefaults.AuthenticationScheme;
                        sharedOptions.DefaultChallengeScheme = WsFederationDefaults.AuthenticationScheme;
                    })
                    .AddWsFederation(options =>
                    {
                        options.Wtrealm = ssoConfig.WtRealm;
                        options.MetadataAddress = ssoConfig.AdfsMetadata;
                    });
                    // .AddCookie(options =>
                    // {
                    //     options.Cookie.HttpOnly = true;
                    //     options.Cookie.SameSite = SameSiteMode.Strict;
                    //     options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    //     options.Cookie.IsEssential = true;
                    //     options.Events.OnValidatePrincipal = PrincipalValidator.ValidateAsync;
                    // });
            }
            else
            {
                var mockAuth = "MockAuthentication";
                services.AddAuthentication(mockAuth)
                    .AddScheme<AuthenticationSchemeOptions, MockAuthenticatedUser>(mockAuth, null);
                // .AddCookie(options =>
                // {
                //     options.Cookie.HttpOnly = false;
                //     options.Cookie.SameSite = SameSiteMode.Lax;
                //     options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                //     options.Cookie.IsEssential = true;
                //     options.Events.OnValidatePrincipal = PrincipalValidator.ValidateAsync;
                // });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsEnvironment(NIST.CVP.Common.Enums.Environments.Local.ToString()))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            if (!env.IsEnvironment(NIST.CVP.Common.Enums.Environments.Local.ToString()))
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                // Map controller endpoints and make them require authorization by default.
                // Anonymous controllers are still possible by decorating with attribute [AllowAnonymous]
                endpoints.MapControllers().RequireAuthorization();
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsEnvironment(NIST.CVP.Common.Enums.Environments.Local.ToString()))
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}