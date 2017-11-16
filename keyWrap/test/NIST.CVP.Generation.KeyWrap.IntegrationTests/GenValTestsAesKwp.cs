using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Autofac;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using KeyWrap;
using NIST.CVP.Generation.KeyWrap.AES;
using AutofacConfig = KeyWrap.AutofacConfig;

namespace NIST.CVP.Generation.KeyWrap.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class GenValTestsAesKwp : GenValTestsBase
    {
        // ParameterValidator expects the algorithm to be "AES-KWP"
        public override string Algorithm { get; } = "AES-KWP";
        public override string Mode { get; } = "KeyWrap";

        public override Executable Generator => Program.Main;
        public override Executable Validator => KeyWrap_Val.Program.Main;

        [SetUp]
        public override void SetUp()
        {
            AdditionalParameters = new [] { "AES-KWP"};
            AutofacConfig.OverrideRegistrations = null;
            KeyWrap_Val.AutofacConfig.OverrideRegistrations = null;
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
            KeyWrap_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeException()
        {
            KeyWrap_Val.AutofacConfig.OverrideRegistrations = builder =>
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

                testCase.cipherText = bs.ToHex();
            }

            // If TC has a plainText, change it
            if (testCase.plainText != null)
            {
                BitString bs = new BitString(testCase.plainText.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.plainText = bs.ToHex();
            }
        }

        protected override string GetTestFileMinimalTestCases(string targetFolder)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Direction = new[] { "encrypt" },
                KwCipher = new[] { "cipher" },
                KeyLen = new[] { 128 },
                PtLen = new MathDomain()
                    .AddSegment(new ValueDomainSegment(8))
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KwCipher = ParameterValidator.VALID_KWCIPHER,
                KeyLen = new [] { 128 },
                PtLen = new MathDomain()
                    .AddSegment(new RangeDomainSegment(new Random800_90(), 8, 64, 8))
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KwCipher = ParameterValidator.VALID_KWCIPHER,
                KeyLen = ParameterValidator.VALID_KEY_SIZES,
                PtLen = new MathDomain()
                    .AddSegment(new RangeDomainSegment(new Random800_90(), 8, 4096, 8))
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
