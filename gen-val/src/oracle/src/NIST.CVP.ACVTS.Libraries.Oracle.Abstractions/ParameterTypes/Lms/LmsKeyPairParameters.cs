using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Lms
{
    public class LmsKeyPairParameters : IParameters
    {
        public LmsMode LmsMode { get; init; }
        public LmOtsMode LmOtsMode { get; init; }
        
        public override bool Equals(object other)
        {
            if (other is LmsKeyPairParameters p)
            {
                return GetHashCode() == p.GetHashCode();
            }

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(LmsMode, LmOtsMode);
    }
}
