using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class PqgProviderPreGenerated : IPqgProvider
    {
        private readonly IDsaFfcFactory _dsaFactory;

        #region pre-generated PQGs
        private static readonly Dictionary<(int p, int q, int shaOutputLength), FfcDomainParameters> _preGeneratedPqg =
            new Dictionary<(int p, int q, int shaLength), FfcDomainParameters>()
            {
                {
                    (2048, 224, 224),
                    new FfcDomainParameters(
                        new BitString("daea7c71588bfca3b8ed40e103ce87e49701d228822cf4cd3a5a5fba7f07895eea4928234724e50521054ccd84dcb24d65da6f940d132ad08261fae24605870c7ef2d4b68e2f55c3af40f296ebc5782c163bb93b8d37c8cb22e6f128012d1ec0e0e3b43167cfe7050a396a1c96cdfa3bdc3116c3de1a0970f6f390facd10b7999c47a833ecba92adddf401a30bc13b34c4040d025e530b06986a2fb7e308c4b5bf25a905da6504c58313d2be55bc2aa79f92d312eefce1c78518635b7fc6195dfcc30db761b25c60752cb544b666b7a1bb6db7b7aeb92037d9e7eee6d925d0c5bc8a9caefc28924cc2e82a4052812e508b5ead44991298f4ee2aa18c15602217").ToPositiveBigInteger(),
                        new BitString("f3c65fd74cba905970d1e32925d7fd88e2412aa88c1614e579890a75").ToPositiveBigInteger(),
                        new BitString("61376016993bba8a477e30c8a78c5846f02a6a8d7a80664c26fa815cfa7243a528400f86013960b1e78d9ee976ac5e63bfa71304ed85d82d589a2d07eafa35485922b9d02b0b8b4f1a0c61f86aae0eb0c34ed89ccebf07dafcefe9cac55711b3c66a8dbf4198cb9b431e05680ac23795df7003f8069716f05991346af46e7870fb944dc2ba01fd539f833f7b3ed80be6a8c3923534666c46a49914d6cf15055f7a7027034f2f54faed83ebff3d445044326cb97cee3237df1cdef814991f66318dc2316782e851e9baf78bc969af376110e514e58a9bb0464c946a62f4eac030a17c01f5c1488e4a7edc3f11d6a224d2a7c06eed2596819178f6a2564f048f81").ToPositiveBigInteger()
                    )
                },
                {
                    (2048, 224, 256),
                    new FfcDomainParameters(
                        new BitString("bb8b7cfa73c1b037b37eafd979421bebc81b973d0dceaf5888c71c87261300b5f680490354f6ec0ec65bee5021c3faa8c77ba8e6083162735e9b15cc47f0881c7fa39f7b332da40866304ba935d5ac185932031ba26cc00b7031b65ba74f64710fcdde5efba82dd353fca857464b0b376d5ef396c5ffad5ef560b81e925dba1f06e34787997a0143dec90861f113818c41e6cb52d7dd5f1914b5ff9e486872b5ff13dddb34dda1e1f96f6a5d64efc3b607661556cf73db23c05971d45335116876ce9f4d03ef98d6947cba00003ffa9b04ce7b9432706377fa0c36697773781c5a8a63224d858410a3c8e6088907156ed66e7ce5c193916b625f230d844eb70b").ToPositiveBigInteger(),
                        new BitString("cb05507d266cad80e324170c8514d6adf60a24e6436a3b10e39d246f").ToPositiveBigInteger(),
                        new BitString("8f935621e365937c954c020d35ddda30a9c407cdee7e42552f4204c14cf0680d8b0cc55ddd87a2092d4a922bda044928619a81772801b89e09fa4be4514db9f254e787f06d4dce31c4fdff8953eb94e4e66c36abe6f9045c439a5ace23cfcd4bd3316d40b48112e93d183078c30e8d49a375aae7b2f667c12b6057bfa1731b73e10cca8009dac7b1f6a44f9d03b67c84915603f86efbade819be71f58dc0c14e4924acfa63081c4304a35dfa4268dd6ef146be94d3cb7aacc010e210da137fbcf31d0f743229ee0c9f6632185bd4aa054ddeafacb41c6d36ba57267eed9a2c28337fe69c9e6f8ce90c89a904ec87e1e5d6b396937748f3ab0148a70da3a7126e").ToPositiveBigInteger()
                    )
                },
                {
                    (2048, 224, 384),
                    new FfcDomainParameters(
                        new BitString("adb40c7fe3860fbbb498f904f9c1cf896d5f9f14ebe7d2358e8aa53b0c3f6e4fd03c74a522ea4faf6bba636ea878a146195557e93a8bcc82ccbc5e7613f5952d34b894aad97cf22c9fa03ac1354aef1f3064c20f8f7181032cf91709ac766ce8559fc790221049b2ad04322045a5b4e0e8a55d04a14a22c54b0227de2c8d50c58cb768c0afa7776cdf7b74f2483ccdce65dd1e924f21c53d5e0631a3b69042bbf3e782b75316b113e4d2d96622dc64334fc418aa98d895b9ae0df03ddd30a4458f9ba7bf0220ead505bf4a52c1408a69d261acd84f12316dacb151905e90321072eb42a374f74906d0196e886c4fc1aa4ef92aabf938e118865741afed84d45b").ToPositiveBigInteger(),
                        new BitString("a6863650928a7c1b1749f0b22624a5febfa3f68a7347ee5cc1329e23").ToPositiveBigInteger(),
                        new BitString("856e1703c0734efbc416a99a1a87e69f981c5be0475666362b74d637862e312738066682b9705c98b72dae9a404837975c7610b80da8a669443656474f94e05db89679b9277b686e4d289e924a128a028fb3369651cddae58a3071ec391181967e1e24aa9b1252d1d9680fb5b75ff91a4159c7cdf9950f327c2fad810e6bfda23d9584ab487e753fc6b14d34cfffc6b854bce50075e4bac8c26ade037eda85cb716fa4903a12461f635c2e649ccb8fe1243a987bd1bc3f7af08cba0fb64b6eae8f71e0519322143f53f56b591251216902ae480916f0cb0d50f6b9662d8a9d6d16efe6cf9c62841112eb004b4193710d364ecd87ede7b831b07f3459736c8051").ToPositiveBigInteger()
                    )
                },
                {
                    (2048, 224, 512),
                    new FfcDomainParameters(
                        new BitString("e4e85db7df0c5aa669c26dfa7a8920c0580975fa36f761347c081f9631c120c1efa7e1165859b1564f7efa2e8825e7cee0110721dd985cd6ae920ff16459a23d3df0f7027b0f240364bb81ad3720465ab54663d0e2d87eda05e1973fe4ab1e8a0e7036c016ca11190b9642c3ce45376c6cea8160b2f7be3ee84cc0bcfea157986a7daf58208ee0f4819848a2e2dec3e498ca4445afd8ccd780e5560a5254d9985e926b384109b75f0143e4af0464dcf3410b781e8d4c54814b92ba3ebd0b072c19918b4cf3c75c32280ae86ebf8a14d9d215ed4478f9945ee7cf2a8ffee58ca00519a64971a00cd57d654dc25763a41224441a4e9e9fc6c1df571c0c0231d4b5").ToPositiveBigInteger(),
                        new BitString("a1b3e7100f528ee0824f2a5a6f3b022f1adfbff02307f5fcc0224f5d").ToPositiveBigInteger(),
                        new BitString("1ab425960434544374ca03edaf1aea9adc3eacf233d2fde6a594a58c1bb93f08c8d01b7ce7160fd439a2e6426ebd1e8f63c2b96c679315a7ae8a255b4e531f8379cbfe561a77bb695be31bf6cf632b1442f93272475fdde6e74ba02dd8a6f23c18748e13daa99ce57a952293d490e9330ce4b4992a0edb60ea88f494f3a424091cf4f62ac8ae0493ddf5f57bd1c14e93b28e536db874dd89e69310cc70a400727a017bf53c6f5cc54abfb83ed07c15aceb055060f62b23f2881ce771b0165338a23487c9bb03388262f97b4cbea9188b7f6336c59091fa32cc4b885d495518873089401605ecce5d380e903f0f916a968dcce0c56074cf3bb1cce5efdcd850eb").ToPositiveBigInteger()
                    )
                },
                {
                    (2048, 256, 256),
                    new FfcDomainParameters(
                        new BitString("9a1b11f4b5a608658029fc3ea4499bed183cf1f41d226da1e601c2ec17abdfacec11e613f31d07659112d7efafa7f826b58abbcd3c59c66c5d895ebcfbe846dcf73abff5182cb3ca212bafbd8d1064628f8b986a6694504a1df94d7ea4c92f5ce7af0cae7937cf054faf0ddfe463bd231084a3981563172d8c04e986d776fc87b58fa24dbe8c8c658b49f610f46efac8999f2ae90521e8e3b2975c0c767324ba5bd7000e8f52b35ffb4fa1a6c77a74f1533abb8391d1fb065f84b45403f9f0adf539dfb580a148e6df99ec52e13f5a7c284af0b7662c5b23bd3c9b072c588ee388bafc3e4538fd7c71d394232b7e6b7e03062652c473842de9b3cf7c83ee3827").ToPositiveBigInteger(),
                        new BitString("adcd738f2a7a3552b39b7dd5d1c9af38d042c6dcf740663309aa5d3ac7ffbafd").ToPositiveBigInteger(),
                        new BitString("8e269cade14a215b080098aa46df67d9e6ead0c2cf21578f34bcd6dbb03ad9b75ab52b215ede9ee3a5c1feae2225e0d6ea3eb41dea9419244b529c356bc677024718b546d1560f0337813d069a933b5b27e24644709424e2b62b0f691cefd3b5bcff90f80e07683c479702dcd487df60631afa853059d52c25597728180438a4c8aed933e7d9c57a36a648a6c6edac2b4518c52b33c8a1876107a0c6d2f53b9aadcc584b2afbddf2a82ab09cb1624b4df2da3f5b1e0f412de8d063bf2579b1ec94369d5a1944025e2fb33ebfd25d98b167f390115c356bff0538f7c934992174c20e2deb5843b7bcc48026105e48e3f21bee4bbc4c2b8df45cb963627819a96d").ToPositiveBigInteger()
                    )
                },
                {
                    (2048, 256, 384),
                    new FfcDomainParameters(
                        new BitString("c261fcdf14f9d4998583a06b237115e16b59f06eb55422ca951a274942c3cef2c1d2ae4afbe20092d99c71da65218c70a0f1be9125e9c92b201876eaa89d083ba2555d58fda7bfde419b0ef20722518034b929b74cbc60cc944b54bca5b9db6844baf81513649821c19eb008e5df21351b113fb5690dbd1c98a0ba72ed8d4b7a787a5215abe825c99002c9c48c01ffbe0fd3f11c8dd7988ff529394d283372a22176d6d26f8ab687c60591a6332297f859a411329fd027402f3191f172372068926ed97839fe480fe65debcc756584058ce54f4f04834c6e843e9da03f7cf72315b049140d769c0119b67435b05cf20ff61d0583b00b001c4997a70c0b8b14bf").ToPositiveBigInteger(),
                        new BitString("a19ec84a765911171178fa95ed3e5f18f31ac888b50811927005708c02ab02e7").ToPositiveBigInteger(),
                        new BitString("60d5b55aa8e82c8b41f26a5cd483bd270617de690c9884dbbff76216f9281332f239dfc9287e2937d5146c969d51f08fbd768814d67930c2e0eb6f5a4b7d9cddee57537173eb5a2007257930d6395b03c0df73d967d2bf7a378c9fe9b8b3f8737edae6b3535b7e0aa1025b669f1aec01c87201e34e87df33f5f22d7b3a28faa5ed956068d8514b08a4a55fd433db4264ec939aa2c334c099c79a7c6df92feb7f09964d69f07fb1a79f7636c995169c092d5a82789c3d7252f76aca3ef10b5eafc4720d7e86bf0b05885c57e2622b00eda0baae4cc1d86deda3b237feb885087ff6ff42228b193aafb49ef3a7990895be9a7e1e97fce8e8b5f9156ee1fdb8bab4").ToPositiveBigInteger()
                    )
                },
                {
                    (2048, 256, 512),
                    new FfcDomainParameters(
                        new BitString("cced909476ad1f99b22b4dcd9eb2d81b7bc70e60a1b6ad4e1b0fea8e72a1dde8c6b4aceba183bb497581107982bf192bf16e605629c1653ccdd36679910d706d05fafbd4c1950a8123440b45f16eaef8459c832f223ebb3d873d268ab128c4db7f1c1815cbb2f85921bd4d1706541f0f1833a8de52ca18b102a1c812675b4ce373ec079b8469056864971c3933bca7427c795f7cf75f2482763d2645f255f647b13a025121179f993530e5b1a353d7018b87e4a1ba4e304a34efb1ce221f8516a03ae8d6bfff0dc6d26fc01074426af2dcec2db01e44cfd3d45478cc9bfd0fce52207058878e6df2ab1c6c049834e3e0fe6c14a61d7db472e0c014c272ac5cf3").ToPositiveBigInteger(), 
                        new BitString("bf826e8cef48386a338569624fdd9a04bb35b8312fb1102768fedb09d3fb15db").ToPositiveBigInteger(), 
                        new BitString("778f16e06d7382ea9939f8c29f6ff821abd46f5f463412514fc3e3bcbd412fe4315729aa34bba1181623de2fb53b7f69d044c82c0b63d1d7fa658e9a4300cb84f3e9b41e2e09ddf26cf48782cc0a6891808d51a51cb0f26e3d25b993b2385b58f57e5bb5ac329acc39858579878e983bd5074b551faf7c115f99e995b26541bbff1a78786c5344e062f45ebc8f00ce49a94198980e618f71311615922b1aae491d0f8079cb4fbf34ebc24da45516cc610d385d0a110e9c96a59e2fd7dc688f291fd72f2ab489838a218ecc9e7104fdf95c026b34407254c447e70256848a322f19b55f6eea6ad4e8457227403be121a017e34e64e41e30ec109dac585a104765").ToPositiveBigInteger()
                    ) 
                }
            };
        #endregion pre-generated PQGs

        public PqgProviderPreGenerated(IDsaFfcFactory dsaFactory)
        {
            _dsaFactory = dsaFactory;
        }

        public FfcDomainParameters GetPqg(int p, int q, HashFunction hashFunction)
        {
            var shaAttributes = ShaAttributes.GetShaAttributes(hashFunction.Mode, hashFunction.DigestSize);

            var preGenerated =
                _preGeneratedPqg
                    .FirstOrDefault(w => 
                        w.Key.p == p && 
                        w.Key.q == q && 
                        w.Key.shaOutputLength == shaAttributes.outputLen
                    )
                    .Value;

            if (preGenerated != null)
            {
                return preGenerated;
            }

            // Could not find pregenerated PQG for options, generate using DSA
            var dsa = _dsaFactory.GetInstance(hashFunction);
            var domainParams = dsa.GenerateDomainParameters(
                new FfcDomainParametersGenerateRequest(
                    q,
                    p,
                    q,
                    shaAttributes.outputLen,
                    null,
                    PrimeGenMode.Probable,
                    GeneratorGenMode.Unverifiable
                )
            ).PqgDomainParameters;

            return domainParams;
        }
    }
}