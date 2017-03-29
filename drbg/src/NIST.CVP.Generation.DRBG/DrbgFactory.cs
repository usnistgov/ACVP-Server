using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.AES_ECB;
using NIST.CVP.Generation.DRBG.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DRBG
{
    public class DrbgFactory : IDrbgFactory
    {
        private readonly IAES_ECB _aesEcb;
        private readonly IEntropyProvider _entropyProvider;

        public DrbgFactory(IEntropyProvider entropyProvider, IAES_ECB aesEcb)
        {
            _entropyProvider = entropyProvider;
            _aesEcb = aesEcb;
        }

        public IDrbg GetDrbgInstance(DrbgParameters drbgParameters)
        {
            switch (drbgParameters.Mechanism)
            {
                case DrbgMechanism.Counter:
                    return GetCounterImplementation(drbgParameters);
                default:
                    throw new ArgumentException("Invalid DRBG Mechanmism provided");
            }
        }

        private IDrbg GetCounterImplementation(DrbgParameters drbgParameters)
        {
            switch (drbgParameters.Mode)
            {
                case DrbgMode.AES128:
                    return new DrbgCounterAes(_entropyProvider, _aesEcb, drbgParameters, 128);
                case DrbgMode.AES192:
                    return new DrbgCounterAes(_entropyProvider, _aesEcb, drbgParameters, 192);
                case DrbgMode.AES256:
                    return new DrbgCounterAes(_entropyProvider, _aesEcb, drbgParameters, 256);
                default:
                    throw new ArgumentException("Invalid DRBG mode provided for current mechanism");
            }
        }
    }
}
