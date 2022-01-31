using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.SSH.Enums;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class SshKdfParameters
    {
        public HashFunction HashAlg { get; set; }
        public Cipher Cipher { get; set; }
    }
}
