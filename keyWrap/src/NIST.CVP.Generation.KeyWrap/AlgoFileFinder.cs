using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KeyWrap
{
    public class AlgoFileFinder : AlgoFileFinderBase
    {
        public override string Name => "AES-KW";
        public override string FilePrefix => "KW";
    }
}
