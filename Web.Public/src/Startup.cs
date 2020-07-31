using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Orleans.Runtime;
using Serilog;
using Web.Public.Configs;
using Web.Public.Exceptions;
using Web.Public.Providers;
using Web.Public.Services;

namespace Web.Public
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private JwtConfig _jwtConfig;
        private string _jwtSigningKey;
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
            
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CertificateAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCertificate(options =>
                {
                    options.AllowedCertificateTypes = CertificateTypes.All;
                    options.ValidateCertificateUse = false;
                    options.RevocationMode = X509RevocationMode.NoCheck;
                    options.Events = new CertificateAuthenticationEvents()
                    {
                        OnCertificateValidated = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetService<ILogger<Startup>>();
                            var certValidatorService =
                                context.HttpContext.RequestServices.GetService<IMtlsCertValidatorService>();

                            if (!certValidatorService.IsValid(context.ClientCertificate))
                            {
                                context.Fail("Certificate did not pass validation procedure.");
                            }
                            
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetService<ILogger<Startup>>();
                            
                            logger.LogError(context.Exception, "Failed auth.");

                            return Task.CompletedTask;
                        } 
                    };
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = _jwtConfig.Issuer,
                        ValidateActor = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateTokenReplay = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(_jwtSigningKey)),
                        SaveSigninToken = true
                    };
                    options.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetService<ILogger<Startup>>();

                            if (context.Exception != null)
                            {
                                logger.LogDebug(context.Exception, "Failed JWT auth. Caught within Startup.");
                                // Rethrow the exception so that it can be caught by the exception handling middleware.
                                throw context.Exception;    
                            }
                            
                            return Task.CompletedTask;
                        }
                    };
                });

            services
                .AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.AllowTrailingCommas = false;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(ILogger<Startup> logger, IApplicationBuilder app, IWebHostEnvironment env, IOptions<JwtConfig> jwtConfig, ISecretKvpProvider secretKvpProvider, IJsonWriterService jsonWriterService)
        {
            logger.LogInformation("Startup.Configure");
            
            _jwtConfig = jwtConfig.Value;
            _jwtSigningKey = secretKvpProvider.GetValueFromKey(SecretKvpProvider.JwtSigningKey);

            app.ConfigureExceptionMiddleware(logger, jsonWriterService);

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseSerilogRequestLogging();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "home",
                    pattern: "/");
            });
        }
    }
}