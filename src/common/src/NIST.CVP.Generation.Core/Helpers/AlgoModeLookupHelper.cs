using System;
using System.Text;
using NIST.CVP.Common;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common;
using NLog;

namespace NIST.CVP.Generation.Core.Helpers
{
    /// <summary>
    /// Helpers for algo/mode lookup
    /// </summary>
    public static class AlgoModeLookupHelper
    {
        private static Logger Logger => LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets an <see cref="AlgoMode"/> from the provided <see cref="algo"/> and <see cref="mode"/>
        /// </summary>
        /// <exception cref="ArgumentNullException">When the algo is null or empty</exception>
        /// <exception cref="InvalidOperationException">When unable to map the algo and mode</exception>
        /// <param name="algo">The algorithm</param>
        /// <param name="mode">The mode of the algorithm</param>
        /// <returns></returns>
        public static AlgoMode GetAlgoModeFromStrings(string algo, string mode)
        {
            const string concatenationSeparator = "-";

            if (string.IsNullOrEmpty(algo))
            {
                string errorMsg =
                    $"{nameof(algo)} must be provided and not empty";
                Logger.Error(errorMsg);
                throw new ArgumentNullException(errorMsg);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(algo);

            if (!string.IsNullOrEmpty(mode))
            {
                sb.Append(concatenationSeparator);
                sb.Append(mode);
            }

            var concatenatedAlgoMode = sb.ToString();

            try
            {
                return EnumHelpers.GetEnumFromEnumDescription<AlgoMode>(concatenatedAlgoMode);
            }
            catch (InvalidOperationException)
            {
                string errorMsg =
                    $"Unable to map {nameof(algo)} ({algo}) and {nameof(mode)} ({mode}) to {nameof(AlgoMode)}";
                Logger.Error(errorMsg);
                throw new InvalidOperationException(errorMsg);
            }
        }
    }
}
