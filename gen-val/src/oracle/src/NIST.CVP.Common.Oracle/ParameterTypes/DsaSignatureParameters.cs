using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class DsaSignatureParameters
    {
        public HashFunction HashAlg { get; set; }
        public FfcDomainParameters DomainParameters { get; set; }
        public int MessageLength { get; set; }
        public FfcKeyPair Key { get; set; }
        public DsaSignatureDisposition Disposition { get; set; }
        public bool IsMessageRandomized { get; set; }
    }
}
