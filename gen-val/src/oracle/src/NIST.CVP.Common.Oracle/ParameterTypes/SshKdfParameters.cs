using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF.Components.SSH.Enums;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class SshKdfParameters
    {
        public HashFunction HashAlg { get; set; }
        public Cipher Cipher { get; set; }
    }
}
