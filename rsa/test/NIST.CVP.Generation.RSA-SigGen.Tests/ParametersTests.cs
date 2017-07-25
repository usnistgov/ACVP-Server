using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NIST.CVP.Tests.Core.TestCategoryAttributes;

namespace NIST.CVP.Generation.RSA_SigGen.Tests
{
    [TestFixture, UnitTest]
    public class ParametersTests
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters
            {
                Algorithm = "RSA",
                Mode = "SigGen",
                IsSample = false,
                Moduli = new[] { 2048, 3072, 4096 },
                HashAlgs = new[] { "SHA-1", "SHA-256" },
                SigGenModes = new[] { "ANSX9.31", "PKCS1v15", "PSS" },
                SaltMode = "fixed",
                Salt = "ABCD",
                SaltLen = new[] { 20, 20, 20 },
                N = "ABCD",
                PrivExp = "ABCD",
                PubExp = "ABCD"
            };

            Assert.IsNotNull(parameters);
        }

        [Test]
        public void ShouldCoverParametersGet()
        {
            var parameters = new Parameters
            {
                Algorithm = "RSA",
                Mode = "SigGen",
                IsSample = false,
                Moduli = new[] { 2048, 3072, 4096 },
                HashAlgs = new[] { "SHA-1", "SHA-256" },
                SigGenModes = new[] { "ANSX9.31", "PKCS1v15", "PSS" },
                SaltMode = "fixed",
                Salt = "ABCD",
                SaltLen = new[] { 20, 20, 20 },
                N = "ABCD",
                PrivExp = "ABCD",
                PubExp = "ABCD"
            };

            Assert.AreEqual("RSA", parameters.Algorithm);
        }
    }
}
