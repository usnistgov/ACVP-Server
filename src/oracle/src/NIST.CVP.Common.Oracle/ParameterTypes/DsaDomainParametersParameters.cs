using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class DsaDomainParametersParameters
    {
        public HashFunction HashAlg { get; set; }
        public PrimeGenMode PQGenMode { get; set; }
        public GeneratorGenMode GGenMode { get; set; }
        public int L { get; set; }
        public int N { get; set; }
        public string Disposition { get; set; }
    }
}
