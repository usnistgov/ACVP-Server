using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.RSA.Signatures;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.RSA_SigVer.Tests
{
    [TestFixture, UnitTest]
    public class PKCS_PSS_Tests
    {
        [Test]
        [Ignore("Known error, testing PKCS vs PSS from CAVS")]
        public void PSSTest()
        {
            var pHex = "d40ad5636fa9150021cffd0fe8aae08bbec4ce6ec66f38cb45497712c2d61593f4a32fde42ee04e98feecfcf3579aaa02cf2364957ff6a56f6fb85cbc0bc40d996602243b0d87cd391aaadd53478456bb6ce4b97ef64ceba08a736a5d0145a0ee158b2bdbdf310232ce785197f930989a110da422e85c0de82d9fef456085097";
            var qHex = "c90362c66a58b9a0eae11fb1a93be894814a6b143f3dcbf23a23ae37298ac9ef3c703ab311be9192ef6cf7099541475ce5deef64ca6477bf5247a19dca0c1acfbc3977fb42cea8e23e8ee1a16f61d3522ca36eeaaa725d6a77a54e2e6d08bc26d3693ca3a1bdf1481ccaf360a14cd9edc986774ca1af3399e2c7c79713b3391f";
            var eHex = "89e191";
            var msgHex = "6155c152159623fed936c1edf46ec03d5b6f5dc8d4f530545b20b20ac580fe2aa36ed4fc69a3e30ad003488c78ba5224d28a88cb3d63ed0b6f35827017020d39c0371952f93c3e87b34aa2f2ea7dc0b5882a68ef867207e85417298f7b8f3c278157973d0c8abbdd4bb2d80592de935a44f5061af6490852d16faf51384f145c";
            var sigHex = "2b4549a4842d3e6e8642f995b4e446fd4effd19d7974fe3d1d41ca6c2e28056e58fd5e79d7a2150f0850f08bee48280d23ad8f1dbad36e34548f6d652e8d4a890b6680a1256822cf7f94cc23e7471fe308094a5ebdd97ec49137aa61353e7fcd852424b4ca90f869d544073d8e41987dd1a2495711ff01f51c4c4d4c3eede4733936eea6afa2c888f7237a3dd9e6ea004d3d4161805b5d91c574efccddf76feb2349a57f2a5e74a89c333def73b71e01158387dcd6fd02922eb378c041eb9cb838b584c1eeefe3a525cce52b150064196f94117b70f1679ea2e581be5dde99e690d1c480428e328b1cfffbd8d5b998aeecb7f3c2eb31a25a04957d200b20c050";
            var saltHex = "abcd";

            var p = new BitString(pHex).ToPositiveBigInteger();
            var q = new BitString(qHex).ToPositiveBigInteger();
            var e = new BitString(eHex).ToPositiveBigInteger();
            var key = new KeyPair(p, q, e);
            var msg = new BitString(msgHex);
            var sig = new BitString(sigHex);
            var salt = new BitString(saltHex);

            var pssSigner = new RSASSA_PSS_Signer(new HashFunction { Mode = ModeValues.SHA1, DigestSize = DigestSizes.d160 }, EntropyProviderTypes.Testable, salt.BitLength / 8);
            pssSigner.AddEntropy(salt);
            var result = pssSigner.Verify(2048, sig, key, msg);

            Assert.IsTrue(result.Success, result.ErrorMessage);

            //var pkcsSigner = new RSASSA_PKCSv15_Signer(new HashFunction { Mode = ModeValues.SHA1, DigestSize = DigestSizes.d160 });
            //var result2 = pkcsSigner.Verify(2048, sig, key, msg);

            //Assert.IsTrue(result2.Success, result2.ErrorMessage);
        }
    }
}
