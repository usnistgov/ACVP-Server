using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_ECB.GenVal
{
    
    public class AlgoFileFinder : AlgoFileFinderBase
    {
        public override string Name { get { return "AES"; } }
        public override string FilePrefix { get { return "ECB"; } }
    }
}
