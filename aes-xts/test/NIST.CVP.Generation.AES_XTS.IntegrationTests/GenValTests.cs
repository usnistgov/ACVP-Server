using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using AES_XTS;
using Autofac;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.AES_XTS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsBase
    {
        public override string Algorithm { get; } = "AES";
        public override string Mode { get; } = "XTS";

        public override Executable Generator => Program.Main;
        public override Executable Validator => AES_XTS_Val.Program.Main;

        [SetUp]
        public override void SetUp()
        {
            AutofacConfig.OverrideRegistrations = null;
            AES_XTS_Val.AutofacConfig.OverrideRegistrations = null;
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
            AES_XTS_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeException()
        {
            AES_XTS_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC is intended to be a failure test, change it
            if (testCase.decryptFail != null)
            {
                testCase.decryptFail = false;
            }

            // If TC has a cipherText, change it
            if (testCase.cipherText != null)
            {
                BitString bs = new BitString(testCase.cipherText.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.cipherText = bs.ToHex();
            }

            // If TC has a plainText, change it
            if (testCase.plainText != null)
            {
                BitString bs = new BitString(testCase.plainText.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.plainText = bs.ToHex();
            }
        }

        protected override string GetTestFileMinimalTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                KeyLen = new[] {128},
                Direction = new[] {"encrypt"},
                PtLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                TweakModes = new[] {"hex"},
                IsSample = true
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileFewTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                KeyLen = new[] { 128 },
                Direction = new[] { "encrypt", "decrypt" },
                PtLen = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 128, 512)),
                TweakModes = new[] { "hex", "number" },
                IsSample = false
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileLotsOfTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                KeyLen = new[] { 128, 256 },
                Direction = new[] { "encrypt", "decrypt" },
                PtLen = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 128, 65536)),
                TweakModes = new[] { "hex", "number" },
                IsSample = false
            };

            return CreateRegistration(folderName, p);
        }
    }
}
