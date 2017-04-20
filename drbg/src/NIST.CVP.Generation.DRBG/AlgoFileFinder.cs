using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_ECB
{
    
    public class AlgoFileFinder : AlgoFileFinderBase
    {
        public override string Name => "DRBG";
        public override string FilePrefix => "ctr";
    }
}
