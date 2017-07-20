using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.DRBG.Enums;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.DRBG.Tests.Fakes
{
    public class FakeDrbgImplementation : DrbgBase
    {
        public DrbgStatus DrbgStatus { get; set; }
        public DrbgResult DrbgResult { get; set; } = new DrbgResult(new BitString(0));

        public void SetMaxPersonalizationStringLength(int length)
        {
            MaxPersonalizationStringLength = length;
        }

        public void SetMaxNumberOfBitsPerRequest(int length)
        {
            MaxNumberOfBitsPerRequest = length;
        }

        public void SetMaxAdditionalInput(int length)
        {
            MaxAdditionalInputLength = length;
        }

        public FakeDrbgImplementation(IEntropyProvider entropyProvider, DrbgParameters drbgParameters) : base(entropyProvider, drbgParameters)
        {
        }

        protected override void SetSecurityStrengths(int requestedSecurityStrength)
        {
            
        }

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
