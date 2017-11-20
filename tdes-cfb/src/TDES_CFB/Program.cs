using NIST.CVP.Generation.Core;
using System;
using NLog;
using Autofac;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.TDES_CFB;
using NIST.CVP.Generation.Core.Helpers;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.TDES_CFB;
using NIST.CVP.Math;

namespace tdes_cfb
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.Error.WriteLine("Invalid arguments supplied");
                return 1;
            }
            var algoStr = args[0];
            var requestFile = args[1];
            LoggingHelper.ConfigureLogging(requestFile, algoStr);
            Logger.Info($"Generating Test Vectors for {requestFile}");

            var algo = EnumHelpers.GetEnumFromEnumDescription<Algo>(algoStr);
            //get generator and call it
            Logger.Debug("Generating");

            try
            {
                AutofacConfig.IoCConfiguration(algo);
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
