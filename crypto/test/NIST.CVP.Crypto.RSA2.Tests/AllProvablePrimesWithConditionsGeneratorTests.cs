using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.RSA2.PrimeGenerators;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.Crypto.RSA2.Tests
{
    // Takes 16 seconds
    [TestFixture, LongCryptoTest]
    public class AllProvablePrimesWithConditionsGeneratorTests
    {
        [Test]
        [TestCase(0, "010001", "ABCD", new[] {200, 200, 200, 200})]
        [TestCase(2048, "03", "ABCD", new[] {200, 200, 200, 200})]
        [TestCase(2048, "010001", "ABCD", new[] {0, 200, 200, 200})]
        [TestCase(2048, "010001", "ABCD", new[] {200, 200, 200})]
        public void ShouldFailWithBadParameters(int nlen, string e, string seed, int[] bitlens)
        {
            var sha = new ShaFactory().GetShaInstance(new HashFunction(ModeValues.SHA1, DigestSizes.d160));
            var subject = new AllProvablePrimesWithConditionsGenerator(sha);
            subject.SetBitlens(bitlens);
            var result = subject.GeneratePrimes(nlen, new BitString(e).ToPositiveBigInteger(), new BitString(seed));
            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase(1024, ModeValues.SHA2, DigestSizes.d256,
            136, 101, 120, 117, "d816d7", "73ec2e0a5ebf29eb2c4fe5781160da0c6321cccf",
            "da80bfe2baa6a3856c3ad7331d69c1de70ad44259547fb6ecb16e1674cc4b9b500c8334d68edb1bda382112fdde5e86d527c145f0a4b955eee8a3b07b6e905fd",
            "f162edb3ac2ad456cc20eec6a4a5a2e999e64923796ce7e71b70d516871eb431f5fe552f5ce26c2311bbe9b373a973a1bc8891642162c5e4f35d0f9fb6ecf7b5")]
        [TestCase(1024, ModeValues.SHA2, DigestSizes.d256,
            120, 118, 112, 105, "f0aa2b", "542de6737b1752b915a70d845121c32a1c575ec4",
            "bfa0a3f447aaac5859544f38779f7f7717d9cfffe2a5ce2ddf3fa32d14425344166e1acd51487e57d556a2f88db091c0673bcc8ba8a249f65cdad8f0c51777fb",
            "dbc72b4ca5dc901ceff4f60bd75875d077814428b05f7ec486b3a0115da94d13b86423c7a349f32022b19cf095e0b801fed4d87a26990a0f4638bfffd21458c9")]
        [TestCase(2048, ModeValues.SHA1, DigestSizes.d160, 
            296, 182, 328, 166, "0100000001", "2f1f303a2630e5eb13d9f152dc114bd4f2df0e4737b2f1113bcdd367", 
            "b87c1a91830ce1f139ef57442649f0b658a38760a8641529477e8852da5e5149d2903eee429f9ec575cae7bb41afa059785a977e48cf3809daedd87c878fe67db149497cb37c1ab8334c608e668c445ae8590d780260210d3d37b3f279fe99ef729199d45b401a222f4ed6f5b4a087d5111cf0e0a5213213e57c173fe46a687b", 
            "d31d66879610923be079764be5f23f04da500b88b0dee74e592c03c5ca5c941300e697cfeca6b453cf2436d03e037bd5320aa46f07ad1c6e3391783928d7be8738ef8d0f3da0e2554acd96ecac6645868cc78f574661be1c6fdf5778b4686e5a0345e264cbe9355d8f2f5b9bf872b62d9ed5cd15942be7641f6a3eefec091181")]
        [TestCase(2048, ModeValues.SHA2, DigestSizes.d224, 
            176, 301, 176, 238, "0100000001", "983511f382c429d80b908c36d401568cee1206291c0967fe6cff7c10", 
            "cedb78fa06b6f5c3d977576aa8444c123d04125786516272f71ef1173eaaa81eb9eff8cc8a731164e4bba33631890ca2eb459c422a5354bc6586cb7faa57db0ce14dac8daa9b49937280ad92ea643d5220d2885892f2a85f3ccde6e7c2b42e659a169342bec9e57701fec88bbda9e948b2bac54eab00e9b9cb3df603dbf3fdb5", 
            "dce6ad5f359534f1edfe93a1687c0993503781a847bcb650d10585281994da028c4ad2db8a22fe3b01cc9d525365d1d56244a67d761472c8f4cf984329e0d4e970fb40cae83fa57efc0ace6610ee140cb7b327d2389551103f70a0f2e4631354ae1d1daeeab0a50c2533962054813c42adef6e36ebed25e11c5452d7f857a0a1")]
        [TestCase(2048, ModeValues.SHA2, DigestSizes.d256, 
            312, 143, 152, 304, "0100000001", "be15669a48f5ffadd029faaf1e31039d8c5aec3ff04850be25568508", 
            "e1c79592e096d32e65082b9e7feb6e6087658ff15fa85bb4eaf27f26b9346e370b3f489106c2a731e1b126d9a295cf9fdad3a6f9594402e8af636b25dc8e568a8d114ab3bf26dd82548403ec27df6184e1b10b7f5b1b2e37f44a5b2fbcde83eaeffea20547050ac0d792934a22dba7b33ff4bce96eee273f292123864f73054d", 
            "fca718ec90a9f296bf3db74bcabc4268870a79d837011a9799a53ca206f147d9f7305b5855f5684a4caa8e517d0c7c7743c15967b2acb46081f3ee109e8d3b719ef37b5542c8744b1eb7b6aa39bdef079c574ff422d64f9533c65e046f167793a9c955c704f43c5250b266e5e3e141b4359819069fc4a4a7a0f294e2cc3f7419")]
        public void ShouldPassWithGoodParameters(int nlen, ModeValues mode, DigestSizes dig, int bitlen1, int bitlen2, int bitlen3, int bitlen4, string e, string seed, string p, string q)
        {
            var sha = new ShaFactory().GetShaInstance(new HashFunction(mode, dig));

            var subject = new AllProvablePrimesWithConditionsGenerator(sha);
            subject.SetBitlens(bitlen1, bitlen2, bitlen3, bitlen4);

            var result = subject.GeneratePrimes(nlen, new BitString(e).ToPositiveBigInteger(), new BitString(seed));

            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.AreEqual(new BitString(p).ToPositiveBigInteger(), result.Primes.P, "p");
            Assert.AreEqual(new BitString(q).ToPositiveBigInteger(), result.Primes.Q, "q");
        }
    }
}
