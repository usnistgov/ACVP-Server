using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_KeyGen.Tests
{
    [TestFixture, UnitTest]
    public class TestVectorFactoryTests
    {
        [Test]
        public void ShouldReturnVectorSet()
        {
            var subject = new TestVectorFactory(new AFTTestGroupFactory(), new KATTestGroupFactory(), new GDTTestGroupFactory());
            var result = subject.BuildTestVectorSet(
                new Parameters
                {
                    Algorithm = "RSA-KeyGen",
                    HashAlgs = new [] {"SHA-1", "SHA-224"},
                    FixedPubExp = "010001",
                    InfoGeneratedByServer = true,
                    IsSample = false,
                    KeyGenModes = new [] {"B.3.2", "B.3.3"},
                    Moduli = new [] {2048, 3072},
                    PrimeTests = new [] {"tblC2"},
                    PubExpMode = "fixed"
                });

            Assert.IsNotNull(result);
        }

        [Test]
        public void ShouldReturnVectorSetWithProperTestGroupsForAllModes()
        {
            var subject = new TestVectorFactory(new AFTTestGroupFactory(), new KATTestGroupFactory(), new GDTTestGroupFactory());
            var result = subject.BuildTestVectorSet(
                new Parameters
                {
                    Algorithm = "RSA-KeyGen",
                    HashAlgs = new [] {"SHA-1", "SHA-224", "SHA-256", "SHA-384", "SHA-512", "SHA-512/224", "SHA-512/256"},
                    InfoGeneratedByServer = true,
                    IsSample = false,
                    KeyGenModes = new [] {"B.3.2", "B.3.3", "B.3.4", "B.3.5", "B.3.6"},
                    Moduli = new [] {2048, 3072, 4096},
                    PrimeTests = new [] {"tblC2", "tblC3"},
                    PubExpMode = "random"
                });

            Assume.That(result != null);
            Assert.AreEqual(102, result.TestGroups.Count);
        }

    }
}
