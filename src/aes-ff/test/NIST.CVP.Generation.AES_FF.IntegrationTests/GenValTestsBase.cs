using NIST.CVP.Common;
using NIST.CVP.Generation.AES_FFX.v1_0.Base;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Generation.AES_FF.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public abstract class GenValTestsBase : GenValTestsSingleRunnerBase
    {
        public override string Mode { get; } = string.Empty;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            // If TC has a cipherText, change it
            if (testCase.ct != null)
            {
                var s = (string)testCase.ct.ToString();

                char[] charArray = s.ToCharArray();
                Array.Reverse(charArray);
                testCase.ct = new string(charArray);
            }

            // If TC has a plainText, change it
            if (testCase.pt != null)
            {
                var s = (string)testCase.pt.ToString();

                char[] charArray = s.ToCharArray();
                Array.Reverse(charArray);
                testCase.pt = new string(charArray);
            }
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                VectorSetId = 42,
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Direction = new string[] { "encrypt" },
                KeyLen = new int[] { ParameterValidator.VALID_KEY_SIZES.First() },
                IsSample = true,
                TweakLen = new MathDomain().AddSegment(new ValueDomainSegment(56)),
                Capabilities = new List<Capability>()
                {
                    new Capability()
                    {
                        Alphabet = "0123456789",
                        Radix = 10,
                        MinLen = 10,
                        MaxLen = 56
                    },
                    new Capability()
                    {
                        Alphabet = "abcdefghijklmnopqrstuvwxyz",
                        Radix = 26,
                        MinLen = 26,
                        MaxLen = 40
                    },
                }.ToArray()
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            Parameters p = new Parameters()
            {
                VectorSetId = 42,
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KeyLen = ParameterValidator.VALID_KEY_SIZES,
                IsSample = false,
                TweakLen = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 128, 8)),
                Capabilities = new List<Capability>()
                {
                    new Capability()
                    {
                        Alphabet = "0123456789",
                        Radix = 10,
                        MinLen = 10,
                        MaxLen = 56
                    },
                    new Capability()
                    {
                        Alphabet = "abcdefghijklmnopqrstuvwxyz",
                        Radix = 26,
                        MinLen = 10,
                        MaxLen = 40
                    },
                    new Capability()
                    {
                        Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz",
                        Radix = 62,
                        MinLen = 10,
                        MaxLen = 28
                    }
                }.ToArray()
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}