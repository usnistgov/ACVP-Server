using System;
using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Orleans.Grains;
using NIST.CVP.Orleans.Grains.Interfaces;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;


namespace NIST.CVP.Orleans.ServerHost
{
    public static class Program
    {
        private static IServiceProvider _serviceProvider { get; }
        private static readonly string _rootDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly OrleansConfig _orleansConfig;

        static Program()
        {
            _serviceProvider = EntryPointConfigHelper.Bootstrap(_rootDirectory);
            _orleansConfig = _serviceProvider.GetService<IOptions<OrleansConfig>>().Value;
        }
        
        static void Main(string[] args)
        {
            var primarySiloEndpoint = new IPEndPoint(
                IPAddress.Parse(_orleansConfig.OrleansServerIp), _orleansConfig.OrleansGatewayPort
            );
            var builder = new SiloHostBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = _orleansConfig.ClusterId;
                    options.ServiceId = Constants.ServiceId;
                })
                // TODO need to make this properly configurable based on environment
                .Configure<EndpointOptions>(options =>
                {
                    options.AdvertisedIPAddress = IPAddress.Loopback;
                })
                .ConfigureServices(ConfigureServices.RegisterServices)
                .UseLocalhostClustering()
                //.UseDevelopmentClustering(primarySiloEndpoint)
                //.ConfigureEndpoints(siloPort: 8080, gatewayPort: 30000)

                .ConfigureApplicationParts(parts =>
                {
                    parts.AddApplicationPart(typeof(IGrainMarker).Assembly).WithReferences();
                }
                )
                //.AddMemoryGrainStorage(Constants.StorageProviderName)
                .ConfigureLogging(logging => logging.AddConsole());
            //need to configure a grain storage called "PubSubStore" for using streaming with ExplicitSubscribe pubsub type
            //.AddMemoryGrainStorage("PubSubStore")
            //Depends on your application requirements, you can configure your silo with other stream providers, which can provide other features, 
            //such as persistence or recoverability. For more information, please see http://dotnet.github.io/orleans/Documentation/Orleans-Streams/Stream-Providers.html
            //.AddSimpleMessageStreamProvider(Constants.ChatRoomStreamProvider); 


            var silo = builder.Build();
            silo.StartAsync().Wait();

            Console.WriteLine("Press Enter to close.");
            Console.ReadLine();

            // shut the silo down after we are done.
            silo.StopAsync().Wait();
        }
    }
}