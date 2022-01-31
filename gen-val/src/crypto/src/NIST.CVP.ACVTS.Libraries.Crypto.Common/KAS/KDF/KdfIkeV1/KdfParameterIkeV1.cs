using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfIkeV1
{
    public class KdfParameterIkeV1 : IKdfParameter
    {
        public Kda KdfType => Kda.Ike_v1;
        public bool RequiresAdditionalNoncePair => true;
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
        public BitString EntropyBits { get; set; }
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
        /// <summary>
        /// Additional nonce used by the initiator for the KDF.
        /// </summary>
        public BitString AdditionalInitiatorNonce { get; set; }
        /// <summary>
        /// Additional nonce used by the responder for the KDF.
        /// </summary>
        public BitString AdditionalResponderNonce { get; set; }
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
