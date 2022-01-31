using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
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
