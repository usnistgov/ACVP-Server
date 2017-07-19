using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.PlatformAbstractions;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.TDES_ECB;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace tdes_ecb
{
    public class Program
    {
        public static int Main(string[] args)
        {

            if (args.Length < 1)
            {
                Console.Error.WriteLine("No arguments supplied");
                return 1;
            }
            var requestFile = args[0];
            LoggingHelper.ConfigureLogging(requestFile, "tdes-ecb");
            Logger.Info($"Running {PlatformServices.Default.Application.ApplicationName} { PlatformServices.Default.Application.ApplicationVersion}");
            Logger.Info($"Generating Test Vectors for {requestFile}");

            //get generator and call it
            Logger.Debug("Generating");

            try
            {
                AutofacConfig.IoCConfiguration();
                using (var scope = AutofacConfig.Container.BeginLifetimeScope())
                {
                    var gen = scope.Resolve<IGenerator>();
                    var result = gen.Generate(requestFile);
                    if (!result.Success)
                    {
                        Console.Error.WriteLine(
                            $"ERROR! Generating Test Vectors for {requestFile}: {result.ErrorMessage}");
                        Logger.Error($"ERROR! Generating Test Vectors for {requestFile}: {result.ErrorMessage}");
                        //Console.ReadLine();
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"ERROR! Generating Test Vectors for {requestFile}: {ex.Message}");
                return 1;
            }


            Logger.Debug($"Success! Generating Test Vectors for {requestFile}");
            //Console.ReadLine();

            return 0;
        }

        private static Logger Logger
        {
            get { return LogManager.GetLogger("Generate"); }
        }

      
       
    }
}
