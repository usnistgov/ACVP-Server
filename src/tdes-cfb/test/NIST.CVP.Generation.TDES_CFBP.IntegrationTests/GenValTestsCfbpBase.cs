using Autofac;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Collections.Generic;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;

namespace NIST.CVP.Generation.TDES_CFBP.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public abstract class GenValTestsCfbpBase : GenValTestsSingleRunnerBase
    {
        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            //TODO the duplication between testCase and resultArray should be refactored
            var rand = new Random800_90();
            if (testCase.decryptFail != null)
            {
                testCase.decryptFail = false;
            }

            var propertiesToScramble = new[] {"cipherText", "cipherText1", "cipherText2", "cipherText3", "plainText", "plainText1", "plainText2", "plainText3" };
            foreach(var prop in propertiesToScramble)
            {
                if (testCase[prop] != null)
                {
                    var bs = new BitString(testCase[prop].ToString());
                    if (bs.BitLength == 0)
                    {
                        bs = new BitString("FF");
                    }
                    else
                    {
                        bs = bs.NOT();
                    }
                    
                    testCase[prop] = bs.ToHex();
                }
            }

            // If TC has a resultsArray, change some of the elements
            if (testCase.resultsArray != null)
            {
                var arrayPropertiesToScramble = new[] { "cipherText", "plainText", "iv1", "iv2", "iv3", "key1", "key2", "key3" };
                foreach (var prop in arrayPropertiesToScramble)
                {
                    if (testCase.resultsArray[0][prop] != null)
                    {
                        var bs = new BitString(testCase.resultsArray[0][prop].ToString());
                        if (bs.BitLength == 0)
                        {
                            bs = new BitString("FF");
                        }
                        else
                        {
                            bs = bs.NOT();
                        }

                        testCase.resultsArray[0][prop] = bs.ToHex();
                    }
                }
            }
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Direction = new[] { "encrypt" },
                IsSample = true,
                KeyingOption = new[] { 1 }
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Direction = ParameterValidator.VALID_DIRECTIONS,
                IsSample = false,
                KeyingOption = new[] { 1 }
            };

            return CreateRegistration(targetFolder, p);
        }

        /// <summary>
        /// Can be used to only generate MMT groups for the genval tests
        /// </summary>
        public class FakeTestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters>
        {
            public IEnumerable<ITestGroupGenerator<Parameters>> GetTestGroupGenerators()
            {
                return new List<ITestGroupGenerator<Parameters>>()
                {
                    new TestGroupGeneratorMultiblockMessage()
                };
            }
        }

        protected override void OverrideRegisteredDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<FakeTestGroupGeneratorFactory>().AsImplementedInterfaces();
        }
    }
}
