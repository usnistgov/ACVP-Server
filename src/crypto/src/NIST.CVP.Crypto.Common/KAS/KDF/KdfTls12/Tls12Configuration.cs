using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Crypto.Common.KAS.KDF.KdfTls12
{
    public class Tls12Configuration : IKdfConfiguration
    {
        public KasKdf KdfType => KasKdf.Tls_v12;
        public bool RequiresAdditionalNoncePair => true;
        public int L { get; set; }
        public int SaltLen { get; set; }
        public MacSaltMethod SaltMethod { get; set; }
        public string FixedInfoPattern { get; set; }
        public FixedInfoEncoding FixedInfoEncoding { get; set; }
        /// <summary>
        /// The hash function in use for the KDF.
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