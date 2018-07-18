using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Hash.TupleHash;
using NIST.CVP.Crypto.TupleHash;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TupleHash.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class MCTTests
    {
        [Test]
        [TestCase(128, "6dc13301be877363018fbbbddd779b7d8a33", "0f67800327bcc9d1be26cd53c2d6508320d0223044b7b1dbc40d0fbbbb69006cf61e9019a679c45cea05bdf9f193e674637e143d008ea87ae024a984c3e6032f288112e667148b963830d7cd5594995b7142799bcf97002cc257852713a87d6a6eecbd5b3c0e81b93e12752ef27000fe4b2d367fd351b44222ab9e4c74f73b923c8e06a16489a29f545c50813173092fb1dbc77d5a338be403b6455211f8909486c6a8f51fb504644f00a2950afb1cf66f998862706fac402b29324d94aa115de4b1ce3f8c375629b8f0e1a3c5c2409ecded01da984e4a94ad584222b40f341905c49f36a6a2ce7cb7b5ed2b28a35eb4a18d9c5b282d952bdd7a9b1b1cc309d785cb6c0ce3630997497defff7db709910d6f20b0807e55b0ce7df6d11035cc981ad5fd36ceea9b51af50f48e77aba9c0fe9c097e6141d7be790b9f241d7e5ec49514a31c89eb4e77f939de00f07dd84cce3a17da71d2067af986c4c35770a8967f1d781e2daa09300c899fee52ce1dbd87a66fc853676552476e5477ab457b851a525eec44867152b0bf7aa5ff1d973f47198186b38e9478daa6007c083da0936c199fe0398f314459171e6ab23d305214cb4a42fa6620ef1150d46c2dd4b39636727097866a170f5c62f5e5a89f7693961b54f5e8430cdf0ce97b2669b70f6b7799e3186d188b4bff662387487cba9c098d115cd961")]
        [TestCase(256, "acb91ac0a42bb1653fdd3b43ce7e8aa259f3", "9b690570da68fe59553d7dc23ab2fb5a8043fd2ec2c2111a7e0b57549457c9e392f3647620f8921db2ac7f4726394cb71b1de8ba24962887e239fbcc0e90eaedec9f3fa765ebf9892925947cd91747574d79d449f934a09d7e6686c7bde901d4ba40146574544643476ada1ed3f00b8a73420cd57e56aa85836755190e96d9ff51d97efb8ba755b5521921707426a19008739535f0e5c6fd8649916cc45445ee8f9a55fabe4472372e5346063a6ff1f70b9a2eb87273a1054326b13ef670b079c25f5142a50949f7dc9e5cef08bcbd111338f05bd34b215e1f0f177d5237d8557a814dc49942b2f6348cb3560ed6d32ae7253bfaaa7087b30e1b75e04a3cb1f2acbfe09a9be7fa01487f4d1ff11b0cc9c59bf1267dbc54d406c8310de10b2ac398b78943940acb2e9c3af64912c0fb64e73ee17569e049605efca09308560d3c06abfde40b2cae690c1c1a2f25538f5f0480c9ed9ebaa50e5c5937980321e262a181e65d1c57b94fbac8490da60037e794b785b233503c066485673d0aeb0bb81899caaf0faae0be6755c32ac5d657284ae0c90ee55b6a0d209b45539808fa8a70dc2d5bfc623b98ab0d826e14687ccc4ac8cd0618c0c52eb9903cba27c8a9bfa81d5382996b301671ac7d045af590870d4218a04bb251779f7ba528881ddcc0155a8fb96c77fe0b0c7864473c52461436be3811d9a6e0a3d7971f40ceeaff9d1f239c2d16d1aeab9cef8e1b8c1ff4f3210cfbbbafa8efe387f98743a1dba5fef5a54213d2944b5e2d2723db3b8eb981150bf7409c97e2c83457b0436e0565b31b16393e2df3b412b81550f0e715963adae254f6629f17c2c32156260b940d26400709cd81bd0f93c247aa73a4f07fd8e13995ef9a7df9855b72c78e4549d9bdcc5542557fe513315eb19438adcc20e6894796a2f03c5c24d5100f36beab234de04189d01aa250b30bbd093b87ef54388d4805948f60fa7e0b4efe17f5524962edd262d4010efd50ac0e848f68e370eef77021a9a7dcd10f17450464afe4ed5309554c2a5b5e9eb08370fd3d12c93a07b21048f125266a5e13c12451ceac1409d5bc4a3486a27d17534b8c781b7cfadcf7f07fc947d2c0fe11445da5558818a4e3f54661370bf94e87529a34504a01c72d03df96c534ea1e097cd688730187eb1d682cfb136672a84f949ed317bbc4f01151bf88a436b9c86d638462aba5bb97c6f60976810e0e5320b6d6b018b380be472b36737ea0a58b6b439fc869f2615d7dd1953a01deb4b23baf944c1270435efb593784a7621ff3a9dcce84f71683f6dfd898edefc28de6532b1735525f9182496945e29c3adfc35f24a8fb59b0fbdc4a5dc5c040fd02f54967cc1dd29331e307472ede5a5ba824aa7ca57dbfe6c55bcb24f720f7f910108f99ae5d4839eddfe06c61dc29fbb1bd5c3c63ed2f76feea979166bd30213b07d4b2e165838368eede19389215ad97628d36e0296a7dea7ca100a1cd30b1359d71a3f5149dffe91ff6fcafac0dfead9a38c89d5921ab4af9a2ff0b0bf5e2d424c40657a509f35104ecfb4299b7914cba2eab7906dc3c26556365da46db58893440c38a0900")]
        public void ShouldMonteCarloTestTupleHashForSampleSuppliedCase(int digestSize, string message, string digest)
        {
            var subject = new TupleHash_MCT(new Crypto.TupleHash.TupleHash());
            var messageBitStringTuple = new List<BitString>(new BitString[] { new BitString(message) });
            var digestBitString = new BitString(digest);
            var hashFunction = new HashFunction
            {
                Capacity = digestSize * 2,
                DigestLength = digestSize,
                Customization = ""
            };

            var domain = new MathDomain();
            domain.AddSegment(new RangeDomainSegment(null, 16, 65536));
            var result = subject.MCTHash(hashFunction, messageBitStringTuple, domain, true);

            Assert.IsNotNull(result, "null check");
            Assert.IsTrue(result.Success, result.ErrorMessage);
            
            var resultDigest = result.Response[result.Response.Count - 1].Digest;
            Assert.AreEqual(digestBitString.BitLength, resultDigest.BitLength);
            Assert.AreEqual(digestBitString.ToHex(), resultDigest.ToHex());
        }
    }
}
