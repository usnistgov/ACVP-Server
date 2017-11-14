using System.Collections.Generic;
using System.IO;
using Autofac;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using DRBG;

namespace NIST.CVP.Generation.DRBG.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class GenValTests : GenValTestsBase
    {
        public override string Algorithm { get; } = "DRBG";
        public override string Mode { get; } = "AES";

        public override Executable Generator => Program.Main;
        public override Executable Validator => DRBG_Val.Program.Main;

        [OneTimeSetUp]
        public override void OneTimeSetUp()
        {
            TestPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\temp_integrationTests\");
        }

        [SetUp]
        public override void SetUp()
        {
            AutofacConfig.OverrideRegistrations = null;
            AutofacConfig.IoCConfiguration();

            DRBG_Val.AutofacConfig.OverrideRegistrations = null;
            DRBG_Val.AutofacConfig.IoCConfiguration();
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
            DRBG_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeException()
        {
            DRBG_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC has a plainText, change it
            if (testCase.returnedBits != null)
            {
                var bs = new BitString(testCase.returnedBits.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.returnedBits = bs.ToHex();
            }
        }

        protected override string GetTestFileMinimalTestCases(string targetFolder)
        {
            return GetTestFileFewTestCases(targetFolder);
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            MathDomain nonceLen = new MathDomain();
            nonceLen.AddSegment(new ValueDomainSegment(128));

            MathDomain additionalInputLen = new MathDomain();
            additionalInputLen.AddSegment(new RangeDomainSegment(new Random800_90(), 128, 192, 64));

            MathDomain persoStringLen = new MathDomain();
            persoStringLen.AddSegment(new ValueDomainSegment(128));

            MathDomain entropyInputLen = new MathDomain();
            entropyInputLen.AddSegment(new ValueDomainSegment(128));

            Parameters p = new Parameters()
            {
                Algorithm = "ctrDRBG",
                Mode = "AES-128",
                NonceLen = nonceLen,
                AdditionalInputLen = additionalInputLen,
                PersoStringLen = persoStringLen,
                EntropyInputLen = entropyInputLen,
                ReturnedBitsLen = 128*4,
                DerFuncEnabled = true,
                ReseedImplemented = true,
                PredResistanceEnabled = true
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            MathDomain nonceLen = new MathDomain();
            nonceLen.AddSegment(new ValueDomainSegment(128));

            MathDomain additionalInputLen = new MathDomain();
            additionalInputLen.AddSegment(new RangeDomainSegment(new Random800_90(), 128, 192, 64));
            additionalInputLen.AddSegment(new ValueDomainSegment(256));

            MathDomain persoStringLen = new MathDomain();
            persoStringLen.AddSegment(new ValueDomainSegment(128));
            persoStringLen.AddSegment(new RangeDomainSegment(new Random800_90(), 256, 512, 128));

            MathDomain entropyInputLen = new MathDomain();
            entropyInputLen.AddSegment(new ValueDomainSegment(256));

            Parameters p = new Parameters()
            {
                Algorithm = "ctrDRBG",
                Mode = "AES-256",
                NonceLen = nonceLen,
                AdditionalInputLen = additionalInputLen,
                PersoStringLen = persoStringLen,
                EntropyInputLen = entropyInputLen,
                ReturnedBitsLen = 128 * 4,
                DerFuncEnabled = true,
                ReseedImplemented = true,
                PredResistanceEnabled = true
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
