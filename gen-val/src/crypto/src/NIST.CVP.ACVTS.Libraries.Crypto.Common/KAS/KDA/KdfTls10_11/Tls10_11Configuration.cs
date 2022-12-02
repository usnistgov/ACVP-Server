using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTls10_11
{
    public class Tls10_11Configuration : IKdfConfiguration
    {
        public Kda KdfType => Kda.Tls_v10_v11;
        public bool RequiresAdditionalNoncePair => true;
        public int L { get; set; }
        public int SaltLen { get; set; }
        public MacSaltMethod SaltMethod { get; set; }
        public string FixedInfoPattern { get; set; }
        public FixedInfoEncoding FixedInfoEncoding { get; set; }
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
