using System;
using System.Text;

namespace NIST.CVP.ACVTS.Libraries.Common.Helpers
{
    public static class AlgoModeHelpers
    {
        /// <summary>
        /// Find the AlgoMode for a given algorithm/mode
        /// </summary>
        /// <param name="algorithm">The algorithm</param>
        /// <param name="mode">The mode</param>
        /// <param name="revision">The testing revision (often the name of the standard)</param>
        /// <returns></returns>
        public static AlgoMode GetAlgoModeFromAlgoAndMode(string algorithm, string mode, string revision)
        {
            if (string.IsNullOrEmpty(algorithm))
            {
                throw new ArgumentNullException(nameof(algorithm));
            }

            if (string.IsNullOrEmpty(revision))
            {
                throw new ArgumentException(nameof(revision));
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(algorithm);
            if (mode != null)
            {
                sb.Append("-");
                sb.Append(mode);
            }

            sb.Append("-");
            sb.Append(revision);

            return EnumHelpers.GetEnumFromEnumDescription<AlgoMode>(sb.ToString());
        }
    }
}
