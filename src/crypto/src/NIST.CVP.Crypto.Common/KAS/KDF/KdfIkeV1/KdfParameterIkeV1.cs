using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.KDF.KdfIkeV1
{
    public class KdfParameterIkeV1 : IKdfParameter
    {
        public KasKdf KdfType => KasKdf.Ike_v1;
        public BitString Salt { get; set; }
        public BitString Iv { get; set; }
        public BitString Z { get; set; }
        public int L { get; set; }
        public string FixedInfoPattern { get; set; }
        public FixedInfoEncoding FixedInputEncoding { get; set; }
        public BitString AlgorithmId { get; set; }
        public BitString Label { get; set; }
        public BitString Context { get; set; }
        /// <summary>
        /// The HashFunction used by the KDF.
        /// </summary>
        public HashFunctions HashFunction { get; set; }
        /// <summary>
        /// The ephemeral data (Either a C value or Nonce) from the initiator.
        /// </summary>
        public BitString InitiatorEphemeralData { get; private set; }
        /// <summary>
        /// The ephemeral data (Either a C value or Nonce) from the responder.
        /// </summary>
        public BitString ResponderEphemeralData { get; private set; }
        public KdfResult AcceptKdf(IKdfVisitor visitor, BitString fixedInfo)
        {
            return visitor.Kdf(this);
        }

        public void SetEphemeralData(BitString initiatorData, BitString responderData)
        {
            InitiatorEphemeralData = initiatorData;
            ResponderEphemeralData = responderData;
        }
    }
}