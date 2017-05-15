using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KeyWrap
{
    public class AlgoFileFinder : AlgoFileFinderBase
    {
        public override string Name => "KW-AES";
        public override string FilePrefix => "KW";
    }
}
