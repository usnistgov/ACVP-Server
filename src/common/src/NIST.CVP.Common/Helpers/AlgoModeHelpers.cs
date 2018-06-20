using System.Text;

namespace NIST.CVP.Common.Helpers
{
    public static class AlgoModeHelpers
    {
        /// <summary>
        /// Find the AlgoMode for a given algorithm/mode
        /// </summary>
        /// <param name="algorithm">The algorithm</param>
        /// <param name="mode">The mode</param>
        /// <returns></returns>
        public static AlgoMode GetAlgoModeFromAlgoAndMode(string algorithm, string mode)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(algorithm);
            if (!string.IsNullOrEmpty(mode))
            {
                sb.Append("-");
                sb.Append(mode);
            }

            return EnumHelpers.GetEnumFromEnumDescription<AlgoMode>(sb.ToString());
        }
    }
}