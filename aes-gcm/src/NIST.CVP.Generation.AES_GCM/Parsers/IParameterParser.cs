using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM.Parsers
{
    public interface IParameterParser
    {
        ParseResponse<Parameters> Parse(string path);
    }
}
