using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.AES_FFX.v1_0.Base;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_FF.IntegrationTests
{
    public class GenValTestsFf1 : GenValTestsBase
    {
        public override AlgoMode AlgoMode => AlgoMode.AES_FF1_v1_0;
        public override string Algorithm => "ACVP-AES-FF1";


        protected override Parameters GetParametersForLotsOfTestCases()
        {
            return new Parameters
            {
                VectorSetId = 42,
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Direction = ParameterValidator.VALID_DIRECTIONS,
                KeyLen = ParameterValidator.VALID_KEY_SIZES,
                IsSample = false,
                TweakLen = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 128, 8)),
                Capabilities = new List<Capability>
                {
                    new Capability
                    {
                        Alphabet = "ab",
                        Radix = 2,
                        MinLen = 100,
                        MaxLen = 512
                    },
                    new Capability
                    {
                        Alphabet = "0123",
                        Radix = 4,
                        MinLen = 10,
                        MaxLen = 256
                    },
                    new Capability
                    {
                        Alphabet = "0123456789abcdef",
                        Radix = 16,
                        MinLen = 10,
                        MaxLen = 128
                    },
                    new Capability
                    {
                        Alphabet = "012345abcdefghijklmnopqrstuvwxyz",
                        Radix = 32,
                        MinLen = 10,
                        MaxLen = 256
                    },
                    new Capability
                    {
                        Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz+/",
                        Radix = 64,
                        MinLen = 10,
                        MaxLen = 28
                    }
                }.ToArray()
            };
        }
    }
}
