﻿using Autofac;
using NIST.CVP.Generation.KeyWrap.TDES;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Generation.Core;
using NIST.CVP.Common;

namespace NIST.CVP.Generation.KeyWrap.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class GenValTestsTdes : GenValTestsSingleRunnerBase
    {
        // ParameterValidator expects the algorithm to be "TDES-KW"
        public override string Algorithm { get; } = "TDES-KW";
        public override string Mode { get; } = "KeyWrap";

        public override AlgoMode AlgoMode => AlgoMode.TDES_KW;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();
        public override IRegisterInjections RegistrationsCrypto => new Crypto.RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC is intended to be a failure test, change it
            if (testCase.testPassed != null)
            {
                testCase.testPassed = !(bool)testCase.testPassed;
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

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KwCipher = ParameterValidator.VALID_KWCIPHER,
                KeyLen = new[] { 192 },
                KeyingOption = new[] { 1, 2 },
                PtLen = new MathDomain()
                    .AddSegment(new RangeDomainSegment(new Random800_90(), 128, 512, 64))
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
                KeyLen = new[] { 192 },
                KeyingOption = ParameterValidator.VALID_KEYING_OPTIONS,
                PtLen = new MathDomain()
                    .AddSegment(new RangeDomainSegment(new Random800_90(), 128, 4096, 64))
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}