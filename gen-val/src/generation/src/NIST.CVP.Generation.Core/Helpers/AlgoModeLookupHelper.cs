using System;
using System.Text;
using NIST.CVP.Common;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Generation.Core.Exceptions;
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
        /// <param name="algo">The algorithm.</param>
        /// <param name="mode">The mode of the algorithm.</param>
        /// <param name="revision">The testing revision for the algorithm.</param></para>
        /// <returns></returns>
        public static AlgoMode GetAlgoModeFromStrings(string algo, string mode, string revision)
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

            if (!string.IsNullOrEmpty(revision))
            {
                sb.Append(concatenationSeparator);
                sb.Append(revision);
            }

            var concatenatedAlgoModeRevision = sb.ToString();

            try
            {
                return EnumHelpers.GetEnumFromEnumDescription<AlgoMode>(concatenatedAlgoModeRevision);
            }
            catch (InvalidOperationException)
            {
                string errorMsg =
                    $"Unable to map {nameof(algo)} ({algo}), {nameof(mode)} ({mode}), and {nameof(revision)} ({revision}) to {nameof(AlgoMode)}";
                Logger.Error(errorMsg);
                throw new AlgoModeRevisionException(errorMsg);
            }
        }
    }
}
