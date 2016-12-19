using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_ECB.Parsers
{
    public interface ILegacyResponseFileParser
    {
        ParseResponse<TestVectorSet> Parse(string path);
    }
}
