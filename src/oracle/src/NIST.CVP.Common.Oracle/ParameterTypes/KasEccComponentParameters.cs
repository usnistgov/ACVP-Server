using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using System.Numerics;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class KasEccComponentParameters
    {
        public Curve Curve { get; set; }
        
        public bool IsSample { get; set; }
    }
}