using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme
{
    public class KasAlgoAttributesIfc : IKasAlgoAttributes
    {
        public KasAlgoAttributesIfc(IfcScheme scheme, int modulo, int l)
        {
            Scheme = scheme;
            Modulo = modulo;
            L = l;
        }

        /// <summary>
        /// The scheme being utilized.
        /// </summary>
        public IfcScheme Scheme { get; }
        /// <summary>
        /// The modulo used.
        /// </summary>
        public int Modulo { get; }
        /// <summary>
        /// The length of the wrapped/established key.
        /// </summary>
        public int L { get; }
    }
}
