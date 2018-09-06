using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KeyWrap.TDES
{
    //TODO: This already exists in TDES_ECB generation. It would be good to use that as well as the AES one

    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };
        public int[] KeyLen { get; set; }
        public string[] Direction { get; set; }
        public string[] KwCipher { get; set; }
        public MathDomain PtLen { get; set; }
        /// <summary>
        /// Keying Option 1 - 3 independant key TDES
        /// Keying Option 2 - 2 Key TDES
        /// Keying Option 3 (No longer supported) - 1 Key TDES - only used in KATs
        /// </summary>
        public int[] KeyingOption { get; set; }

    }
}
