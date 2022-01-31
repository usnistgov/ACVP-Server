using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
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
