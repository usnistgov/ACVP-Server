using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using System;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class EcdsaKeyParameters : IParameters
    {
        public Curve Curve { get; set; }
        public EcdsaKeyDisposition Disposition { get; set; }

        public override bool Equals(object other)
        {
            if (other is EcdsaKeyParameters p)
            {
                return GetHashCode() == p.GetHashCode();
            }

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(Curve, Disposition);
    }
}
