using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using AES_CTR;
using Autofac;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CTR.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class GenValTests : GenValTestsBase
    {
        public override string Algorithm => "AES";
        public override string Mode => "CTR";

        public override Executable Generator => Program.Main;
        public override Executable Validator => AES_CTR_Val.Program.Main;

        [SetUp]
        public override void SetUp()
        {
            //SaveJson = true;
            AutofacConfig.OverrideRegistrations = null;
            AES_CTR_Val.AutofacConfig.OverrideRegistrations = null;
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
            AES_CTR_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeException()
        {
            AES_CTR_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override string GetTestFileMinimalTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Direction = new[] {"encrypt"},
                KeyLen = new[] {128},
                DataLength = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                IsSample = true,
                IncrementalCounter = true,
                OverflowCounter = false
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileFewTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Direction = new[] { "encrypt" },
                KeyLen = new[] { 128, 256 },
                DataLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 8, 128, 8)),
                IsSample = true,
                IncrementalCounter = false,
                OverflowCounter = true
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileLotsOfTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KeyLen = ParameterValidator.VALID_KEY_SIZES,
                DataLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 1, 128)),
                IsSample = true,    // needs sample to complete deferred cases
                IncrementalCounter = true,
                OverflowCounter = true
            };

            return CreateRegistration(folderName, p);
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC has a cipherText, change it
            if (testCase.cipherText != null)
            {
                var bs = new BitString(testCase.cipherText.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.cipherText = bs.ToHex();
            }

            // If TC has a plainText, change it
            if (testCase.plainText != null)
            {
                var bs = new BitString(testCase.plainText.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.plainText = bs.ToHex();
            }
        }
    }
}
