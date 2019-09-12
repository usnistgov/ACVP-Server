using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.KDF
{
    public class KdfParameterOneStep : IKdfParameter
    {
        /// <inheritdoc />
        public KasKdf KdfType => KasKdf.OneStep;
        /// <summary>
        /// The AuxFunction (hash or mac) to use with the KDF.
        /// </summary>
        public AuxFunction AuxFunction { get; set; }
        /// <summary>
        /// A salt value for use when the AuxFunction used is a MAC algorithm.
        /// </summary>
        public BitString Salt { get; set; }
        /// <summary>
        /// The shared secret for use in deriving a key.
        /// </summary>
        public BitString Z { get; set; }
        /// <summary>
        /// The length of the key to derive.
        /// </summary>
        public int L { get; set; }
        /// <summary>
        /// The pattern to use when constructing fixed info.
        /// </summary>
        public string FixedInfoPattern { get; set; }

        public KdfResult AcceptKdf(IKdfVisitor visitor, BitString fixedInfo)
        {
            return visitor.Kdf(this, fixedInfo);
        }
    }
}