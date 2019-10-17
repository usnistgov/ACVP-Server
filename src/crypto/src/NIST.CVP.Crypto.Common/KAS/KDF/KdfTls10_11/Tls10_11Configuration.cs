using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Crypto.Common.KAS.KDF.KdfTls10_11
{
    public class Tls10_11Configuration : IKdfConfiguration
    {
        public KasKdf KdfType => KasKdf.Tls_v10_v11;
        public bool RequiresAdditionalNoncePair => true;
        public int L { get; set; }
        public int SaltLen { get; set; }
        public MacSaltMethod SaltMethod { get; set; }
        public string FixedInputPattern { get; set; }
        public FixedInfoEncoding FixedInputEncoding { get; set; }
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