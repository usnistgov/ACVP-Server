using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfHkdf
{
    public class HkdfMultiExpansionConfiguration : IKdfMultiExpansionConfiguration
    {
        public Kda KdfType => Kda.Hkdf;
        public int L { get; set; }
        public int SaltLen { get; set; }
        public MacSaltMethod SaltMethod { get; set; }
        public HashFunctions HmacAlg { get; set; }
        public IKdfMultiExpansionParameter GetKdfParameter(IKdfMultiExpansionParameterVisitor visitor)
        {
            return visitor.CreateParameter(this);
        }
    }
}
