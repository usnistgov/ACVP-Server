using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.SHA;

namespace NIST.CVP.Generation.SHA2
{
    public class LegacyResponseFileParser : ILegacyResponseFileParser<TestVectorSet>
    {
        public ParseResponse<TestVectorSet> Parse(string path)
        {
            return null;
        }

        private ModeValues ExtractMode(string line)
        {
            if (line.Contains("SHA-1"))
            {
                return ModeValues.SHA1;
            }
            else
            {
                return ModeValues.SHA2;
            }
        }

        private DigestSizes ExtractDigestSize(string line)
        {
            if (line.Contains("SHA-224 "))
            {
                return DigestSizes.d224;
            }
            else if (line.Contains("SHA-256 "))
            {
                return DigestSizes.d256;
            }
            else if (line.Contains("SHA-384 "))
            {
                return DigestSizes.d384;
            }
            else if (line.Contains("SHA-512 "))
            {
                return DigestSizes.d512;
            }
            else if (line.Contains("SHA-512/224"))
            {
                return DigestSizes.d512t224;
            }
            else if (line.Contains("SHA-512/256"))
            {
                return DigestSizes.d512t256;
            }
            else
            {
                return DigestSizes.d160;
            }
        }

        private string ExtractType(string line)
        {
            if (line.Contains("ShortMsg"))
            {
                return "short";
            }

            if (line.Contains("LongMsg"))
            {
                return "long";
            }

            if (line.Contains("Monte"))
            {
                return "montecarlo";
            }

            return null;
        }
    }
}
