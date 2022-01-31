using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;

namespace NIST.CVP.ACVTS.Generation.GenValApp.Models
{
    /// <summary>
    /// The GenValAppRunner's current running options
    /// </summary>
    public class GenValRunningOptions
    {
        public AlgoMode AlgoMode { get; }
        public GenValMode GenValMode { get; }

        public GenValRunningOptions(AlgoMode algoMode, GenValMode genValMode)
        {
            AlgoMode = algoMode;
            GenValMode = genValMode;
        }
    }
}