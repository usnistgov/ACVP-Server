using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Enums;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Xmss
{
    public class XmssKeyPairParameters : IParameters
    {
        public XmssMode XmssMode { get; init; }

        public override bool Equals(object other)
        {
            if (other is XmssKeyPairParameters p)
            {
                return GetHashCode() == p.GetHashCode();
            }

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(XmssMode);
    }
}
