using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Crypto.Common.KAS.KDF.KdfIkeV2
{
    public class IkeV2Configuration : IKdfConfiguration
    {
        public KasKdf KdfType => KasKdf.Ike_v2;
        public bool RequiresAdditionalNoncePair => true;
        public int L { get; set; }
        public int SaltLen { get; set; }
        public MacSaltMethod SaltMethod { get; set; }
        public string FixedInfoPattern { get; set; }
        public FixedInfoEncoding FixedInfoEncoding { get; set; }
        /// <summary>
        /// The hash function to be used by the KDF.
        /// </summary>
        public HashFunctions HashFunction { get; set; }
        /// <summary>
        /// Should the initiator additional nonce be generated?
        /// </summary>
        public bool ServerGenerateInitiatorAdditionalNonce { get; set; }
        /// <summary>
        /// Should the responder additional nonce be generated?
        /// </summary>
        public bool ServerGenerateResponderAdditionalNonce { get; set; }
        public IKdfParameter GetKdfParameter(IKdfParameterVisitor visitor)
        {
            return visitor.CreateParameter(this);
        }
    }
}