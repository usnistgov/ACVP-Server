using System.Collections.Generic;
using Autofac;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.SHA3.v2_0;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA3.IntegrationTests.v2_0
{
    public abstract class GenValTestsSha3NewAlgoBase : GenValTestsSingleRunnerBase
    {
        public abstract override string Algorithm { get; }
        public override string Mode { get; } = null;

        public abstract string[] Modes { get; }
        public abstract int[] SeedLength { get; }
        public override string Revision { get; set; } = "2.0";
        
        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            MathDomain dom = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 1720, 12144, 8));

            var parameters = new Parameters
            {
                Algorithm = Algorithm,
                Revision = Revision,
                DigestSizes = new List<string> { "256" },
                MessageLength = dom,
                PerformLargeDataTest = new[] { 1, 2 },
                IsSample = true
            };

            return CreateRegistration(targetFolder, parameters);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            MathDomain dom = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 1720, 12144, 8));

            // Tests NEW Hash Algorithm
            var parameters = new Parameters
            {
                Algorithm = Algorithm,
                Revision = Revision,
                DigestSizes = new List<string> { "256" },
                MessageLength = dom,
                PerformLargeDataTest = ParameterValidator.VALID_LARGE_DATA_SIZES,
                IsSample = false
            };

            return CreateRegistration(targetFolder, parameters);
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
        
        /// <summary>
        /// Can be used to exclude MCT tests
        /// </summary>
        protected override void OverrideRegisteredDependencies(ContainerBuilder builder)
        {
            base.OverrideRegisteredDependencies(builder);

            builder.RegisterType<FakeTestGroupGeneratorFactory>().AsImplementedInterfaces();
        }
        
        public class FakeTestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
        {
            public IEnumerable<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>> GetTestGroupGenerators(Parameters parameters)
            {
                return new List<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>>
                {
                    new TestGroupGeneratorAft()
                };
            }
        }
    }
}
