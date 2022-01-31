using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfHkdf
{
    public class KdfParameterHkdf : IKdfParameter
    {
        public Kda KdfType => Kda.Hkdf;
        public bool RequiresAdditionalNoncePair => false;
        public BitString Salt { get; set; }
        public BitString Iv { get; set; }
        public BitString T { get; set; }
        public BitString Z { get; set; }
        public int L { get; set; }
        public string FixedInfoPattern { get; set; }
        public FixedInfoEncoding FixedInputEncoding { get; set; }
        public BitString AlgorithmId { get; set; }
        public BitString Label { get; set; }
        public BitString Context { get; set; }
        public BitString AdditionalInitiatorNonce { get; set; }
        public BitString AdditionalResponderNonce { get; set; }
        public BitString EntropyBits { get; set; }
        public HashFunctions HmacAlg { get; set; }
        public KdfResult AcceptKdf(IKdfVisitor visitor, BitString fixedInfo)
        {
            return visitor.Kdf(this, fixedInfo);
        }

        public void SetEphemeralData(BitString initiatorData, BitString responderData)
        {
            // Not used for this KDF type
        }
    }
}
