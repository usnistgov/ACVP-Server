using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NIST.CVP.Common.Oracle;
using NLog;

namespace NIST.CVP.PoolAPI
{
    public class Startup
    {
        private const string CorsPolicyName = "AllowAnyOrigin";

        public Startup(IConfiguration config)
        {
            Logger.Info("Startup service ctor.");
            Configuration = config;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Logger.Info("Configuring IOC container.");

            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName,
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });

            services.AddControllersWithViews()
                .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env, IHostApplicationLifetime applicationLifetime, IOracle oracle)
        {
            Logger.Info("Configuring Startup service...");
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors(CorsPolicyName);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            applicationLifetime.ApplicationStarted.Register(() =>
            {
                oracle.InitializeClusterClient().Wait();
            });
            applicationLifetime.ApplicationStopping.Register(() =>
            {
                oracle.CloseClusterClient().Wait();
            });
            
            Logger.Info("Startup service configured.");
        }

        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    }
}
