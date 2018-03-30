using System.Collections.Generic;
using System.Linq;
using Autofac;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CFB128.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "AES";
        public override string Mode { get; } = "CFB128";

        public override AlgoMode AlgoMode => AlgoMode.AES_CFB128;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC is intended to be a failure test, change it
            if (testCase.decryptFail != null)
            {
                testCase.decryptFail = false;
            }

            // If TC has a cipherText, change it
            if (testCase.cipherText != null)
            {
                BitString bs = new BitString(testCase.cipherText.ToString());
                testCase.cipherText = rand.GetDifferentBitStringOfSameSize(bs).ToHex();
            }

            // If TC has a plainText, change it
            if (testCase.plainText != null)
            {
                BitString bs = new BitString(testCase.plainText.ToString());
                testCase.plainText = rand.GetDifferentBitStringOfSameSize(bs).ToHex();
            }

            // If TC has a resultsArray, change some of the elements
            if (testCase.resultsArray != null)
            {
                BitString bsIV = new BitString(testCase.resultsArray[0].iv.ToString());
                testCase.resultsArray[0].iv = rand.GetDifferentBitStringOfSameSize(bsIV).ToHex();

                BitString bsKey = new BitString(testCase.resultsArray[0].key.ToString());
                testCase.resultsArray[0].key = rand.GetDifferentBitStringOfSameSize(bsKey).ToHex();

                BitString bsPlainText = new BitString(testCase.resultsArray[0].plainText.ToString());
                testCase.resultsArray[0].plainText = rand.GetDifferentBitStringOfSameSize(bsPlainText).ToHex();

                BitString bsCipherText = new BitString(testCase.resultsArray[0].cipherText.ToString());
                testCase.resultsArray[0].cipherText = rand.GetDifferentBitStringOfSameSize(bsCipherText).ToHex();
            }
        }
        
        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Direction = new string[] { "encrypt" },
                KeyLen = new int[] { ParameterValidator.VALID_KEY_SIZES.First() },
                IsSample = true
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
                KeyLen = ParameterValidator.VALID_KEY_SIZES,
                IsSample = false
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
                    new TestGroupGeneratorMultiBlockMessage()
                };
            }
        }

        protected override void OverrideRegisteredDependencies(ContainerBuilder builder)
        {
            base.OverrideRegisteredDependencies(builder);

            builder.RegisterType<FakeTestGroupGeneratorFactory>().AsImplementedInterfaces();
        }
    }
}
