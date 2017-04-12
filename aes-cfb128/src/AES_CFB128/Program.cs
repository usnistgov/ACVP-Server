using System;
using System.IO;
using Autofac;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Generation.AES_CFB128;
using NIST.CVP.Generation.Core;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace AES_CFB128
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
            LoggingHelper.ConfigureLogging(requestFile, "aes-cfb8");
            Logger.Info($"Generating Test Vectors for {requestFile}");

            //get generator and call it
            Logger.Debug("Generating");

            try
            {
                AutofacConfig.IoCConfiguration();
                using (var scope = AutofacConfig.Container.BeginLifetimeScope())
                {
                    var gen = scope.Resolve<Generator<Parameters,TestVectorSet>>();
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
