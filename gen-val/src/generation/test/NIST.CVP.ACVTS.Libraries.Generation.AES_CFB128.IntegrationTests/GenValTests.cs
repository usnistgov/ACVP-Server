﻿using System.Collections.Generic;
using System.Linq;
using Autofac;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.AES_CFB128.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CFB128.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "ACVP-AES-CFB128";
        public override string Mode { get; } = null;

        public override AlgoMode AlgoMode => AlgoMode.AES_CFB128_v1_0;


        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC has a cipherText, change it
            if (testCase.ct != null)
            {
                BitString bs = new BitString(testCase.ct.ToString());
                testCase.ct = rand.GetDifferentBitStringOfSameSize(bs).ToHex();
            }

            // If TC has a plainText, change it
            if (testCase.pt != null)
            {
                BitString bs = new BitString(testCase.pt.ToString());
                testCase.pt = rand.GetDifferentBitStringOfSameSize(bs).ToHex();
            }

            // If TC has a resultsArray, change some of the elements
            if (testCase.resultsArray != null)
            {
                BitString bsIV = new BitString(testCase.resultsArray[0].iv.ToString());
                testCase.resultsArray[0].iv = rand.GetDifferentBitStringOfSameSize(bsIV).ToHex();

                BitString bsKey = new BitString(testCase.resultsArray[0].key.ToString());
                testCase.resultsArray[0].key = rand.GetDifferentBitStringOfSameSize(bsKey).ToHex();

                BitString bsPlainText = new BitString(testCase.resultsArray[0].pt.ToString());
                testCase.resultsArray[0].pt = rand.GetDifferentBitStringOfSameSize(bsPlainText).ToHex();

                BitString bsCipherText = new BitString(testCase.resultsArray[0].ct.ToString());
                testCase.resultsArray[0].ct = rand.GetDifferentBitStringOfSameSize(bsCipherText).ToHex();
            }
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            Parameters p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Direction = new[] { "encrypt" },
                KeyLen = new[] { ParameterValidator.VALID_KEY_SIZES.First() },
                IsSample = true
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            Parameters p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KeyLen = ParameterValidator.VALID_KEY_SIZES,
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }

        /// <summary>
        /// Can be used to only generate MMT groups for the genval tests
        /// </summary>
        public class FakeTestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
        {
            public IEnumerable<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>> GetTestGroupGenerators(Parameters parameters)
            {
                return new List<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>>
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
