using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Numerics;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KES.Tests
{
    [TestFixture, UnitTest, FastIntegrationTest]
    public class FfcMqvTests
    {
        private MqvFfc _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new MqvFfc();
        }

        private static object[] SuccessSharedSecretZTests = new object[]
        {
            new object[]
            {
                "from CAVS test1",
                // xPrivateKeyPartyA
                new BitString("95b0c5c27323a5db027b3e5cc147401a6e14f74140a1e0abb3c97c54").ToPositiveBigInteger(),
                // yPublicKeyPartyB
                new BitString("8c1007ea63f8dcd3c2056f2f8095c7315ec6abb42d0d63375f6cbcd3cd6f1bebbe138b0d9913254b6097abe352e2cb1fc1e1e2a33be342fb85e57a6c450e406201c4a4305496b4442f51ef6bf21b7cf2af6d2940aa19778d9e29c6963769bf0aaf00406b2ab704d176cbdf3ca60a1623f5b6c1335894053ba6b7b2d2b0dd824a4123859fef0461970dff2f2029b7b90325db0552ee78157f630295d0bcda333bb7673998b156586861050572e61745f8cf712119cc1752476cccab7aeb14851394acab243f5fb0fcdc3426bf958080fb588517e427dec19e23bfecca3d3d1c62331b75939066355331bb320f571e43394c5bb329c4b113cd7e88748d10a727c3").ToPositiveBigInteger(),
                // rPrivateKeyPartyA
                new BitString("54d2a8339aa26a7e5c43cf59fdbecb554a3d52b4e9fbaba22f0c3f1c").ToPositiveBigInteger(),
                // tPublicKeyPartyA
                new BitString("dc19a5f24fbd306b5000e9bffa5658102148fa0a08eac65f579c880aef3e9e8e222108bcfe1487d287508eef4879c813cfcd8ef0f99eb02a8647b244cdf0d39d9afab0336209ecc5cd8fa62e67e38c0e3f9f752f82d633ce1d9022a949615138d746b5acf4e5a5fbf7e0ab50fdedc68d458f1d404c44474cb7860d50f937e3a54af9485c0120ffa634d890a46b2e0363f32dd2e6fadb19a2178413700efdae4f5d6d69c3c7e273b1867c88ffceabf951c79a00ce1f3de1dd812dc7b57c7f8d9342cfe90c7c88ec3c400f45c0be587f1b43515cd35de732a01bed784f8b28841476ccbab5b5c483ab2db1d848f98c7d2a871dbb1d66b18f62e04040b5c23b8d2c").ToPositiveBigInteger(),
                // tPublicKeyPartyB
                new BitString("8c1007ea63f8dcd3c2056f2f8095c7315ec6abb42d0d63375f6cbcd3cd6f1bebbe138b0d9913254b6097abe352e2cb1fc1e1e2a33be342fb85e57a6c450e406201c4a4305496b4442f51ef6bf21b7cf2af6d2940aa19778d9e29c6963769bf0aaf00406b2ab704d176cbdf3ca60a1623f5b6c1335894053ba6b7b2d2b0dd824a4123859fef0461970dff2f2029b7b90325db0552ee78157f630295d0bcda333bb7673998b156586861050572e61745f8cf712119cc1752476cccab7aeb14851394acab243f5fb0fcdc3426bf958080fb588517e427dec19e23bfecca3d3d1c62331b75939066355331bb320f571e43394c5bb329c4b113cd7e88748d10a727c3").ToPositiveBigInteger(),
                // P
                new BitString("e95b3b9be6520afad80ee8fcd12538da31d16162f894bb46c4a4443fcc714eee031047afc5a3d9722fcbc0d879cadfd3c6754d16444418a7c0e83acd9372a34a0aba811b8a3316dd5af0cf58beef6b3a4358e840583608cdbdc2bb236ec10a16b4464bf6f6557e550cb0e694d7ada01aa0f0d60468de08cab41068304b1d62f83ee343a7338d15a829fc28b65ef516f92f4db5626a2f099f6041e32a5daccb8fdddd272a654aec32654ece0322cdf6d74a841f3c2934c73ae455be3dc39a85245afc9595594f651d9cdf3511b6631aeb9ccdbf914617c71d47b16d3dec13d90d0d9f9d06f0b683ed59d25fccff3a324bd860d11c3f513118259f75433bcf61a1").ToPositiveBigInteger(),
                // Q
                new BitString("f350ea98cf1f0049178f8ee9061746c92f1182da3f620b368260a0b1").ToPositiveBigInteger(),
                // Expected Z
                new BitString("2cb5fe670720d52154f1b8ebf9887daf9da02a0ac242395db549b6b774d6f003ea5c490a49a1097ecbb334b9a0005551e6f2cbe8b2512b0ca1f62ef5b8825c7a23bb14c95f3658b7c0058f6f05510958d7e0d37a84609b023274c524ec3d8bad4d08692321f6abc910c46ada0f1a034a30e5d00b9ae4151d804e33df4f370b1315c16d657ac1f44f0dfee86437509eb24efa94e97c089409edceb0267ee7cc93acd4a830ddae34c9f32efd4a5e2946115f2db5edafafed814d275c07228b4094ecdd80ba936f36277a9f76663e89b9e815aaf320ae7d01955c28061c679644d9f27314287b62e7de4197c1c4c2b136dcaa486962587b434e49942a84f9131f06").ToPositiveBigInteger(),
            },
            new object[]
            {
                "from CAVS test2",
                // xPrivateKeyPartyA
                new BitString("a5c5f77df6d5e78340852dd3df08c9b465a7670c7872dc72b84bc600").ToPositiveBigInteger(),
                // yPublicKeyPartyB
                new BitString("1391626efdefd4057a3a44862e56c2e28564deb79f95d09e13427c9aed816b57ac1d56050eb79e6ad3c0b90069c92c23bae1db9557684b54d41f18a8cac3e3e8d28d9ba4b52f20b9e7b57ee16be6780a558b3bdff1b9b3563913ac0e8370c31c770533382b2f0d6df5697265f3f4a4d9e40348db3271571aab321775985fb2eac5acdeba8bf0b8f20e0866f5681fd62ae163c43736865218fe1e73b0d9f9dd3d0c7159bbb0a40bdfd0829f0f6e13a172e2b542f4a31ab18d95af9d17521b53317d7a2c971a3ef248cdebc621cf6e7b074fde8a64cc8ad1181a0d7f87a234464978fc5ec47c57ec24337a0d417c189c8a9d0f44f59cbc370db4b0b7d0181c558d").ToPositiveBigInteger(),
                // rPrivateKeyPartyA
                new BitString("17ea24e7ad1cf773db5f8617e01b16a9a6ff4aad1f475246bcd9f375").ToPositiveBigInteger(),
                // tPublicKeyPartyA
                new BitString("38f9ed24848a0b6e92c2eef92ec10ff0654e0dba051a2427d0a894070459b4b7d438d271d6a6b9f401498f8d3eb0538fdd53f5e44dc9dcc4f872a5764b9bf43f03829e7fce08c445a9639e9b1f0b18e3938ade20677055e12adf2ffff3f3c8e9e3c759c2bc10d5767f937cb52c046641cea209ac55c32509d0b6f733605fb45ab32b54e9c1052fc6fa345b80ccf36ac6dff748a40d6e243f68db8b47d3195878fd11d02afd97c67fad2cb4e2e9b287b591ca743e1a757d78ca5ddeb7d2c6f6e69aab45cf262db00f85dd34ad105e31df1d2e6ec57e2a75f27ca7a72189d995e2f2fa48586954b84f55c814e4dd7640d83c6a827776f1d6dc537598814a2076fb").ToPositiveBigInteger(),
                // tPublicKeyPartyB
                new BitString("5581042f6ef01708b16226c7b21629b1b98737469b8a4606bfa427a5a3185726e2f3fe09534672012ab03aab9edfd12e98aa57264613d8638c5e2ce1304f58f000e39fab9a5538664ccff339012cfabbb4aca7d3f6652a408339ecb45e602661514d8af727a7207efbd04f07a780dab014bf0bfabac6cf6b128d9950062122903f292e296d60c889f5a38437afb08f02a164e0e12654cd39cd4a2590cd0bf5401ed0da935bfad4d589184cce065536633e5da76bd64d1ce26b094ffbb4a3fa754b0676c7358ca05d2941a17e9148936bbbb513c47b8c9782b302c55114d0fc15b197f4191008ff7af5ae791b03e5f08dbfc6fd3f2a46de484234b69696e2c287").ToPositiveBigInteger(),
                // P
                new BitString("e06646f6e96b21701956766eb93c9fe98212372c4c0932f51a1219de49d504b131ea1f65b7d4af964bc86fa86bf3454e1fbb65b2a9824d078ca93dca1a6b0a21d3688229cda2fa2c8b33789a9fb63107eb6d354848b43c61d723a3c4a51d56ae7a341f402a07355aeaec400a03fb0bce010d9c266530f84548760e04048b8669e5d8633aa12ab17eeb01fb3a85e682f566ba91fadcf9bbb9a3a65cef51d47e084ecbc639ef1536a706ab29900bb1594ee639d6adbb4295c6c2dce05c69630be5bd9ce7d110678ef9fe10166f6d85e230eff6850dd8313b856ad18ff556f82724e22c2e09dddc7c1d17c55548dfa72cb52e7ad375b53df3c4a738030a900e1f39").ToPositiveBigInteger(),
                // Q
                new BitString("b7a1971219418d90e12bc41a912fe36e7cc316397e66b4f1fbe44969").ToPositiveBigInteger(),
                // Expected Z
                new BitString("04024d8ad97f497c6b11a9b0f2a8d72adebee7f59a1d21877b1ebb7e16c5387b24c0b7d3861fe49ff7a9c0247ae24e6e8a38fa8741cb2153898c1b54ab538f42972a52feaafa34a2470e7b3926253522ff586c9e14f353f247834443f5e0bf4176d8ad08ab55409b424dd48c1a4beec19dce52f6569d6e72cc3e14fd4fdffe80424a53e0095d1004aae7e221055bf44cc9cf439b859a2e36f34dd17f0eb7d541d9944dad8c9430c95f2731ee78a33951bd8bc67f8a3d1a67d3951ae87d7d11e71d59c75b1e74b9b652e4a0bc373f6fef5b68ddd0cc7c276dc4855a7f4c7bc56ca5a074009744e5ff4bac4f1430a56ae4c75d731ec917a7d9682618d60afcd256").ToPositiveBigInteger(),
            },
            new object[]
            {
                "from CAVS test3",
                // xPrivateKeyPartyA
                new BitString("69dd82efcf663d211bf3e70e2d58f18e0b688c4b61f0feb7cd87eb5b").ToPositiveBigInteger(),
                // yPublicKeyPartyB
                new BitString("5c2b8a37dbd6256ef9b4f1b3486d1c6dc6e83ca0149b4d3b4f49122aea84e327a36dfdd816b4af70c7955fb06674c630f5ad07c0080d85001a8c95293d5fcbbea8c8de7b44fc3c478a6ce3ed4aa17ae3f7a64f035f184a21b3601b71898dc4cf5f53cec068d5d7d4777072bc407e54c9e19f272a8a2c434ac4be02a50a3154d10754988cf006c9c631f424648689e784a248ec01bb259c5ec751d7dd9195c3cb892bbb3059bf15dea780934f22f5912bb9cc3e148fc1224e442623c2d302b8b2781e58f4dede5af63d3f67f01ea1ab2ef6124fd433bdc63f234e8c3b1a6dc517f8d34a6bba09911ece1be0170f78b6ac587dd3e19268bcc2cae6f6dfe5f07319").ToPositiveBigInteger(),
                // rPrivateKeyPartyA
                new BitString("1168b4abe59c0117d42a987f0642ca0c7b2c3431fd4faea3627356b9").ToPositiveBigInteger(),
                // tPublicKeyPartyA
                new BitString("2673fa31d0e65edf658b7919f2ac5c04fe93a53ad30edfa9b708fb2fd9e67cef54ba7caba498269be145cf99b8d7b29301c199d837c9b229f60f63eb77668f73d76bfd744b821897757b3933643056db8fa1018b06ba793415c925f2dcb90f2080d207c729e16027fb7d668282cc1058837d550f19ee6218d7f7575a787c8dfd38e57992b434f1535af0f35a3502c9106d00f9dff54c68b36e15a7eed4881793d71cc313803fd9a2b2ae00b855826b6dd98f8f09372a79ec3a6091b3887a99b56b551044e0fce1203c90bef6b990cb4d821e670246c7a857704f475c40438177459902e6b4349cfc2ed07fb784428ef6b20974af6dcf1bb7910e057f989b827f").ToPositiveBigInteger(),
                // tPublicKeyPartyB
                new BitString("2b7a62a1d48b9d96e84744c3692f7256ce19027808edc727cdfc6de8dc4b1d3d71aee8601902a05de6faf8e11694e0d7dbf50b2bf81b164a75d3222bf19858748db0d7a72d1d5e885d82d0efeb64087c7b375a1aa8bb35fe7b5e667c21ce5334135c0ce904fed206058b9ee385e5d9efb6a62da6e974755db6a58673404c036b7e41b1145899dc414ad1ae84bdf5fb337dbd3a294bc9a4c42ac080c4857198994f8a4d8161e725267018408abea722569a84abe940054abfaf83a0fb29c0deae33a83a0bfc3d15fd42d220ab9ce0cafc978d3768e3e66926943ecfc0e9741c326f5e10dbc1db90ed74e8bdde1ae94f4a92367c5fea10f35f524ef92dd5d05fda").ToPositiveBigInteger(),
                // P
                new BitString("b7bdd8de705bdd9286c8ec2e16ecfdb607cf72561911a7dfbb3f159fbb7b255cfcff4fe07c9ba70b1d159440cc765370ec64ad2927d5732512f8e61c43b3fcc101fdcd2cc040a108986e9c7fe74a6e105c4f4173d7932aabe19779e007166263317764ee4331fc7d74257e1f961a7e95808c1a188dba1eb84d4e545144f04da646d3c23c6c74b52560c057e0d414321cd8a5cdcec78c18869436199f1919b60146083166195725b8bc41144848038061e951b0ef5a6674ef3af189b12424a759393dcc142f8198177d257aa565d534320a2da4bb348f12bf48473ea824a2f0554c947b684b56e87723575ee0194cbea2a18c2a06f454965863d29e1a7b034795").ToPositiveBigInteger(),
                // Q
                new BitString("8b0961c1a8c1c3cd6ac715c20ebe036325f57c94f40c308fdb87890f").ToPositiveBigInteger(),
                // Expected Z
                new BitString("50e33c18ecde0db72e14feb5dd923eef1f4c55a765bb754eb4624c5af0eba4e813f967474c34dd9a819d9869c7426de979db485977355cd75e12ef80832f72117c9bde4cf5d7721a98df2a02ce0779933420e6c1d79c57a71888e08229ac0817e06f8481ebd3ba1252e6d76c7fedd4e946e662bc585b98c28bad7d886dc52d1f0840701a25eee41372fdcdbfde262da275d429e492b36ef61bf23d847e797be9b1180cf0eb2bf23a5033397f17031e8a4ca52d65ef3d69ed4ff38fb8717633e15bac47775a13e0adf5914884e9c7b3042349bfd0dff8213b3bfb0ff1908ad708de84903fcdf538e10f6b54b719eafccd1c7b1e22f3614e3850bb0a090530f754").ToPositiveBigInteger(),
            },
            new object[]
            {
                "from CAVS test4",
                // xPrivateKeyPartyA
                new BitString("20de34509cc419662981335cf1b697a2c5231fd12cde6207fde008e2").ToPositiveBigInteger(),
                // yPublicKeyPartyB
                new BitString("57ecac4d552c94ce67b84b4e6a3082f1d8a870e122f488a010ffda155e2a5dcc5f2892d67b5cead6a4b36529ce5ad11f565837105bc59f360f48541ae35f5d0ebde321e12c059a1fa2076e316488696f9f238ad88e0c14dade5e680f9423c5aecd6f2ef9dbd6e72a78dc71e7f94348d0eaf97c6ea8900a88bde33c3470da66c74efbeb0496b62be97c2870d325827f7c5d8756f5dd071a1ff32b819fb2841dc4aedc84b65734ef08494b65d16fd16d449349ad65004e4e22f3d2e08bb842c024b0015591dfca1318d069e6b9a56b356395d786a3e39a7c9422a0b799966f3ff359824569872c948702666c2fee9002d1b465051ba62d81116b5fd8c07ec9d09c").ToPositiveBigInteger(),
                // rPrivateKeyPartyA
                new BitString("6e1e4a900c0094eb95a3da95c4bf233dec8fc58298be7248be1d9296").ToPositiveBigInteger(),
                // tPublicKeyPartyA
                new BitString("2b7a62a1d48b9d96e84744c3692f7256ce19027808edc727cdfc6de8dc4b1d3d71aee8601902a05de6faf8e11694e0d7dbf50b2bf81b164a75d3222bf19858748db0d7a72d1d5e885d82d0efeb64087c7b375a1aa8bb35fe7b5e667c21ce5334135c0ce904fed206058b9ee385e5d9efb6a62da6e974755db6a58673404c036b7e41b1145899dc414ad1ae84bdf5fb337dbd3a294bc9a4c42ac080c4857198994f8a4d8161e725267018408abea722569a84abe940054abfaf83a0fb29c0deae33a83a0bfc3d15fd42d220ab9ce0cafc978d3768e3e66926943ecfc0e9741c326f5e10dbc1db90ed74e8bdde1ae94f4a92367c5fea10f35f524ef92dd5d05fda").ToPositiveBigInteger(),
                // tPublicKeyPartyB
                new BitString("2673fa31d0e65edf658b7919f2ac5c04fe93a53ad30edfa9b708fb2fd9e67cef54ba7caba498269be145cf99b8d7b29301c199d837c9b229f60f63eb77668f73d76bfd744b821897757b3933643056db8fa1018b06ba793415c925f2dcb90f2080d207c729e16027fb7d668282cc1058837d550f19ee6218d7f7575a787c8dfd38e57992b434f1535af0f35a3502c9106d00f9dff54c68b36e15a7eed4881793d71cc313803fd9a2b2ae00b855826b6dd98f8f09372a79ec3a6091b3887a99b56b551044e0fce1203c90bef6b990cb4d821e670246c7a857704f475c40438177459902e6b4349cfc2ed07fb784428ef6b20974af6dcf1bb7910e057f989b827f").ToPositiveBigInteger(),
                // P
                new BitString("b7bdd8de705bdd9286c8ec2e16ecfdb607cf72561911a7dfbb3f159fbb7b255cfcff4fe07c9ba70b1d159440cc765370ec64ad2927d5732512f8e61c43b3fcc101fdcd2cc040a108986e9c7fe74a6e105c4f4173d7932aabe19779e007166263317764ee4331fc7d74257e1f961a7e95808c1a188dba1eb84d4e545144f04da646d3c23c6c74b52560c057e0d414321cd8a5cdcec78c18869436199f1919b60146083166195725b8bc41144848038061e951b0ef5a6674ef3af189b12424a759393dcc142f8198177d257aa565d534320a2da4bb348f12bf48473ea824a2f0554c947b684b56e87723575ee0194cbea2a18c2a06f454965863d29e1a7b034795").ToPositiveBigInteger(),
                // Q
                new BitString("8b0961c1a8c1c3cd6ac715c20ebe036325f57c94f40c308fdb87890f").ToPositiveBigInteger(),
                // Expected Z
                new BitString("50e33c18ecde0db72e14feb5dd923eef1f4c55a765bb754eb4624c5af0eba4e813f967474c34dd9a819d9869c7426de979db485977355cd75e12ef80832f72117c9bde4cf5d7721a98df2a02ce0779933420e6c1d79c57a71888e08229ac0817e06f8481ebd3ba1252e6d76c7fedd4e946e662bc585b98c28bad7d886dc52d1f0840701a25eee41372fdcdbfde262da275d429e492b36ef61bf23d847e797be9b1180cf0eb2bf23a5033397f17031e8a4ca52d65ef3d69ed4ff38fb8717633e15bac47775a13e0adf5914884e9c7b3042349bfd0dff8213b3bfb0ff1908ad708de84903fcdf538e10f6b54b719eafccd1c7b1e22f3614e3850bb0a090530f754").ToPositiveBigInteger(),
            }
            //new object[]
            //{
            //    "from CAVS test1",
            //    // xPrivateKeyPartyA
            //    new BitString("").ToPositiveBigInteger(),
            //    // yPublicKeyPartyB
            //    new BitString("").ToPositiveBigInteger(),
            //    // rPrivateKeyPartyA
            //    new BitString("").ToPositiveBigInteger(),
            //    // tPublicKeyPartyA
            //    new BitString("").ToPositiveBigInteger(),
            //    // tPublicKeyPartyB
            //    new BitString("").ToPositiveBigInteger(),
            //    // P
            //    new BitString("").ToPositiveBigInteger(),
            //    // Q
            //    new BitString("").ToPositiveBigInteger(),
            //    // Expected Z
            //    new BitString("").ToPositiveBigInteger(),
            //}
        };

        [Test]
        [TestCaseSource(nameof(SuccessSharedSecretZTests))]
        public void ShouldCalculateCorrectSharedSecret(
            string label,
            BigInteger xPrivateKeyPartyA,
            BigInteger yPublicKeyPartyB,
            BigInteger rPrivateKeyPartyA,
            BigInteger tPublicKeyPartyA,
            BigInteger tPublicKeyPartyB,
            BigInteger p,
            BigInteger q,
            BigInteger expectedSharedZ
        )
        {
            var result = _subject.GenerateSharedSecretZ(
                new FfcDomainParameters(p, q, 0), 
                new FfcKeyPair(xPrivateKeyPartyA, 0),
                new FfcKeyPair(yPublicKeyPartyB),
                new FfcKeyPair(rPrivateKeyPartyA, 0),
                new FfcKeyPair(tPublicKeyPartyA),
                new FfcKeyPair(tPublicKeyPartyB)
            );

            Assume.That(result.Success, $"{nameof(result)} should have been successful");
            Assert.AreEqual(expectedSharedZ, result.SharedSecretZ.ToBigInteger(), nameof(expectedSharedZ));
        }
    }
}