using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Web.Admin.Auth;
using Web.Admin.Auth.Models;

namespace Web.Admin
{
    public class Startup
    {
        private const string AdfsCorsPolicy = "AdfsCorsPolicy";
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(AdfsCorsPolicy,
                    builder =>
                    {
                        builder
                            //.WithOrigins("https://sts.nist.gov")
                            .AllowAnyOrigin();
                    });
            });
            
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
                Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
                services.AddAuthentication(sharedOptions =>
                    {
                        sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        sharedOptions.DefaultChallengeScheme = WsFederationDefaults.AuthenticationScheme;
                    })
                    .AddWsFederation(options =>
                    {
                        options.Wtrealm = ssoConfig.WtRealm;
                        options.Wreply = ssoConfig.WReply;
                        options.MetadataAddress = ssoConfig.AdfsMetadata;
                        options.Events.OnAuthenticationFailed += context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Startup>>();
                            logger.LogError("Failed auth.");
                            logger.LogError($"Context Exception {context.Exception}");
                            logger.LogError($"Context Exception {context.ProtocolMessage}");
                            
                            return Task.CompletedTask;
                        }; 
                        options.Events.OnSecurityTokenValidated += PrincipalValidator.ValidateAsync;
                    })
                    .AddCookie();
            }
            else
            {
                var mockAuth = "MockAuthentication";
                services.AddAuthentication(options =>
                    {
                        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultAuthenticateScheme = mockAuth;
                    })
                    .AddScheme<AuthenticationSchemeOptions, MockAuthenticatedUser>(mockAuth, null)
                    .AddCookie();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsEnvironment(NIST.CVP.Common.Enums.Environments.Local.ToString()) 
            || env.IsEnvironment(NIST.CVP.Common.Enums.Environments.Dev.ToString()))
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
            app.UseSpaStaticFiles();
            
            app.UseRouting();
            
            app.UseCors(AdfsCorsPolicy);
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                // Map controller endpoints and make them require authorization by default.
                // Anonymous controllers are still possible by decorating with attribute [AllowAnonymous]
                //endpoints.MapControllers().RequireAuthorization();
                endpoints.MapControllers();
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