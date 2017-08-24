using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NIST.CVP.Generation.RSA_SigVer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.RSA_SigVer.Tests
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
                Capabilities = BuildCapabilities(),
                SigVerModes = new[] { "ANSX9.31", "PKCS1v15", "PSS" },
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
                Capabilities = BuildCapabilities(),
                SigVerModes = new[] { "ANSX9.31", "PKCS1v15", "PSS" },
            };

            Assert.AreEqual("RSA", parameters.Algorithm);
        }

        private SigCapability[] BuildCapabilities()
        {
            var hashPairs = new HashPair[ParameterValidator.VALID_HASH_ALGS.Length];
            for (var i = 0; i < hashPairs.Length; i++)
            {
                hashPairs[i] = new HashPair
                {
                    HashAlg = ParameterValidator.VALID_HASH_ALGS[i],
                    SaltLen = (i + 1) * 8
                };
            }

            var capabilities = new SigCapability[ParameterValidator.VALID_MODULI.Length];
            for (var i = 0; i < capabilities.Length; i++)
            {
                capabilities[i] = new SigCapability
                {
                    Modulo = ParameterValidator.VALID_MODULI[i],
                    HashPairs = hashPairs
                };
            }

            return capabilities;
        }
    }
}
