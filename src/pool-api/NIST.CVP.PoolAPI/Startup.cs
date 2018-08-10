using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NIST.CVP.Pools;

namespace NIST.CVP.PoolAPI
{
    public class Startup
    {
        public static PoolManager PoolManager { get; set; }

        public Startup(IConfiguration config)
        {
            Configuration = config;

            var poolConfigFile = config.GetValue<string>("PoolConfig");
            var poolDirectory = config.GetValue<string>("PoolDirectory");
            PoolManager = new PoolManager(poolConfigFile, poolDirectory);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
        }
    }
}
