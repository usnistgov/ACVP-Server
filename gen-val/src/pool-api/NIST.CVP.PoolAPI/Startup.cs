using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Interfaces;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Services;
using NIST.CVP.Crypto.Oracle;
using NIST.CVP.Pools;
using NIST.CVP.Pools.Interfaces;
using NIST.CVP.Pools.Services;
using NLog;

namespace NIST.CVP.PoolAPI
{
    public class Startup
    {
        private const string CorsPolicyName = "AllowAnyOrigin";

        public Startup(IConfiguration config)
        {
            LogManager.GetCurrentClassLogger().Info("Startup service ctor.");
            Configuration = config;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            LogManager.GetCurrentClassLogger().Info("Configuring IOC container.");

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
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            LogManager.GetCurrentClassLogger().Info("Configuring Startup service...");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseHsts();
            }

            app.UseRouting();
            app.UseCors(CorsPolicyName);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            LogManager.GetCurrentClassLogger().Info("Startup service configured.");
        }
    }
}
