﻿using System.Linq;
using NIST.CVP.Common;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CCM.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "AES";
        public override string Mode { get; } = "CCM";

        public override AlgoMode AlgoMode => AlgoMode.AES_CCM;

        public override IRegisterInjections RegistrationsCrypto => new Crypto.RegisterInjections();

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

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.plainText = bs.ToHex();
            }
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            MathDomain ptDomain = new MathDomain();
            ptDomain.AddSegment(new ValueDomainSegment(0));

            MathDomain aadDomain = new MathDomain();
            aadDomain.AddSegment(new ValueDomainSegment(8));

            MathDomain tagDomain = new MathDomain();
            tagDomain.AddSegment(new ValueDomainSegment(ParameterValidator.VALID_TAG_LENGTHS.First()));

            MathDomain nonceDomain = new MathDomain();
            nonceDomain.AddSegment(new ValueDomainSegment(ParameterValidator.VALID_NONCE_LENGTHS.First()));

            Parameters p = new Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                KeyLen = new int[] { ParameterValidator.VALID_KEY_SIZES.First() },
                PtLen = ptDomain,
                AadLen = aadDomain,
                TagLen = tagDomain,
                IvLen = nonceDomain,
                IsSample = true
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            Random800_90 random = new Random800_90();

            MathDomain ptDomain = new MathDomain();
            ptDomain.AddSegment(new RangeDomainSegment(random, 0, 32*8, 8 ));

            MathDomain aadDomain = new MathDomain();
            aadDomain.AddSegment(new RangeDomainSegment(random, 0, (1 << 19), 8));

            MathDomain tagDomain = new MathDomain();
            foreach (var length in ParameterValidator.VALID_TAG_LENGTHS)
            {
                tagDomain.AddSegment(new ValueDomainSegment(length));
            }

            MathDomain nonceDomain = new MathDomain();
            foreach (var length in ParameterValidator.VALID_NONCE_LENGTHS)
            {
                nonceDomain.AddSegment(new ValueDomainSegment(length));
            }

            Parameters p = new Parameters()
            {
                Algorithm = Algorithm,
                Mode = Mode,
                KeyLen = ParameterValidator.VALID_KEY_SIZES,
                PtLen = ptDomain,
                AadLen = aadDomain,
                TagLen = tagDomain,
                IvLen = nonceDomain,
                IsSample = false
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}