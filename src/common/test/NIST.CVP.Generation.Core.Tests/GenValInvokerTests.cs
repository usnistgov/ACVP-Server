using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.Core.Tests
{
    [TestFixture, UnitTest, FastIntegrationTest]
    public class GenValInvokerTests
    {
        [Test]
        public void EachAlgoModeRevisionShouldHaveAnISupportedAlgoModeRevision()
        {
            foreach (AlgoMode algoMode in Enum.GetValues(typeof(AlgoMode)))
            {
                var result = GenValInvoker.GetAlgoModeRevisionInjectables(algoMode);

                Assert.IsNotNull(result, $"{nameof(algoMode)}: {algoMode}");
                Assert.IsInstanceOf(typeof(ISupportedAlgoModeRevisions), result);
            }
        }

        [Test]
        public void EachAlgoModeRevisionShouldExistOnlyOnceWithinGeneration()
        {
            // Get the ISupportedAlgoModeRevisions from the loaded assemblies.
            var algoModeRevisionCandidates = GenValInvoker.GetSupportedAlgoModeRevisions();

            // Get the list of supported AlgoModes from the candidates
            var algoModesFromCandidates = new List<AlgoMode>();
            algoModeRevisionCandidates.ForEach(fe =>
                algoModesFromCandidates.AddRangeIfNotNullOrEmpty(
                    fe.SupportedAlgoModeRevisions
                )
            );

            // There should never be more than one of any AlgoMode from the candidate list
            var counts = algoModesFromCandidates.GroupBy(gb => gb).Select(s => new { Key = s.Key, Count = s.Count() });

            Assert.True(counts.All(a => a.Count == 1));
        }
    }
}