using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KDF.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Generation.Tests.KDF;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "KDF";
        public override string Mode => null;

        public override AlgoMode AlgoMode => AlgoMode.KDF_v1_0;


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
                    .WithKdfMode(KdfModes.Counter)
                    .WithMacMode(new [] {MacModes.CMAC_AES128, MacModes.HMAC_SHA384})
                    .WithCounterLength(new [] {24})
                    .WithFixedDataOrder(new [] {CounterLocations.AfterFixedData, CounterLocations.BeforeFixedData})
                    .WithSupportedLengths(new MathDomain().AddSegment(new ValueDomainSegment(128)))
                    .Build(),

                new CapabilityBuilder()
                    .WithKdfMode(KdfModes.Feedback)
                    .WithMacMode(new [] {MacModes.CMAC_TDES, MacModes.HMAC_SHA3_224})
                    .WithCounterLength(new [] {0})
                    .WithFixedDataOrder(new [] {CounterLocations.None})
                    .WithRequiresEmptyIv(true)
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
                    .WithKdfMode(KdfModes.Counter)
                    .WithMacMode(ParameterValidator.VALID_MAC_MODES)
                    .WithCounterLength(ParameterValidator.VALID_COUNTER_LENGTHS.Except(new [] {0}).ToArray())
                    .WithFixedDataOrder(new [] {CounterLocations.AfterFixedData, CounterLocations.BeforeFixedData, CounterLocations.MiddleFixedData})
                    .WithSupportedLengths(new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 8, 1024)))
                    .Build(),

                new CapabilityBuilder()
                    .WithKdfMode(KdfModes.Feedback)
                    .WithMacMode(ParameterValidator.VALID_MAC_MODES)
                    .WithCounterLength(ParameterValidator.VALID_COUNTER_LENGTHS)
                    .WithFixedDataOrder(new [] {CounterLocations.None, CounterLocations.AfterFixedData, CounterLocations.BeforeFixedData, CounterLocations.BeforeIterator})
                    .WithSupportedLengths(new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 8, 1024)))
                    .WithSupportsEmptyIv(true)
                    .Build(),

                new CapabilityBuilder()
                    .WithKdfMode(KdfModes.Pipeline)
                    .WithMacMode(ParameterValidator.VALID_MAC_MODES)
                    .WithCounterLength(ParameterValidator.VALID_COUNTER_LENGTHS)
                    .WithFixedDataOrder(new [] {CounterLocations.None, CounterLocations.AfterFixedData, CounterLocations.BeforeFixedData, CounterLocations.BeforeIterator})
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
