using System;
using System.Text;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Exceptions;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.Helpers
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
            try
            {
                return AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(algo, mode, revision);
            }
            catch (ArgumentNullException)
            {
                string errorMsg =
                    $"Both {nameof(algo)} and {nameof(revision)} are required and cannot be empty strings. Unable to map {nameof(algo)} ({algo}), {nameof(mode)} ({mode}), and {nameof(revision)} ({revision}) to {nameof(AlgoMode)}";
                Logger.Error(errorMsg);
                throw new AlgoModeRevisionException(errorMsg);
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
