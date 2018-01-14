using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.MAC.CMAC.Enums;

namespace NIST.CVP.Generation.CMAC
{
    public static class AlgorithmSpecificationMapping
    {
        public static List<(string algoSpecification, CmacTypes mappedCmacType, int keySize)> Map =
            new List<(string algoSpecification, CmacTypes mappedCmacType, int keySize)>()
            {
                ("CMAC-AES-128", CmacTypes.AES128, 128),
                ("CMAC-AES-192", CmacTypes.AES192, 192),
                ("CMAC-AES-256", CmacTypes.AES256, 256),
                ("CMAC-TDES", CmacTypes.TDES, 192),
            };
    }
}
