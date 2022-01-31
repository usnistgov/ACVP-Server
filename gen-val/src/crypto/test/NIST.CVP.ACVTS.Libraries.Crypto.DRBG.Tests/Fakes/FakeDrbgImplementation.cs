using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.DRBG.Tests.Fakes
{
    public class FakeDrbgImplementation : DrbgBase
    {
        public FakeDrbgImplementation(IEntropyProvider entropyProvider, DrbgParameters drbgParameters) : base(entropyProvider, drbgParameters)
        {
            var att = DrbgAttributesHelper.GetDrbgAttributes(drbgParameters.Mechanism, drbgParameters.Mode, drbgParameters.DerFuncEnabled);
            var intMax = Int32.MaxValue - 1;

            Attributes = new DrbgAttributes
                         (
                            att.Mechanism,
                            att.Mode,
                            att.MaxSecurityStrength,
                            att.MinEntropyInputLength,
                            intMax,
                            intMax,
                            intMax,
                            intMax,
                            intMax,
                            att.MinNonceLength,
                            intMax
                        );

        }

        public DrbgStatus DrbgStatus { get; set; }
        public DrbgResult DrbgResult { get; set; } = new DrbgResult(new BitString(0));

        protected override DrbgStatus InstantiateAlgorithm(BitString entropyInput, BitString nonce, BitString personalizationString)
        {
            return DrbgStatus;
        }

        protected override DrbgStatus ReseedAlgorithm(BitString entropyInput, BitString additionalInput)
        {
            return DrbgStatus;
        }

        protected override DrbgResult GenerateAlgorithm(int requestedNumberOfBits, BitString additionalInput)
        {
            return DrbgResult;
        }
    }
}
