using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep
{
    public class OneStepConfiguration : IKdfConfiguration
    {
        public KasKdf KdfType => KasKdf.OneStep;
        public bool RequiresAdditionalNoncePair => false;
        public int L { get; set; }
        public int SaltLen { get; set; }
        public MacSaltMethod SaltMethod { get; set; }
        public string FixedInfoPattern { get; set; }
        public FixedInfoEncoding FixedInfoEncoding { get; set; }
        /// <summary>
        /// The Hash or MAC functions utilized for the KDF
        /// </summary>
        public KasKdfOneStepAuxFunction AuxFunction { get; set; }
        public IKdfParameter GetKdfParameter(IKdfParameterVisitor visitor)
        {
            return visitor.CreateParameter(this);
        }
    }
}