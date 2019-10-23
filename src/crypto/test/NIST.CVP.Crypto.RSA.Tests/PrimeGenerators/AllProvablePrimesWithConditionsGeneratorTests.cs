using System;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.RSA.Tests.PrimeGenerators
{
    [TestFixture, LongCryptoTest]
    public class AllProvablePrimesWithConditionsGeneratorTests
    {
        [Test]
        [TestCase(0, "010001", "ABCD", new[] {200, 200, 200, 200})]
        [TestCase(2048, "03", "ABCD", new[] {200, 200, 200, 200})]
        [TestCase(2048, "010001", "ABCD", new[] {0, 200, 200, 200})]
        [TestCase(2048, "010001", "ABCD", new[] {200, 200, 200})]
        public void ShouldFailWithBadParametersFips186_4(int nlen, string e, string seed, int[] bitlens)
        {
            var param = new PrimeGeneratorParameters
            {
                Modulus = nlen,
                PublicE = new BitString(e).ToPositiveBigInteger(),
                Seed = new BitString(seed),
                BitLens = bitlens
            };
            
            var sha = new ShaFactory().GetShaInstance(new HashFunction(ModeValues.SHA1, DigestSizes.d160));
            var subject = new AllProvablePrimesWithConditionsGenerator(sha);

            var result = subject.GeneratePrimesFips186_4(param);
            Assert.IsFalse(result.Success);
        }

        [Test]
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
        public void ShouldPassWithGoodParametersFips186_4(int nlen, ModeValues mode, DigestSizes dig, int bitlen1, int bitlen2, int bitlen3, int bitlen4, string e, string seed, string p, string q)
        {
            var param = new PrimeGeneratorParameters
            {
                Modulus = nlen,
                PublicE = new BitString(e).ToPositiveBigInteger(),
                Seed = new BitString(seed),
                BitLens = new [] {bitlen1, bitlen2, bitlen3, bitlen4}
            };
            
            var sha = new ShaFactory().GetShaInstance(new HashFunction(mode, dig));

            var subject = new AllProvablePrimesWithConditionsGenerator(sha);

            var result = subject.GeneratePrimesFips186_4(param);

            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.AreEqual(p.ToUpper(), new BitString(result.Primes.P).ToHex(), "p");
            Assert.AreEqual(q.ToUpper(), new BitString(result.Primes.Q).ToHex(), "q");
        }
        
        [Test]
        [TestCase(2048, ModeValues.SHA1, DigestSizes.d160, 
            296, 182, 328, 166, "0100000001", "2f1f303a2630e5eb13d9f152dc114bd4f2df0e4737b2f1113bcdd367", 
            "B87C1A91830CE1F139EF57442649F0B658A38760A8641529477E8852DA5E5149D2903EEE429F9EC575CAE7BB41AFA059785A977E48CF3809DAEDD87C878FE67DB149497CB37C1AB8334C608E668C445AE8590D780260210D3D37B3F279FE99EF729199D45B401A222F4ED6F5B4A087D5111CF0E0A5213213E57C173FE46A687B", 
            "D31D66879610923BE079764BE5F23F04DA500B88B0DEE74E592C03C5CA5C941300E697CFECA6B453CF2436D03E037BD5320AA46F07AD1C6E3391783928D7BE8738EF8D0F3DA0E2554ACD96ECAC6645868CC78F574661BE1C6FDF5778B4686E5A0345E264CBE9355D8F2F5B9BF872B62D9ED5CD15942BE7641F6A3EEFEC091181")]
        [TestCase(3072, ModeValues.SHA2, DigestSizes.d224, 
            176, 301, 176, 238, "0100000001", "983511f382c429d80b908c36d401568cee1206291c0967fe6cff7c1012345678", 
            "F56ADD295AE21CCDEF84A4A11C21A448F81693AA5C25890D5EDB2BD448FAF6406DC6702AA460669B12C6CCBB05773DA2070E99BB76B55694E899B0CA9FADABC82CCD673CC919FCC758F4822AE6A5C1866DD490EE82BE2911DA9B56E1551BA2F6572B6A891F7BE025F4F870A82974B7AB2DC2474F6AC81B852AE52DD1825E8F015A3809DD1A880FE73739B7EC9BBFC8D61EBE16F581C86DA858E4B40A449E87986D45E7558782DEEC48EAB173423BD9567C30407F2C343DF57F718EB9D9619263", 
            "DA9500DB07CABADC5558C5C02FCAAD4F18F0E1DA08FE429ED1CA54FE679310CE31FD3495FB4DA9375ECD7523D74C952529250EC8C0E377AC7ACF5AEFA9AC9A77D733B92350F29EBC64B74F818E84A9A34BB1F6F4AFF661463D9569A67AA27960D03A2A6E80E936B1A651D707917FCEC5B85796455B11AE74629F7234202C2C10004AE62B1EBB33A6C7D96D1C6AD8F791274A8A8349B6356FFAD78EB1DCCC50633D1525FF3B686C93E3B6053D14578B99DD8F40205B6C455A2CBFA2C76E3E6789")]
        [TestCase(4096, ModeValues.SHA2, DigestSizes.d256, 
            312, 143, 152, 304, "0100000001", "be15669a48f5ffadd029faaf1e31039d8c5aec3ff04850be2556850812345678", 
            "CF9A3E08E39A17627DFFDB1302FEF3CD01FEBD04C1EDEF4E52EA9D10507571536A04FB9A59E67635AE980E0E7BB1EE5552749F97B9BF0EA59551645FAB1190226A762AED072287C1B1E399DD793B3FEA437B1D423A0C3E29CC6EE64C500302E950A7454B7BE533C3750253C3E87A8F108E72DC83893B099EE13599F8453CB6594988B6F95535E2E3363B16AFD9E14E49BB2ACB002B801EBEA4AAADC87CADEC41A29AD57D40B72328BC4DE11411A7C6A7B39A7F5B0217DACBD8FE26249845E0AFF4BFC593BE6E9DDF5FDD14E8370386C9A94C1D236CEDFF06BD271189832C36CF0C3D7F784E1E11936521E95337B1BA6605D08D7F5C99022A3BC49CA1F367A0FD", 
            "EC6578E3D34608234A2BAA754C7DBE70F17EF2BC37F8408AB02407EB9CA13ACF463ED15B2CE818659FAEEAFE768C0F10B73291227B688FB7C0FFF8A3B016827C8846084E3BB9B0F624286939CE185FCF65A2DA51E51238C515B21E2D10AD3BBE942729660BF6FE604554886CF24A0CE3AC2BCEE9CCF2A9548FD80D28AB10C48514C354F328E14AD15D68828659653CB25DA6A6BBDA33923EBC8B8A21612614E562E331007E43CBFFD9E6D24F17F2FB91EECB570F9F5727E4326DB22A825DB70DA4B258F179CB013DF6201E38EBFEA0ACA37BD3166F411D0D1353488576C10C7A5845ECF8406D5619684978A6F0D98BB9326A12E256194E3BDA38BAF790C4F4D1")]
        public void ShouldPassWithGoodParametersFips186_5(int nlen, ModeValues mode, DigestSizes dig, int bitlen1, int bitlen2, int bitlen3, int bitlen4, string e, string seed, string p, string q)
        {
            var param = new PrimeGeneratorParameters
            {
                Modulus = nlen,
                PublicE = new BitString(e).ToPositiveBigInteger(),
                Seed = new BitString(seed),
                BitLens = new [] {bitlen1, bitlen2, bitlen3, bitlen4}
            };
            
            var sha = new ShaFactory().GetShaInstance(new HashFunction(mode, dig));

            var subject = new AllProvablePrimesWithConditionsGenerator(sha);

            var result = subject.GeneratePrimesFips186_5(param);

            Assert.IsTrue(result.Success, result.ErrorMessage);
            
            Console.WriteLine($"p: {new BitString(result.Primes.P).ToHex()}");
            Console.WriteLine($"q: {new BitString(result.Primes.Q).ToHex()}");

            Assert.AreEqual(p, new BitString(result.Primes.P).ToHex(), "p");
            Assert.AreEqual(q, new BitString(result.Primes.Q).ToHex(), "q");
        }
    }
}
