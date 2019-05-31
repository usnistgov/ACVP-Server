using Microsoft.Extensions.DependencyInjection;
using NIST.CVP.Common.Helpers;
using PoolBitStringConverter.Services;
using System;
using System.IO;

namespace PoolBitStringConverter
{
    /// <summary>
    /// Application is used to convert hex string representations of bitStrings into an appropriate object containing both the hex, as well as bitLength.
    /// Currently several poolTypes of data include the hex, but without the bitLength, the hex is interpreted as a full byte.
    ///
    /// This "full byte" works for ***most*** of the pool data, but there are some outliers like:
    ///     AES-CFB1
    ///     TDES-CFB1
    ///     TDES-CFBP1
    ///     AES-CBC-CS1
    ///     AES-CBC-CS2
    ///     AES-CBC-CS3
    /// </summary>
    class Program
    {
        public static IServiceProvider ServiceProvider { get; }
        public static readonly string RootDirectory = AppDomain.CurrentDomain.BaseDirectory;

        static Program()
        {
            var configurationRoot = EntryPointConfigHelper.GetConfigurationRoot(RootDirectory);
            var serviceCollection = EntryPointConfigHelper.GetBaseServiceCollection(configurationRoot);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("This application is used to convert all existing BitStrings contained with the PoolValue db repository into a new format.");
            Console.WriteLine("Previously the BitStrings were written with the assumption of a mod 8 BitString (only contains full bytes).  This should not have been the case.");
            Console.WriteLine("This application will ingest all data from the current database, and save it back under the new BitString methodology (saving the BitLength and BitString together, as an object).");
            Console.WriteLine(
                "Ensure that the current poolConfig.json is present, " +
                "that the 'PoolName' property exists, " +
                "and that the PoolNames do not contain the .json extension within their value."
            );
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Environment.NewLine);

            Console.WriteLine("Enter the full file path/name of the poolConfig.json:");
            var poolConfig = Console.ReadLine();

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Environment.NewLine);

            if (!File.Exists(poolConfig))
            {
                Console.WriteLine("PoolConfig.json not found.");
                Console.ReadKey();

                return;
            }

            Console.WriteLine("Config file found, proceeding with parsing and insertion into database.");

            var poolReserializer = new ReserializeBitStringPoolValueService(ServiceProvider, poolConfig);
            poolReserializer.Execute();
        }



    }
}
