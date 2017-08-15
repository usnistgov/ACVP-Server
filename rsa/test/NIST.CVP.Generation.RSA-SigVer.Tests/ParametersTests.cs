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
            var caps = new CapabilityObject[ParameterValidator.VALID_HASH_ALGS.Length];
            for (var i = 0; i < caps.Length; i++)
            {
                caps[i] = new CapabilityObject
                {
                    HashAlg = ParameterValidator.VALID_HASH_ALGS[i],
                    SaltLen = (i + 1) * 8
                };
            }

            var parameters = new Parameters
            {
                Algorithm = "RSA",
                Mode = "SigGen",
                IsSample = false,
                Moduli = new[] { 2048, 3072 },
                Capabilities = caps,
                SigGenModes = new[] { "ANSX9.31", "PKCS1v15", "PSS" },
            };

            Assert.IsNotNull(parameters);
        }

        [Test]
        public void ShouldCoverParametersGet()
        {
            var caps = new CapabilityObject[ParameterValidator.VALID_HASH_ALGS.Length];
            for (var i = 0; i < caps.Length; i++)
            {
                caps[i] = new CapabilityObject
                {
                    HashAlg = ParameterValidator.VALID_HASH_ALGS[i],
                    SaltLen = (i + 1) * 8
                };
            }

            var parameters = new Parameters
            {
                Algorithm = "RSA",
                Mode = "SigGen",
                IsSample = false,
                Moduli = new[] { 2048, 3072 },
                Capabilities = caps,
                SigGenModes = new[] { "ANSX9.31", "PKCS1v15", "PSS" },
            };

            Assert.AreEqual("RSA", parameters.Algorithm);
        }
    }
}
