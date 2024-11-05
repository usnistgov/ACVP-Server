using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.RSA.Tests
{
    public class EstimatedSecurityStrengthTests
    {
        /// <summary>
        /// These are the modulo and their "estimated/comparable security strengths" as per
        /// https://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-57pt1r5.pdf
        ///
        /// We didn't have security strengths for modulo > 4096, which was causing issues around
        /// seed generation for prime key generation.
        ///
        /// Implemented the algorithm from https://crypto.stackexchange.com/questions/8687/security-strength-of-rsa-in-relation-with-the-modulus-size
        /// in c#, to get the calculated security strength for *any* given modulo.
        ///
        /// The delta in the assert is because the numbers from NIST were a comparison to "comparable security strengths of symmetric ciphers",
        /// so they aren't exact.
        /// </summary>
        /// <param name="modulo"></param>
        /// <param name="estimatedSecurityStrength"></param>
        [Test]
        [TestCase(1024, 80, 86)]
        [TestCase(2048, 112, 116)]
        [TestCase(3072, 128, 138)]
        [TestCase(7680, 192, 203)]
        [TestCase(15360, 256, 269)]
        public void ShouldReturnEstimatedSecurityStrength(int modulo, int estimatedSecurityStrength, int securityStrength)
        {
            var calculatedSecurityStrength = KeyGenHelper.CalculateEstimatedSecurityStrength(modulo);

            Console.WriteLine(calculatedSecurityStrength);

            Assert.That(
                calculatedSecurityStrength, Is.EqualTo(estimatedSecurityStrength).Within(15),
                $"Expected: {estimatedSecurityStrength}, Actual: {calculatedSecurityStrength}");
            Assert.That(calculatedSecurityStrength, Is.EqualTo(securityStrength), nameof(securityStrength));
        }
    }
}
