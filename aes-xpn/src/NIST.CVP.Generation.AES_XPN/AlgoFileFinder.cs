using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_XPN
{
    //@@@extract goodies to base class for re-use by all algos
    public class AlgoFileFinder : AlgoFileFinderBase
    {
        public override string Name { get { return "AES_XPN"; } }
    }
}
