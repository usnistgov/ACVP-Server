using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DRBG
{
    
    public class AlgoFileFinder : AlgoFileFinderBase
    {
        public override string Name => "DRBG";
        public override string FilePrefix => "ctr";
    }
}
