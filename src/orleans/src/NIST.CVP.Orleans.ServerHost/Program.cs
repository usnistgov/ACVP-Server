using System;
using System.Net;
using Microsoft.Extensions.Logging;
using NIST.CVP.Orleans.Grains;
using NIST.CVP.Orleans.Grains.Interfaces;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace NIST.CVP.Orleans.ServerHost
{
    public class Program
    {
        static void Main(string[] args)
        {

            var builder = new SiloHostBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = Constants.ClusterId;
                    options.ServiceId = Constants.ServiceId;
                })
                .Configure<EndpointOptions>(options =>
                {
                    options.AdvertisedIPAddress = IPAddress.Loopback; 
                })
                .UseLocalhostClustering()
                
                .ConfigureApplicationParts(parts =>
                    {
                        parts.AddApplicationPart(typeof(OracleGrain).Assembly).WithReferences();
                    }
                )
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