﻿using Autofac;
using NIST.CVP.Common;
using NIST.CVP.Generation.AES_CBC_CTS.v1_0;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Generation.AES_CBC_CTS.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public abstract class GenValTestsCtsBase : GenValTestsSingleRunnerBase
    {
        public override string Mode { get; } = string.Empty;



        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

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

                BitString bsKey = new BitString(testCase.resultsArray[0].key.ToString());
                bsKey = rand.GetDifferentBitStringOfSameSize(bsKey);
                testCase.resultsArray[0].key = bsKey.ToHex();

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
                VectorSetId = 42,
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                PayloadLen = new MathDomain().AddSegment(new ValueDomainSegment(129)),
                Direction = new string[] { "encrypt" },
                KeyLen = new int[] { ParameterValidator.ValidKeySizes.First() },
                IsSample = true
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                VectorSetId = 42,
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                PayloadLen = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 128, 65536)),
                Direction = ParameterValidator.ValidDirections,
                KeyLen = ParameterValidator.ValidKeySizes,
                IsSample = false
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
                    new TestGroupGeneratorMultiBlockMessagePartialBlock(),
                    new TestGroupGeneratorMultiBlockMessageFullBlock()
                };
            }
        }

        protected override void OverrideRegisteredDependencies(ContainerBuilder builder)
        {
            //builder.RegisterType<FakeTestGroupGeneratorFactory>().AsImplementedInterfaces();
        }
    }
}