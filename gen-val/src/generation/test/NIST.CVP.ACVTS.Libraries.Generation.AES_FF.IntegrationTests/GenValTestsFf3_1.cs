using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.AES_FFX.v1_0.Base;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_FF.IntegrationTests
{
    public class GenValTestsFf3_1 : GenValTestsBase
    {
        public override AlgoMode AlgoMode => AlgoMode.AES_FF1_v1_0;
        public override string Algorithm => "ACVP-AES-FF3-1";

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
                    new Capability { Alphabet = "0123456789", Radix = 10, MinLen = 10, MaxLen = 56 },
                    new Capability { Alphabet = "abcdefghijklmnopqrstuvwxyz", Radix = 26, MinLen = 10, MaxLen = 40 },
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
