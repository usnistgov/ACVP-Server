using System;

using Orleans;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;

namespace OrleansHelloWorld
{
    /// <summary>
    /// Orleans test silo host
    /// NOTE this is full framework, there is a core version in preview, wanted to check this out first.
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            // First, configure and start a local silo
            var siloConfig = ClusterConfiguration.LocalhostPrimarySilo();
            var silo = new SiloHost("TestSilo", siloConfig);
            silo.InitializeOrleansSilo();
            silo.StartOrleansSilo();

            Console.WriteLine("Silo started.");

            // Then configure and connect a client.
            var clientConfig = ClientConfiguration.LocalhostSilo();
            var client = new ClientBuilder().UseConfiguration(clientConfig).Build();
            client.Connect().Wait();

            Console.WriteLine("Client connected.");

            //
            // This is the place for your test code.
            //

            var friend = client.GetGrain<GrainHelloWorld.Interfaces.IGrain1>(Guid.NewGuid());
            Console.WriteLine("\n\n{0}\n\n", friend.SayHello().Result);

            Console.WriteLine("\nPress Enter to terminate...");
            Console.ReadLine();

            // Shut down
            client.Close();
            silo.ShutdownOrleansSilo();
        }
    }
}
