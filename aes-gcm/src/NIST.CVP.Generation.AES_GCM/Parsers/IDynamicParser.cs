using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM.Parsers
{
    public interface IDynamicParser
    {
        ParseResponse<object> Parse(string path);
    }
}