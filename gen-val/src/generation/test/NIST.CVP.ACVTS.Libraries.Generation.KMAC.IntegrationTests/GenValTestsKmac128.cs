﻿using System.Collections.Generic;
using Autofac;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.KMAC.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.KMAC.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsKmac128 : GenValTestsSingleRunnerBase
    {
        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();


        public override string Algorithm { get; } = "KMAC-128";
        public override string Mode { get; } = null;
        public override AlgoMode AlgoMode => AlgoMode.KMAC_128_v1_0;

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            if (testCase.mac != null)
            {
                var bs = new BitString(testCase.mac.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);
                testCase.mac = bs.ToHex();
            }

            if (testCase.msg != null)
            {
                var bs = new BitString(testCase.msg.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);
                testCase.msg = bs.ToHex();
            }
        }

        /// <summary>
        /// Can be used to only generate AFT groups for the genval tests
        /// </summary>
        public class FakeTestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
        {
            public IEnumerable<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>> GetTestGroupGenerators(Parameters parameters)
            {
                return new List<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>>
                {
                    new TestGroupGeneratorAlgorithmFunctional()
                };
            }
        }

        protected override void OverrideRegisteredDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<FakeTestGroupGeneratorFactory>().AsImplementedInterfaces();
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var minMax = new MathDomain();
            minMax.AddSegment(new RangeDomainSegment(null, 256, 512, 8));

            var parameters = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                DigestSizes = new List<int> { 128 },
                MsgLen = minMax,
                MacLen = minMax,
                KeyLen = minMax,
                HexCustomization = true,
                XOF = new[] { true, false },
                IsSample = true
            };

            return CreateRegistration(targetFolder, parameters);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var minMax = new MathDomain();
            minMax.AddSegment(new RangeDomainSegment(null, 256, 4096));

            var minMaxMsg = new MathDomain();
            minMaxMsg.AddSegment(new RangeDomainSegment(null, 0, 65536));

            var parameters = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                DigestSizes = new List<int> { 128 },
                MsgLen = minMaxMsg,
                MacLen = minMax,
                KeyLen = minMax,
                HexCustomization = true,
                XOF = new[] { true, false },
                IsSample = false,
            };

            return CreateRegistration(targetFolder, parameters);
        }
    }
}
