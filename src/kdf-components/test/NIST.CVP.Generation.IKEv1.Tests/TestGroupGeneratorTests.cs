using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.IKEv1.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorTests
    {
        private TestGroupGenerator _subject;

        #region GetParametersAndExpectedGroups
        private static List<object> GetParametersAndExpectedGroups()
        {
            var randy = new Random800_90();

            var list = new List<object>
            {
                new object[]
                {
                    "Minimal Inputs",
                    new []
                    {
                        new CapabilityBuilder()
                            .WithAuthenticationMode("dsa")
                            .WithHashAlg(new [] {"sha-1"})
                            .WithDHLengths(new MathDomain().AddSegment(new ValueDomainSegment(128)))
                            .WithInitNonceLengths(new MathDomain().AddSegment(new ValueDomainSegment(128)))
                            .WithRespNonceLengths(new MathDomain().AddSegment(new ValueDomainSegment(128)))
                            .Build()
                    },
                    1
                },
                new object[]
                {
                    "Additional mode w/ Minimal Inputs",
                    new []
                    {
                        new CapabilityBuilder()
                            .WithAuthenticationMode("dsa")
                            .WithHashAlg(new [] {"sha-1"})
                            .WithDHLengths(new MathDomain().AddSegment(new ValueDomainSegment(128)))
                            .WithInitNonceLengths(new MathDomain().AddSegment(new ValueDomainSegment(128)))
                            .WithRespNonceLengths(new MathDomain().AddSegment(new ValueDomainSegment(128)))
                            .Build(),
                        new CapabilityBuilder()
                            .WithAuthenticationMode("psk")
                            .WithHashAlg(new [] {"sha2-224"})
                            .WithDHLengths(new MathDomain().AddSegment(new ValueDomainSegment(128)))
                            .WithInitNonceLengths(new MathDomain().AddSegment(new ValueDomainSegment(128)))
                            .WithRespNonceLengths(new MathDomain().AddSegment(new ValueDomainSegment(128)))
                            .WithPSKLengths(new MathDomain().AddSegment(new ValueDomainSegment(128)))
                            .Build()
                    }, 
                    2 
                },
                new object[]
                {
                    "All inputs at maximum, except Domains",
                    new []
                    {
                        new CapabilityBuilder()
                            .WithAuthenticationMode("dsa")
                            .WithHashAlg(ParameterValidator.VALID_HASH_ALGS)
                            .WithDHLengths(new MathDomain().AddSegment(new ValueDomainSegment(128)))
                            .WithInitNonceLengths(new MathDomain().AddSegment(new ValueDomainSegment(128)))
                            .WithRespNonceLengths(new MathDomain().AddSegment(new ValueDomainSegment(128)))
                            .Build(),
                        new CapabilityBuilder()
                            .WithAuthenticationMode("psk")
                            .WithHashAlg(ParameterValidator.VALID_HASH_ALGS)
                            .WithDHLengths(new MathDomain().AddSegment(new ValueDomainSegment(128)))
                            .WithInitNonceLengths(new MathDomain().AddSegment(new ValueDomainSegment(128)))
                            .WithRespNonceLengths(new MathDomain().AddSegment(new ValueDomainSegment(128)))
                            .WithPSKLengths(new MathDomain().AddSegment(new ValueDomainSegment(128)))
                            .Build(),
                        new CapabilityBuilder()
                            .WithAuthenticationMode("pke")
                            .WithHashAlg(ParameterValidator.VALID_HASH_ALGS)
                            .WithDHLengths(new MathDomain().AddSegment(new ValueDomainSegment(128)))
                            .WithInitNonceLengths(new MathDomain().AddSegment(new ValueDomainSegment(128)))
                            .WithRespNonceLengths(new MathDomain().AddSegment(new ValueDomainSegment(128)))
                            .Build()
                    },  
                    3 * 5
                },
                new object[]
                {
                    "Maximum number of groups for a test vector set",
                    new []
                    {
                        new CapabilityBuilder()
                            .WithAuthenticationMode("dsa")
                            .WithHashAlg(ParameterValidator.VALID_HASH_ALGS)
                            .WithDHLengths(new MathDomain().AddSegment(new RangeDomainSegment(randy, 64, 512, 8)))
                            .WithInitNonceLengths(new MathDomain().AddSegment(new RangeDomainSegment(randy, 64, 512, 8)))
                            .WithRespNonceLengths(new MathDomain().AddSegment(new RangeDomainSegment(randy, 64, 512, 8)))
                            .Build(),
                        new CapabilityBuilder()
                            .WithAuthenticationMode("psk")
                            .WithHashAlg(ParameterValidator.VALID_HASH_ALGS)
                            .WithDHLengths(new MathDomain().AddSegment(new RangeDomainSegment(randy, 64, 512, 8)))
                            .WithInitNonceLengths(new MathDomain().AddSegment(new RangeDomainSegment(randy, 64, 512, 8)))
                            .WithRespNonceLengths(new MathDomain().AddSegment(new RangeDomainSegment(randy, 64, 512, 8)))
                            .WithPSKLengths(new MathDomain().AddSegment(new RangeDomainSegment(randy, 64, 512, 8)))
                            .Build(),
                        new CapabilityBuilder()
                            .WithAuthenticationMode("pke")
                            .WithHashAlg(ParameterValidator.VALID_HASH_ALGS)
                            .WithDHLengths(new MathDomain().AddSegment(new RangeDomainSegment(randy, 64, 512, 8)))
                            .WithInitNonceLengths(new MathDomain().AddSegment(new RangeDomainSegment(randy, 64, 512, 8)))
                            .WithRespNonceLengths(new MathDomain().AddSegment(new RangeDomainSegment(randy, 64, 512, 8)))
                            .Build()
                    },  
                    3 * 5 * 3
                }
            };

            return list;
        }
        #endregion GetParametersAndExpectedGroups

        [Test]
        [TestCaseSource(nameof(GetParametersAndExpectedGroups))]
        public void ShouldReturnOneITestGroupForEachCombination(string label, Capability[] capabilities, int expectedResultCount)
        {
            var p = new Parameters
            {
                Algorithm = "kdf-components",
                Mode = "ikev1",
                Capabilities = capabilities
            };

            _subject = new TestGroupGenerator();
            var result = _subject.BuildTestGroups(p).ToList();

            Assert.AreEqual(expectedResultCount, result.Count);
        }
    }
}
