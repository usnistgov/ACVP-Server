﻿using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Crypto.Common.KAS.Helpers
{
    public static class ParameterSetDetails
    {
        public static readonly
            Dictionary<FfcParameterSet, (int pLength, int qLength, int minHashLength, int minMacKeyLength, int minMacLength)> 
            FfcDetails =
                new Dictionary<FfcParameterSet, (int pLength, int qLength, int minHashLength, int minMacKeyLength, int minMacLength)>()
                {
                    {FfcParameterSet.Fb, ( 2048, 224, 224, 112, 64 )},
                    {FfcParameterSet.Fc, ( 2048, 256, 256, 128, 64 )}
                };

        public static (int pLength, int qLength, int minHashLength, int minMacKeyLength, int minMacLength) 
            GetDetailsForFfcParameterSet(FfcParameterSet parameterSet)
        {
            return FfcDetails.First(w => w.Key == parameterSet).Value;
        }

        public static readonly
            Dictionary<EccParameterSet, (int minLengthN, int maxLengthN, int maxLengthH, int minHashLength, int minMacKeyLength, int minMacLength)>
            EccDetails =
                new Dictionary<EccParameterSet, (int minLengthN, int maxLengthN, int maxLengthH, int minHashLength, int minMacKeyLength, int minMacLength)>()
                {
                    {EccParameterSet.Eb, (224, 255, 14, 224, 112, 64)},
                    {EccParameterSet.Ec, (256, 383, 16, 256, 128, 64)},
                    {EccParameterSet.Ed, (384, 511, 24, 384, 192, 64)},
                    {EccParameterSet.Ee, (512, int.MaxValue, 32, 512, 256, 64)},
                };


        public static (int minLengthN, int maxLengthN, int maxLengthH, int minHashLength, int minMacKeyLength, int minMacLength) 
            GetDetailsForEccParameterSet(EccParameterSet parameterSet)
        {
            return EccDetails.First(w => w.Key == parameterSet).Value;
        }
        
        public static readonly 
            Dictionary<int, int>
                RsaModuloDetails = new Dictionary<int, int>()
                {
                    {2048, 112},
                    {3072, 128},
                    {4096, 152},
                    {6144, 176},
                    {8192, 200}
                };

        public static int GetDetailsForModulo(int modulo)
        {
            if (RsaModuloDetails.TryFirst(t => t.Key == modulo, out var result))
            {
                return result.Value;
            }

            throw new ArgumentException($"Invalid {nameof(modulo)}");
        }
    }
}
