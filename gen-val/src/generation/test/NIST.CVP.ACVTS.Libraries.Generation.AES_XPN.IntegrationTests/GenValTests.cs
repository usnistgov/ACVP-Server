﻿using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.AES_XPN.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_XPN.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "ACVP-AES-XPN";
        public override string Mode { get; } = null;

        public override AlgoMode AlgoMode => AlgoMode.AES_XPN_v1_0;


        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            // If TC is intended to be a failure test, change it
            if (testCase.testPassed != null)
            {
                testCase.testPassed = true;
            }

            // If TC has a cipherText, change it
            if (testCase.ct != null)
            {
                BitString bs = new BitString(testCase.ct.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.ct = bs.ToHex();
            }

            // If TC has a tag, change it
            if (testCase.tag != null)
            {
                BitString bs = new BitString(testCase.tag.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.tag = bs.ToHex();
            }

            // If TC has a plainText, change it
            if (testCase.pt != null)
            {
                BitString bs = new BitString(testCase.pt.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.pt = bs.ToHex();
            }
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            Parameters p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KeyLen = new[] { ParameterValidator.VALID_KEY_SIZES.First() },
                PayloadLen = new MathDomain().AddSegment(new ValueDomainSegment(0)),
                IvGen = ParameterValidator.VALID_IV_GEN[0],
                IvGenMode = ParameterValidator.VALID_IV_GEN_MODE[0],
                SaltGen = ParameterValidator.VALID_SALT_GEN[1],
                AadLen = new MathDomain().AddSegment(new ValueDomainSegment(0)),
                TagLen = new[] { 64 },
                IsSample = true
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            Parameters p = new Parameters
            {
                Algorithm = Algorithm,
                Revision = Revision,
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KeyLen = ParameterValidator.VALID_KEY_SIZES,
                PayloadLen = new MathDomain()
                    .AddSegment(new ValueDomainSegment(128))
                    .AddSegment(new ValueDomainSegment(256)),
                IvGen = ParameterValidator.VALID_IV_GEN[1],
                IvGenMode = ParameterValidator.VALID_IV_GEN_MODE[1],
                SaltGen = ParameterValidator.VALID_SALT_GEN[1],
                AadLen = new MathDomain()
                    .AddSegment(new ValueDomainSegment(128))
                    .AddSegment(new ValueDomainSegment(120)),
                TagLen = new[] { 64, 128 },
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
