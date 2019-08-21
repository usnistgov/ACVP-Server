using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep
{
    public class OneStepConfiguration : IKasKdfConfiguration
    {
        public KasKdf KdfType => KasKdf.OneStep;
        
        /// <summary>
        /// The Hash or MAC functions utilized for the KDF
        /// </summary>
        public AuxFunction AuxFunction { get; set; }
        /// <summary>
        /// The encoding type of the fixedInput (used in One step KDF)
        /// </summary>
        public KasKdfOneStepEncoding Encoding { get; set; }
    }
}