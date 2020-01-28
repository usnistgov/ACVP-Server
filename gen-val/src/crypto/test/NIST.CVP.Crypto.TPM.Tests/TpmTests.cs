using System;
using NIST.CVP.Crypto.Common.KDF.Components.TPM;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.TPM.Tests
{
    [TestFixture, FastCryptoTest]
    public class TpmTests
    {
        [Test]
        [TestCase("13ebe0d11c2faa4ed48e9ba14d032fbe52749001", "ec55201856da349d83e16539e36702787ffd15f1", "b9691b8360b0654f3640e526a7e477c7e8836876", "e9ab65ddbddf97c3f457950f97d10e0e5c29fafc")]
        [TestCase("00608c9c9ae91bee8d794781d07ce6f06467019b", "dbcf3631b3953a97e676f983cfb5fa433a288f3d", "2fa13cf29d0bf2b8396e8d8d944ba3f2c8a9fc75", "da7e3417ffc8024edc6106d1660ad4c88e37972e")]
        [TestCase("6082b69b1dade4b09749a8f7a23e62b8e1674639", "c5f8d086f3487af8c3adc162f52a6c8dbc96d7e0", "d3a0d5974ced13240f83080360f98f3d8702bf1f", "b7e99f1bff8fa436b87ad8d9d5a943f04b12061d")]
        [TestCase("d63dfaae1bcf60deb0b03b3d77677436c35e6882", "c8f7368add32bb46afa2a7527d9ada2bea2db422", "15c6dfe22b664c03a77828fbefbf586821e54aea", "b2c66ad8c60c45dca3b6b00607dbacb7c077f644")]
        [TestCase("34bc303349bf6738a48cdaccec0110bdd873c399", "36e8abcf888a5ce18cf6ad02254a3c40cc72010d", "0697def7e59c00bc2c32fb56d7766793cdbde295", "abe4b0c0bad4c813e016b3fef0bca917ce4e08c3")]
        public void ShouldDeriveKeyCorrectlyTPM(string authHex, string evenHex, string oddHex, string expectedHex)
        {
            var auth = new BitString(authHex);
            var nonceEven = new BitString(evenHex);
            var nonceOdd = new BitString(oddHex);
            var expectedSKey = new BitString(expectedHex);

            var factory = new TpmFactory(new HmacFactory(new ShaFactory()));
            var subject = factory.GetTpm();

            var result = subject.DeriveKey(auth, nonceEven, nonceOdd);

            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.AreEqual(expectedSKey, result.SKey);
        }
    }
}
