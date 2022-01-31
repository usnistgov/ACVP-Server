using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.IKEv1.Enums;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class IkeV1KdfParameters
    {
        public AuthenticationMethods AuthenticationMethod { get; set; }
        public int NInitLength { get; set; }
        public int NRespLength { get; set; }
        public int GxyLength { get; set; }
        public int PreSharedKeyLength { get; set; }
        public HashFunction HashAlg { get; set; }
    }
}
