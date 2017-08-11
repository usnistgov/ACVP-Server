using Autofac;
using NIST.CVP.Generation.Core;
using NLog;
using System;

namespace RSA_SigGen
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
            LoggingHelper.ConfigureLogging(requestFile, "rsa-siggen");
            Logger.Info($"Generating Test Vectors for {requestFile}");

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

                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"ERROR! Generating Test Vectors for {requestFile}: {ex.Message}, {ex.StackTrace}");
                return 1;
            }

            Logger.Debug($"Success! Generating Test Vectors for {requestFile}");

            return 0;
        }

        private static Logger Logger { get { return LogManager.GetLogger("Generate"); } }
    }
}