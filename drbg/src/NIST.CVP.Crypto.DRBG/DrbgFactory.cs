using System;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_ECB;
using NIST.CVP.Crypto.DRBG.Enums;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.DRBG
{
    public class DrbgFactory : IDrbgFactory
    {
        public IDrbg GetDrbgInstance(DrbgParameters drbgParameters, IEntropyProvider iEntropyProvider)
        {
            switch (drbgParameters.Mechanism)
            {
                case DrbgMechanism.Counter:
                    return GetCounterImplementation(drbgParameters, iEntropyProvider);
                default:
                    throw new ArgumentException("Invalid DRBG Mechanmism provided");
            }
        }

        private IDrbg GetCounterImplementation(DrbgParameters drbgParameters, IEntropyProvider iEntropyProvider)
        {
            AES_ECB.AES_ECB aesEcb = new AES_ECB.AES_ECB(new RijndaelFactory(new RijndaelInternals()));

            switch (drbgParameters.Mode)
            {
                case DrbgMode.AES128:
                    return new DrbgCounterAes(iEntropyProvider, aesEcb, drbgParameters, 128);
                case DrbgMode.AES192:
                    return new DrbgCounterAes(iEntropyProvider, aesEcb, drbgParameters, 192);
                case DrbgMode.AES256:
                    return new DrbgCounterAes(iEntropyProvider, aesEcb, drbgParameters, 256);
                default:
                    throw new ArgumentException("Invalid DRBG mode provided for current mechanism");
            }
        }
    }
}
