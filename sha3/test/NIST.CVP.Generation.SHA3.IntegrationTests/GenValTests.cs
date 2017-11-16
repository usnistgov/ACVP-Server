using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Helpers;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using SHA3;

namespace NIST.CVP.Generation.SHA3.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsBase
    {
        public override string Algorithm { get; } = "SHA";
        public override string Mode { get; } = "3";

        public override Executable Generator => Program.Main;
        public override Executable Validator => SHA3_Val.Program.Main;

        [SetUp]
        public override void SetUp()
        {
            AutofacConfig.OverrideRegistrations = null;
            SHA3_Val.AutofacConfig.OverrideRegistrations = null;
        }

        protected override void OverrideRegistrationGenFakeFailure()
        {
            AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureParameterParser<Parameters>>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeFailure()
        {
            SHA3_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeException()
        {
            SHA3_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            if (testCase.md != null)
            {
                var bs = new BitString(testCase.md.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);
                testCase.md = bs.ToHex();
            }

            if (testCase.msg != null)
            {
                var bs = new BitString(testCase.msg.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);
                testCase.msg = bs.ToHex();
            }

            if (testCase.resultsArray != null)
            {
                var bsMessage = new BitString(testCase.resultsArray[0].msg.ToString());
                bsMessage = rand.GetDifferentBitStringOfSameSize(bsMessage);
                testCase.resultsArray[0].msg = bsMessage.ToHex();

                var bsDigest = new BitString(testCase.resultsArray[0].md.ToString());
                bsDigest = rand.GetDifferentBitStringOfSameSize(bsDigest);
                testCase.resultsArray[0].md = bsDigest.ToHex();
            }
        }

        protected override string GetTestFileMinimalTestCases(string targetFolder)
        {
            var parameters = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                DigestSizes = new[] { 224 },
                BitOrientedInput = false,
                IncludeNull = true,
                IsSample = true
            };

            return CreateRegistration(targetFolder, parameters);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var parameters = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                DigestSizes = new [] {224, 256, 384, 512},
                BitOrientedInput = true,
                IncludeNull = true,
                IsSample = true
            };

            return CreateRegistration(targetFolder, parameters);
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            RemoveMctAndVotTestGroupFactories();
            var minMax = new MathDomain();
            minMax.AddSegment(new RangeDomainSegment(null, 256, 512, 8));

            var parameters = new Parameters
            {
                Algorithm = "SHAKE",
                Mode = "",
                DigestSizes = new[] { 128 },
                BitOrientedInput = false,
                BitOrientedOutput = false,
                IncludeNull = false,
                OutputLength = minMax,
                IsSample = true
            };

            return CreateRegistration(targetFolder, parameters);
        }

        //private string GetTestFileManyTestCasesSHAKE(string targetFolder)
        //{
        //    var minMax = new MathDomain();
        //    minMax.AddSegment(new RangeDomainSegment(null, 256, 512, 1));

        //    var parameters = new Parameters
        //    {
        //        Algorithm = "SHAKE",
        //        DigestSizes = new[] { 128, 256 },
        //        BitOrientedInput = true,
        //        BitOrientedOutput = true,
        //        IncludeNull = true,
        //        OutputLength = minMax,
        //        IsSample = true
        //    };

        //    return CreateRegistration(targetFolder, parameters);
        //}

        /// <summary>
        /// Can be used to only generate AFT groups for the genval tests
        /// </summary>
        public class FakeTestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters>
        {
            public IEnumerable<ITestGroupGenerator<Parameters>> GetTestGroupGenerators()
            {
                return new List<ITestGroupGenerator<Parameters>>()
                {
                    new TestGroupGeneratorAlgorithmFunctional()
                };
            }
        }
        
        private void RemoveMctAndVotTestGroupFactories()
        {
            AutofacConfig.OverrideRegistrations += builder =>
            {
                builder.RegisterType<FakeTestGroupGeneratorFactory>().AsImplementedInterfaces();
            };
        }
    }
}
