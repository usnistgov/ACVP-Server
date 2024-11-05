using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Generation.GenValApp.Helpers;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.GenValApp.Tests
{
    /// <summary>
    /// Tests used to ensure there is a instance of <see cref="ISupportedAlgoModeRevision" /> for each <see cref="AlgoMode"/>.
    /// </summary>
    [TestFixture, UnitTest, FastIntegrationTest]
    public class AutofacConfigTests
    {
        [Test]
        public void EachAlgoModeRevisionShouldHaveAnISupportedAlgoModeRevision()
        {
            foreach (AlgoMode algoMode in Enum.GetValues(typeof(AlgoMode)))
            {
                var result = AutofacConfig.GetAlgoModeRevisionInjectables(algoMode);

                Assert.That(result, Is.Not.Null, $"{nameof(algoMode)}: {algoMode}");
                Assert.That(result, Is.InstanceOf(typeof(ISupportedAlgoModeRevisions)));
            }
        }

        [Test]
        public void EachAlgoModeRevisionShouldExistOnlyOnceWithinGeneration()
        {
            // Get the ISupportedAlgoModeRevisions from the loaded assemblies.
            var algoModeRevisionCandidates = AutofacConfig.GetSupportedAlgoModeRevisions();

            // Get the list of supported AlgoModes from the candidates
            var algoModesFromCandidates = new List<AlgoMode>();
            algoModeRevisionCandidates.ForEach(fe =>
                algoModesFromCandidates.AddRangeIfNotNullOrEmpty(
                    fe.SupportedAlgoModeRevisions
                )
            );

            // There should never be more than one of any AlgoMode from the candidate list
            var counts = algoModesFromCandidates.GroupBy(gb => gb).Select(s => new {s.Key, Count = s.Count() });

            Assert.That(counts.All(a => a.Count == 1), Is.True);
        }
    }
}
