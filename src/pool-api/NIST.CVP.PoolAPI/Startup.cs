using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Crypto.Oracle;
using NIST.CVP.Pools;
using NIST.CVP.Pools.Interfaces;
using NIST.CVP.Pools.Services;
using NLog;

namespace NIST.CVP.PoolAPI
{
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<EnvironmentConfig>(Configuration.GetSection(nameof(EnvironmentConfig)));
            services.Configure<AlgorithmConfig>(Configuration.GetSection(nameof(AlgorithmConfig)));
            services.Configure<PoolConfig>(Configuration.GetSection(nameof(PoolConfig)));
            services.Configure<OrleansConfig>(Configuration.GetSection(nameof(OrleansConfig)));

            services.AddSingleton<IMongoDbFactory, MongoDbFactory>();
            services.AddSingleton<IPoolRepositoryFactory, PoolRepositoryFactory>();
            services.AddSingleton<IOracle, Oracle>();
            services.AddSingleton<PoolManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            LogManager.GetCurrentClassLogger().Info("Startup service configured.");
        }
    }
}
