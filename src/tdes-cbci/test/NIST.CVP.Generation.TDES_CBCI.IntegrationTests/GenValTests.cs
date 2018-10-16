using Autofac;
using NIST.CVP.Common;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Collections.Generic;

namespace NIST.CVP.Generation.TDES_CBCI.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "TDES-CBCI";
        public override string Mode { get; } = string.Empty;

        public override AlgoMode AlgoMode => AlgoMode.TDES_CBCI;

		public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();
		public override IRegisterInjections RegistrationsCrypto => new Crypto.RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC has a cipherText, change it
            if (testCase.ct != null)
            {
                BitString bs = new BitString(testCase.ct.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.ct = bs.ToHex();
            }

            // If TC has a plainText, change it
            if (testCase.pt != null)
            {
                BitString bs = new BitString(testCase.pt.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.pt = bs.ToHex();
            }

            // If TC has a resultsArray, change some of the elements
            if (testCase.resultsArray != null)
            {
                BitString bsIV = new BitString(testCase.resultsArray[0].iv.ToString());
                bsIV = rand.GetDifferentBitStringOfSameSize(bsIV);
                testCase.resultsArray[0].iv = bsIV.ToHex();

                BitString bsKey1 = new BitString(testCase.resultsArray[0].key1.ToString());
                bsKey1 = rand.GetDifferentBitStringOfSameSize(bsKey1);
                testCase.resultsArray[0].key1 = bsKey1.ToHex();

                BitString bsKey2 = new BitString(testCase.resultsArray[0].key2.ToString());
                bsKey2 = rand.GetDifferentBitStringOfSameSize(bsKey2);
                testCase.resultsArray[0].key2 = bsKey2.ToHex();

                BitString bsKey3 = new BitString(testCase.resultsArray[0].key3.ToString());
                bsKey3 = rand.GetDifferentBitStringOfSameSize(bsKey3);
                testCase.resultsArray[0].key3 = bsKey3.ToHex();

                BitString bsPlainText = new BitString(testCase.resultsArray[0].pt.ToString());
                bsPlainText = rand.GetDifferentBitStringOfSameSize(bsPlainText);
                testCase.resultsArray[0].pt = bsPlainText.ToHex();

                BitString bsCipherText = new BitString(testCase.resultsArray[0].ct.ToString());
                bsCipherText = rand.GetDifferentBitStringOfSameSize(bsCipherText);
                testCase.resultsArray[0].ct = bsCipherText.ToHex();
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
            var p = new Parameters
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
        public class FakeTestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
        {
            public IEnumerable<ITestGroupGenerator<Parameters, TestGroup, TestCase>> GetTestGroupGenerators(Parameters parameters)
            {
                return new List<ITestGroupGenerator<Parameters, TestGroup, TestCase>>()
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

