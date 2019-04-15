using NIST.CVP.Common;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.KDF.Tests;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Linq;
using NIST.CVP.Generation.KDF.v1_0;

namespace NIST.CVP.Generation.KDF.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "KDF";
        public override string Mode => "";

        public override AlgoMode AlgoMode => AlgoMode.KDF_v1_0;

        public override IRegisterInjections RegistrationsCrypto => new Crypto.RegisterInjections();
        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            if (testCase.keyOut != null)
            {
                var bs = new BitString(testCase.keyOut.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.keyOut = bs.ToHex();
            }
        }

        protected override string GetTestFileFewTestCases(string folderName)
        {
            var capabilities = new[]
            {
                new CapabilityBuilder()
                    .WithKdfMode("counter")
                    .WithMacMode(new [] {"cmac-aes128", "hmac-sha2-384"})
                    .WithCounterLength(new [] {24})
                    .WithFixedDataOrder(new [] {"after fixed data", "before fixed data"})
                    .WithSupportedLengths(new MathDomain().AddSegment(new ValueDomainSegment(128)))
                    .Build(),

                new CapabilityBuilder()
                    .WithKdfMode("feedback")
                    .WithMacMode(new [] {"cmac-tdes", "hmac-sha2-512"})
                    .WithCounterLength(new [] {0})
                    .WithFixedDataOrder(new [] {"none"})
                    .WithSupportedLengths(new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 8, 512, 8)))
                    .Build()
            };
            
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = true,
                Capabilities = capabilities
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileLotsOfTestCases(string folderName)
        {
            var capabilities = new[]
            {
                new CapabilityBuilder()
                    .WithKdfMode("counter")
                    .WithMacMode(ParameterValidator.VALID_MAC_MODES)
                    .WithCounterLength(ParameterValidator.VALID_COUNTER_LENGTHS.Except(new [] {0}).ToArray())
                    .WithFixedDataOrder(new [] {"after fixed data", "before fixed data", "middle fixed data"})
                    .WithSupportedLengths(new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 8, 1024)))
                    .Build(),

                new CapabilityBuilder()
                    .WithKdfMode("feedback")
                    .WithMacMode(ParameterValidator.VALID_MAC_MODES)
                    .WithCounterLength(ParameterValidator.VALID_COUNTER_LENGTHS)
                    .WithFixedDataOrder(new [] {"none", "after fixed data", "before fixed data", "before iterator"})
                    .WithSupportedLengths(new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 8, 1024)))
                    .WithSupportsEmptyIv(true)
                    .Build(),

                new CapabilityBuilder()
                    .WithKdfMode("double pipeline iteration")
                    .WithMacMode(ParameterValidator.VALID_MAC_MODES)
                    .WithCounterLength(ParameterValidator.VALID_COUNTER_LENGTHS)
                    .WithFixedDataOrder(new [] {"none", "after fixed data", "before fixed data", "before iterator"})
                    .WithSupportedLengths(new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 8, 1024)))
                    .Build()
            };
            
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = true,
                Capabilities = capabilities
            };

            return CreateRegistration(folderName, p);
        }
    }
}
