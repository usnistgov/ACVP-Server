using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.CTR.Enums;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Crypto.CTR.Helpers
{
    public static class AlgorithmSpecificationToDomainMapping
    {
        public static List<(string specificationAlgorithm, Cipher cipher, int blockSize)> Mapping =
            new List<(string specificationAlgorithm, Cipher cipher, int blockSize)>
            {
                ("AES", Cipher.AES, 128),
                ("TDES", Cipher.TDES, 64)
            };

        public static (string specificationAlgorithm, Cipher cipher, int blockSize) GetMappingFromAlgorithm(string algorithm)
        {
            if (!Mapping
                .TryFirst(
                    w => w.specificationAlgorithm.Equals(algorithm, StringComparison.OrdinalIgnoreCase),
                    out var result))
            {
                throw new ArgumentException("Invalid Algorithm provided.");
            }

            return result;
        }

        public static (string specificationAlgorithm, Cipher cipher, int blockSize) GetMappingFromAlgorithm(Cipher algorithm)
        {
            if (!Mapping
                .TryFirst(
                    w => w.cipher.Equals(algorithm),
                    out var result))
            {
                throw new ArgumentException("Invalid Algorithm provided.");
            }

            return result;
        }
    }
}
