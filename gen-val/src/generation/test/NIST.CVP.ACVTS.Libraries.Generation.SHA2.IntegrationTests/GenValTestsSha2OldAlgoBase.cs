using System.Collections.Generic;
using Autofac;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.SHA2.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA2.IntegrationTests
{
    public abstract class GenValTestsSha2OldAlgoBase : GenValTestsSingleRunnerBase
    {
        public abstract override string Algorithm { get; }
        public override string Mode { get; } = null;
        public abstract string[] Modes { get; }
        public abstract int[] SeedLength { get; }
        
        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var parameters = new Parameters
            {
                Algorithm = Algorithm,
                Revision = Revision,
                DigestSizes = new List<string> { "224" },
                MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 8, 65528, 8)),
                PerformLargeDataTest = new[] { 1, 2 },
                IsSample = true
            };

            return CreateRegistration(targetFolder, parameters);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var parameters = new Parameters
            {
                Algorithm = Algorithm,
                Revision = Revision,
                DigestSizes = new List<string> { "224" },
                MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 65535)),
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
                    new TestGroupGeneratorAlgorithmFunctionalTest()
                };
            }
        }
    }
}
