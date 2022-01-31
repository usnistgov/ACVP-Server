using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.CTS;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.CTS
{
    public class CiphertextStealingTransformFactory : ICiphertextStealingTransformFactory
    {
        public ICiphertextStealingTransform Get(CiphertextStealingMode mode)
        {
            switch (mode)
            {
                case CiphertextStealingMode.CS1:
                    return new CiphertextStealingMode1();

                case CiphertextStealingMode.CS2:
                    return new CiphertextStealingMode2();

                case CiphertextStealingMode.CS3:
                    return new CiphertextStealingMode3();
            }

            throw new ArgumentException($"Invalid {nameof(mode)} ({mode})");
        }
    }
}
