using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_KeyGen.Tests
{
    [TestFixture, UnitTest]
    public class ParametersTests
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters
            {
                Algorithm = "RSA-KeyGen",
                KeyGenModes = new[] { "B.3.2", "B.3.5" },
                IsSample = false,
                Moduli = new [] {2048, 3072, 4096},
                HashAlgs = new [] {"SHA-1", "SHA-256"},
                PubExpMode = "random"
            };

            Assert.IsNotNull(parameters);
        }

        [Test]
        public void ShouldCoverParametersGet()
        {
            var parameters = new Parameters
            {
                Algorithm = "RSA-KeyGen",
                KeyGenModes = new[] { "B.3.2", "B.3.5" },
                IsSample = false,
                Moduli = new[] { 2048, 3072, 4096 },
                HashAlgs = new[] { "SHA-1", "SHA-256" },
                PubExpMode = "fixed",
                FixedPubExp = "010001"
            };

            Assert.AreEqual("RSA-KeyGen", parameters.Algorithm);
        }
    }
}
