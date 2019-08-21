using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep
{
    public class AuxFunction
    {
        /// <summary>
        /// The Hash or Mac function name
        /// </summary>
        public KasKdfOneStepAuxFunction AuxFunctionName { get; set; }
        /// <summary>
        /// SaltLen applies to MAC based aux functions.
        /// </summary>
        public int SaltLen { get; set; }
        /// <summary>
        /// The salting methods used for the KDF (hashes do not require salts, MACs do)
        /// </summary>
        public MacSaltMethod MacSaltMethod { get; set; }
        /// <summary>
        /// The pattern used for FixedInputConstruction.
        /// </summary>
        public string FixedInputPattern { get; set; }
    }
}