using NIST.CVP.Common;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using System;
using System.Collections.Generic;

namespace NIST.CVP.Crypto.Common.Symmetric.Helpers
{
    public static class AlgoModeToEngineModeOfOperationMapping
    {
        public static List<(AlgoMode algoMode, BlockCipherEngines engine, BlockCipherModesOfOperation mode)> Mappings =
            new List<(AlgoMode algoMode, BlockCipherEngines engine, BlockCipherModesOfOperation mode)>()
            {
                (AlgoMode.AES_CBC_v1_0, BlockCipherEngines.Aes, BlockCipherModesOfOperation.Cbc),
                (AlgoMode.AES_CCM_v1_0, BlockCipherEngines.Aes, BlockCipherModesOfOperation.Ccm),
                (AlgoMode.AES_CFB1_v1_0, BlockCipherEngines.Aes, BlockCipherModesOfOperation.CfbBit),
                (AlgoMode.AES_CFB8_v1_0, BlockCipherEngines.Aes, BlockCipherModesOfOperation.CfbByte),
                (AlgoMode.AES_CFB128_v1_0, BlockCipherEngines.Aes, BlockCipherModesOfOperation.CfbBlock),
                (AlgoMode.AES_CTR_v1_0, BlockCipherEngines.Aes, BlockCipherModesOfOperation.Ctr),
                (AlgoMode.AES_ECB_v1_0, BlockCipherEngines.Aes, BlockCipherModesOfOperation.Ecb),
                (AlgoMode.AES_GCM_v1_0, BlockCipherEngines.Aes, BlockCipherModesOfOperation.Gcm),
                (AlgoMode.AES_OFB_v1_0, BlockCipherEngines.Aes, BlockCipherModesOfOperation.Ofb),
                (AlgoMode.AES_XPN_v1_0, BlockCipherEngines.Aes, BlockCipherModesOfOperation.Gcm),
                
                (AlgoMode.TDES_CBC_v1_0, BlockCipherEngines.Tdes, BlockCipherModesOfOperation.Cbc),
                (AlgoMode.TDES_CBCI_v1_0, BlockCipherEngines.Tdes, BlockCipherModesOfOperation.Cbci),
                (AlgoMode.TDES_CFB1_v1_0, BlockCipherEngines.Tdes, BlockCipherModesOfOperation.CfbBit),
                (AlgoMode.TDES_CFB8_v1_0, BlockCipherEngines.Tdes, BlockCipherModesOfOperation.CfbByte),
                (AlgoMode.TDES_CFB64_v1_0, BlockCipherEngines.Tdes, BlockCipherModesOfOperation.CfbBlock),
                (AlgoMode.TDES_CFBP1_v1_0, BlockCipherEngines.Tdes, BlockCipherModesOfOperation.CfbpBit),
                (AlgoMode.TDES_CFBP8_v1_0, BlockCipherEngines.Tdes, BlockCipherModesOfOperation.CfbpByte),
                (AlgoMode.TDES_CFBP64_v1_0, BlockCipherEngines.Tdes, BlockCipherModesOfOperation.CfbpBlock),
                (AlgoMode.TDES_CTR_v1_0, BlockCipherEngines.Tdes, BlockCipherModesOfOperation.Ctr),
                (AlgoMode.TDES_ECB_v1_0, BlockCipherEngines.Tdes, BlockCipherModesOfOperation.Ecb),
                (AlgoMode.TDES_OFB_v1_0, BlockCipherEngines.Tdes, BlockCipherModesOfOperation.Ofb),
                (AlgoMode.TDES_OFBI_v1_0, BlockCipherEngines.Tdes, BlockCipherModesOfOperation.Ofbi)
            };

        public static (BlockCipherEngines engine, BlockCipherModesOfOperation mode) GetMapping(AlgoMode algoMode)
        {
            if (!Mappings.TryFirst(w => w.algoMode == algoMode, out var result))
            {
                throw new ArgumentException($"Invalid {nameof(algoMode)}");
            }

            return (result.engine, result.mode);
        }

        public static AlgoMode GetMapping(BlockCipherEngines engine, BlockCipherModesOfOperation mode)
        {
            if (!Mappings.TryFirst(w => w.engine == engine && w.mode == mode, out var result))
            {
                throw new ArgumentException($"Invalid {nameof(engine)} and {nameof(mode)} combination");
            }

            return result.algoMode;
        }
    }
}