using NIST.CVP.Crypto.Common.KAS.Enums;
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
        public BitString Salt { get; }
        /// <summary>
        /// The shared secret for use in deriving a key.
        /// </summary>
        public BitString Z { get; }
        /// <summary>
        /// The length of the key to derive.
        /// </summary>
        public int L { get; }
        /// <summary>
        /// The context binding information for the derived key.
        /// </summary>
        public BitString FixedInfo { get; }

        /// <summary>
        /// Construct a KDF Parameter utilizing an AuxFunction of MAC
        /// </summary>
        /// <param name="auxFunction"></param>
        /// <param name="z"></param>
        /// <param name="l"></param>
        /// <param name="fixedInfo"></param>
        /// /// <param name="salt"></param>
        public KdfParameterOneStep(AuxFunction auxFunction, BitString z, int l, BitString fixedInfo, BitString salt )
            : this (auxFunction, z, l, fixedInfo)
        {
            Salt = salt;
        }
        
        /// <summary>
        /// Construct a KDF Parameter utilizing an AuxFunction of Hash
        /// </summary>
        /// <param name="auxFunction"></param>
        /// <param name="z"></param>
        /// <param name="l"></param>
        /// <param name="fixedInfo"></param>
        public KdfParameterOneStep(AuxFunction auxFunction, BitString z, int l, BitString fixedInfo)
        {
            AuxFunction = auxFunction;
            Z = z;
            L = l;
            FixedInfo = fixedInfo;
        }

        public KdfResult AcceptKdf(IKdfVisitor visitor)
        {
            return visitor.Kdf(this);
        }
    }
}