﻿using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA.Signatures;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.RSA.Tests.Signatures
{
    [TestFixture, FastCryptoTest]
    public class FailingSignatureTests
    {
        [Test]
        [TestCase(SignatureSchemes.Ansx931, SignatureModifications.E, PssMaskTypes.None, "0100000001", "967dfa05a939fe8907aea3a15381e7ea75fd6f07d9f791005296889edbb5cc66c6689dbff09254f094d21b66ab7dcb990f2cd0c911ffd39830a7db126291820089f1c009369181d248732b1336ae83914c5306169ccf45bfbd00228309fbea28fa033ed180f1d8b1a74736dbd92b05a30f02d4e3f1df6b73df5e5df3c9a287c1346275554f0e07c8128d2cf8dff08d8038814545533fdb9aa78c2e0cf8abea18a0b85d524cd436c568706801ca27b83d080ba139743e30f873a4b4c7884540e30765adba51aae09158c61b01baddcda7b073f72f1d26718dbe9fa40a7c7feda141435f663f77a786ca0203af168967827bfa8ea8af53a69fdd9cdc77c9fb71cd", "03420806d72ab04607ee3c0ba5724e48ce5c29f9477166853e82887c3275de3391cd62c06f9f7273f87f91a6fcabf7ae597630e17a712a98b9d75c959c1aa583834a3d43d7ad6307995d9c109ed137b582f10e5439ae1ee2ba060b60f281aec9b3992adb488192fa53adf37fea18bd3965bb4ae24c9ccd67a5246369c25280b7cf1b3a60053296fc6c604c39656105ef9ad53927ade900fc9d2f092dde2db9b4106b1f5b1ff89e03e9a8f6c8fe4d8e8bdfa00cb7be9bfc2d4c9156ed311557b9c194d3d169c975fb446c62de947501676bbd0ea891ded9d1274f958b6b388cb03f674bf71b44acec588bac12ee3238cf605e518eac40ac6650f1e9911c5c6a17")]
        [TestCase(SignatureSchemes.Ansx931, SignatureModifications.Message, PssMaskTypes.None, "0100000001", "90c2913e83f4eaeebd4d36c893f1cf5c713c2e56a9b851c27fce57023c59058441c0829bcec54e0aef718e0830c26a7a9ab40ad8b3090fd062a7d2ccebce25e3e1e6292497a9751d1f760555902b11ed6f0e7208dd3686070c078a281ae859eacb453094c7f91a746bc0a43a8a69acbaef724787b52e99d625da094818e90caa2c478d10d35f4b3f50734bc61ccaedfe4d1051aee1cbe44145adcc66fbd6f59aa1a8f3e4d3c21907528155354bbcea920c08a19db8ce71749d8f57901764e93d60df46163ff4cec0bdfb9f65c7e0221a83d264b705d2d16d06d3278314a6e30f8fbe901b61fdab6d08172df914fee969dad1955722702e974525237d939b8f3d", "0fd8c198af843d600d44e6932dec598d68e0215385f038eb42016d99e531a37667cc8c6e0a3d12f4a59043c21009dc4b9b8e9f6b43c9424c6a9427befbab4e75babfe13e5cb897773a6cbf6d40785c23c2341ebc0341cd3fffd5e11e510e6eb9e7e31988fd262e1b05682966884956958f5eb4eb8cce99ab09a1a18c58501b3a2128609f3ec07fe080a7aa98a6991e6043aeeb33d9dc1446c3dd8e61808ed4299057e0f119dbdec67d2191d66cfbbdcd88eb8e4c7182d44e616ffd7d638944ff2f083725d43bb30cd059d88dc3d9b60e409da8c47cf2e84f8b884db8650cb4113b5ccb8e70faed37e0e74143a075456a8167cf5f8321d56ee6c05acf26f13759")]
        [TestCase(SignatureSchemes.Ansx931, SignatureModifications.ModifyTrailer, PssMaskTypes.None, "56ad21", "8f9c6891089116fbbb655c062566836c505a3ec11d0f82eb3f18df85db0b37183e37183ac479fa99d937ccbf20cbf2f09d60807bed82bedec797cbafcd3cf908e58745a03726beef48aca0312e161b9087d9b99a314f82c78dfa73f569e2a3a1168a4a5560f420ed993b8bf051b93f50554a3c7b52ccff460b4b3ff75de598b038ed870f6a79c3c74fcde791b196bfb754bf3dd2fde8968b777a784d3e283067c1dc21875e63d2d6c16fa851f6c3306e362d0ec72190cd97a8950b6a09b2952038abc5f07ba33b7854efc528744f6044de8d3f3368d5103bdd538d159b0e924d2d879190ba75da6bc2924f6fc30e4436dcb96779d6261a7b9cc8a59add807851", "37987feecdba6cd23135e362920dcdded6a4ee7e875c347217f5a656588cd11779c1e4a4805807ca19da1526e3d3b5730d336a8d2f88d723487097199649a9e61f4c16e63843362257df5544e4cc2b1a20498df0d2febf5b61d00946d43f3a8a2b64e62870be8c735c14509169f60357eb6274e01bf4dcc968de0aa1047c59eb55ad8c11930bfd5d6343efb3df813c47d56a3fe9977846d80b87ac029327cf298ed458591411261609afad86067aeb2527759fa5c3ea101baa5c6ba0baf6de95c3de8b0025e628af2cc9b7e88f6bf2641ab131ecbcd7b980d5408f899d47d032f2f6cafc06429d23e6e8fdf47b4cc3b379dd8f2069d7e4ed96efecb0c451945f")]
        [TestCase(SignatureSchemes.Ansx931, SignatureModifications.MoveIr, PssMaskTypes.None, "a05095", "bd25c7275a544e41e00a681663544f8d857f544c3ad959f0597c273064aea563e7fb72d7518809c7d2d43ecb7d6be37250cde972408b07a1a578baada93e988f6b9f956901004ad02be1692df156406deade97a57d81d5d131383e420a0a9e69c09c38ecc1021cd36c1303b4bb0656d933685e6327dcbb9cf0dfdc3af00c4650798242a22bbe83008dc3c937cd98d9bb32bc626d7095f3e120b99d9fa5d93967202e4c62a94a58384ddaf660234032e80df0e983e6de94f6751844b34e8fe0e2a54f7190018dbff4cfc6f4bbc5a0b45dc561f154921f5ebeeb5bb12004e55dcbe56ecf1d294b78b50ed478248e37d4a37960c34db95ce3128d9fe009ae2bf51d", "40876a9103fad6962393f73e9b8d9536bc3f0879c0d6917f426e5ecaa2f1f73acc399f7eba1a91d0806ae289889661021d2267be576565ed32dd4a6cad133ea71dbf1da7a3901f3a84daeb89bc26811fd17eb764ff9e48532bf80fa25e2519c01f903d628559d865e4d083e3a1a05879c6790050c6a9830f68408475a619e2c101aa34b791e12c0617058b264100fcf08d16c1b4988a0dd910b66fd3657c5e7cec71a31d2d4484655b997a213d17ad70e33fc874403fd2973f44fca77b83ad9cd1a6791299b5599a4f9eab8ef1c6694386466663608633d4ed1b5dbffaa63a97058ee951d155044e3ce360b7986b1a838a69e025242fd694172d02cec1b9570f")]
        [TestCase(SignatureSchemes.Ansx931, SignatureModifications.Signature, PssMaskTypes.None, "0100000001", "e98d3bb4e2e0c8a055c77f9810ecbe8243583f1d3fb1392b35e24220c422208c2ad462e13fa29880361e0a75fa970798482b21ecb8ac8a708697cf1cc0ec4256a07abbe8ffbed36833d23bea73fb5dda47d7346c5b234e55e0ae5bbb1ce13543fca4c1b6a5916293940438be5f5a864957fa95e1b143b44a3d311ed1a6d5acde54e42d39792fe35e5bc42de88a59bd8f974ddfdbd63f58c0ab85286017580f4fff6ff21464f2f594f55e9f42eb56af3774fb368f27c3fb37a5eaaca553eac7cd66ef450066247122a06d7c7ddae9014db664bdaaf72745064a9485c740caff1d82493b437fbed719481928b2f0962108ad2b5ff7a73eeab22141d8f5b54f83b1", "070d24cfe60d112224bc53387971ff2b44041da48e9f5ade89da9802463edf356267443206519b4a2acbecf1538c2a28920d34211ab7b234f76798f99d392f69c67fc07705ee52be219fb91604aa67758a22aee899a8b95fa95d60ea010346bf6cbdf31bb3fb7bd63784f65c9cc2c632530864c1eb4381488c3f61c98e25b736c769b9fd0b99a0c1b576bc43c545a53dde35be00bfe9940bf11f86bfab0e6409a73d509b11f1a0eff582826d178b88d1bfd57576df8c4ac4614004425bcdfe41dd02ecea447826eab2584aa9bfcc7e405b194b2ad53f5c54185f312a536552bff5eeaa594d7f4c6369cba4ea185763cacad5053820452dbb17e7f441273af3e1")]

        [TestCase(SignatureSchemes.Pkcs1v15, SignatureModifications.E, PssMaskTypes.None, "1a16c9", "851d941e586b0237605c8464f2608afffe934cdd54ed6807e13d4eed6a776e0d0f43d4ac368a5237ae67c5710091c79af41e7c5f2f6ac32015ce3799caa6618a20407ddc93a36c14594b8921cddad0af54228046d3ba8e52a4367e13ff5a1f6bfbcbe964e0ed0fda6421f76803f3be56f3d166114d1ffe944089778f012b0c8016a7129425d50d5ca55cec17eff37bb80baab533c9a236ffc790dde253b7a9a095b1fd670bdbb6040e41d811c43599b237f3147cc60c35fda9c9cafe40c468a7e96a7d231569f2f426ee1b0c7d576b7c9c0c84d2c38229e075e95b132814292310ba076e45581b261169fabb6534debf6f7ef3bceec382c9de1a7d3d9140546f", "02d791a330741c92008ac18e675a465c4978726e5804e0fce4cad335760ea16941e9966612c5bb43eb4bc8cb5fd67952634dea2786db41d1b82a28a7dcb5eac8fbfb947eac5455601439a2ff23367c5e526420225c3d8277a4ca08f1906cd3430d37fc3d9f3bc9730119851533ed83a070d369e2a22162ca25ebedfd8f4d7c0e0a85459d8c9a75cd8b02ca463af66703fb9e977ca6e9a9b2feb04ef309ecca8d27de528a8ba044a6eda5cf2bcbccee1fa1bcabf5a69cc7c452f9e50654ade048a72b35d4d418e58e972547219098520f131c57707c3e89c62e03c09157340f1f3d4bc66cffb7df9ba7a0262c251d8bf485da05226bd4cd0188b7fdeeb74e7421")]
        [TestCase(SignatureSchemes.Pkcs1v15, SignatureModifications.Message, PssMaskTypes.None, "0100000001", "b877ab7aea8eb23ec5e40d2553c1b7cdddf504faca5d215c1fe7b40b106edbdf2d1d9d61764aa8bc4375d2f83f0038e3acfbfc46e4398ad10d7633fdf2b3d15bfc96dc4df280b83059098249ca26032c6503b44de527dbf94cb727c2dedf4c921a495fbf628fc2555b7a82575882ae1f480b2d8ebd9c324973dbc2a12d99f98a4328cd5d18941c5be59155feef80e6bbf8ba18059e9f460feca2c2c1675985e5533120c1937283da4ab3cdabe0f8038d17c60d19b59fd33e1645b8c59f30583f9b9f94de93dcfcbeb0a8c6f0ca5270048233a1e22d8663e1842ae12f879fe6f9b8755d64a212ddefc4871a8f4bfa30e7a9446a7088c1c6aceaede89087cfd1c7", "5aa23ac9499898036c71375cb43226bf42bc01d165f54b79c598259c93eef9f382e23690d7a4fc1ad5dfd79b449886246320626f39e4c597c7f0c956f9e5d2444f6e35e95191ad7a8d9daa9ad998f867437d1dd59e1f067fbcf5ca3c150d4d10771306690a4de22b284d91a37e720d366b868089734fc4e629cff0ad80f59020090969fad08429f91f755e3cbdcd68dd2745ff8c150005e35d8b99b2e36036015d9ec7be76db5e3253ccf67446dc44a664b76843ed0113dc6812e5855059f9306ea02160808675ba481220370e9124effaa4bb76b6979f162b248a0d564f032ce1ef95b04d608c9b4f35b55b59edee1c13ce193c59295a15d078da7279800c19")]
        [TestCase(SignatureSchemes.Pkcs1v15, SignatureModifications.ModifyTrailer, PssMaskTypes.None, "1a16c9", "851d941e586b0237605c8464f2608afffe934cdd54ed6807e13d4eed6a776e0d0f43d4ac368a5237ae67c5710091c79af41e7c5f2f6ac32015ce3799caa6618a20407ddc93a36c14594b8921cddad0af54228046d3ba8e52a4367e13ff5a1f6bfbcbe964e0ed0fda6421f76803f3be56f3d166114d1ffe944089778f012b0c8016a7129425d50d5ca55cec17eff37bb80baab533c9a236ffc790dde253b7a9a095b1fd670bdbb6040e41d811c43599b237f3147cc60c35fda9c9cafe40c468a7e96a7d231569f2f426ee1b0c7d576b7c9c0c84d2c38229e075e95b132814292310ba076e45581b261169fabb6534debf6f7ef3bceec382c9de1a7d3d9140546f", "02d791a330741c92008ac18e675a465c4978726e5804e0fce4cad335760ea16941e9966612c5bb43eb4bc8cb5fd67952634dea2786db41d1b82a28a7dcb5eac8fbfb947eac5455601439a2ff23367c5e526420225c3d8277a4ca08f1906cd3430d37fc3d9f3bc9730119851533ed83a070d369e2a22162ca25ebedfd8f4d7c0e0a85459d8c9a75cd8b02ca463af66703fb9e977ca6e9a9b2feb04ef309ecca8d27de528a8ba044a6eda5cf2bcbccee1fa1bcabf5a69cc7c452f9e50654ade048a72b35d4d418e58e972547219098520f131c57707c3e89c62e03c09157340f1f3d4bc66cffb7df9ba7a0262c251d8bf485da05226bd4cd0188b7fdeeb74e7421")]
        [TestCase(SignatureSchemes.Pkcs1v15, SignatureModifications.MoveIr, PssMaskTypes.None, "c61d63", "97acffd9d01b0049756b286389387f5ea08b5cda408e47e8efc6aa11f5ff90c2a1953154aa360382a80f1d6306b9f431c266b4d9ac00356b3a1cd26786e00821dca1c665f72d777a172595607339fbb479841a3adafb137c6759b4b1edf6d909444e9ffe7769ea3ef2c31e3c459f30f8611e97fa6f75509aa4116a083036dc8655da8cb8837b10d45d0698413331a54349fdc150a19e3ff06446e2e3c9ed56d821004af72e97459e00237ac0b1950ac6e0effb93d395a1186680a69e98d9f74f9a83998bab6d2a735aa0be39201ebaf19e99ca33e5434eff478bc6be9bb18a6531ace2dddd3e2dc119a33d61c3e1ddbadc2205a3aa88abf66c7799cb271466bd", "24337c7ea7387ce0e521554de5e7d4b50a5165f4780a926b4a1a5261f1d25f4d6e40f8026713ae17c5aa4825016cb8ff5d40985be553406fb973eb49690918a50ecb10c5e8c6cadafd82d90ea8be8cd5a9d8b4e050367d0e2a69893b5976114535afd8891d20aaaf2810c21156242910899e1bf29f862795371d4ffc7e5c45adb9379415000d1b791d11c1ac44d835ba353eedb43c448ae1d331e0bf352dfe1c56ab3dd6757eaf4917ff80af8d96cde63e48dd7aca148ced6202948359e6a69c484f5b0117eb986c0af67af9c140bc1d418eac97993e9e65db55c1aadd5fb4b6bcf4c35986a378a2bec63fe3c684e7d9a0c5611d68cc6b989673557e7a6fe775")]
        [TestCase(SignatureSchemes.Pkcs1v15, SignatureModifications.Signature, PssMaskTypes.None, "0100000001", "9b80237a3c498c4ce9e2dc5e8b2c061e6a0bf7f6b0bcd9be906a4a630c130068682a184b93b159ee400edc529014f57a2343fe54403016a3cbb31e81643287156c302156752452fff60224214a7bb3f89f80de5aed7ffc4ae64f7036b0dcbf9ee810ce4c50d6f993e09177c423d9ab6ed7695e58e156b2005b2e2a0957c3fa8ef1faf426dfbda1c565aef25945c3d3b0ee735baadc4cfa4fdf573b6b8f3e2a4de81c6257069bdad149af59e3b9b60ced615d1c9c92afc16d07e5190a5ba987f623016e122290c859ca5cd826279825b15c86d8becb2847b065aca5817d3f75d97e9e8a7544d4201cc1de4359aec668260ff8a7daa3ec3309f75dab0738c25a19", "178559fb6fc772c824b058bc606830f4e17e9589c98f2a1112c57043c2163ecdb05af876abedc3dd53fa5b9e5acd249b8427fbd1e4f37ab0aa01a764b3eba8bdc3443e23a7347549606d7352b4525c47ab845108896228aa62b07f5921b670abeb344dd058d195297a775f14611be13c98c4b2dc09eb80a90977d18ef6247cda82231f24b212f478aa5eb4cfc908835e29796c9c6928e158e99b24c5b35e42f22236509c3606c820efb57b24c6141fb5b84d5d2ad60c818c50623d364a0eceb3114ae47d3d9aaf368841925867638a3f74df20c0157dfbe0b9511de8293698f08bdc10fbd66611b6f61fed8dc0214ee07d2e1b320b9e834b10e1dca813a38ec5")]

        [TestCase(SignatureSchemes.Pss, SignatureModifications.E, PssMaskTypes.MGF1, "0100000001", "a628a77126aa97bab3f1dc05e07b11a2fa8d7d14ad0f9b89b23b95772468bc16fa394dd8bff635cf9aecb375eb84bdfeafb2c4e30a76ef49977f5f80ca386463690c73a52f1822f185f03b09681392690c9027705f028a60ee2413c3d389f4943796fa637fd77ff7efa401fbb859e856ffbfd679f21d81c62a924ab3c019968796765a335bdd7ec3826464d6df9c39975a104e7df1356bc3185109982d3751678122038747c8c38baa681296083bea454b896ab95a09abc296d6cdd5253d87a88fee36d1efbbe4966222edf2a0ae99979cba3c91f9a441061b3a70df6529b5bc8120c3f41a780aa695a38d3e413d367966aa82f4139e9765793938c76f327497", "13bf0a8133f70e4094ace4f6a7c8531a11a51e0fcf80490fa4ab6e06f913e256eb7101ecd71553b99ee03c503d2f4af9a83a9ff4892b2d54dca69d4c6274db29da202dfe69eac37a58cdf9445343e56c5f8ab8aa1df27cadec5b39768802cd9f55baa0a4c17db0e46397a85b4886643ea0f1e9aefc11f925ae8c616b705e190b6e5770e331374de1db7df3a8f84ce1689a5d47018b8f80e07e12246e4c4b83b925249aa6f85446aed7b7d5391bd583acf7942157b7a359239aad1e1771997e0087370e1c14869a9289edfbd849d1e3a312659741326e0286421925589c946980fc63b1f820db7da2e13a1592fdd004050d1c67bf735edcdabd8b2add28300e41")]
        [TestCase(SignatureSchemes.Pss, SignatureModifications.Message, PssMaskTypes.MGF1, "56ad21", "8f9c6891089116fbbb655c062566836c505a3ec11d0f82eb3f18df85db0b37183e37183ac479fa99d937ccbf20cbf2f09d60807bed82bedec797cbafcd3cf908e58745a03726beef48aca0312e161b9087d9b99a314f82c78dfa73f569e2a3a1168a4a5560f420ed993b8bf051b93f50554a3c7b52ccff460b4b3ff75de598b038ed870f6a79c3c74fcde791b196bfb754bf3dd2fde8968b777a784d3e283067c1dc21875e63d2d6c16fa851f6c3306e362d0ec72190cd97a8950b6a09b2952038abc5f07ba33b7854efc528744f6044de8d3f3368d5103bdd538d159b0e924d2d879190ba75da6bc2924f6fc30e4436dcb96779d6261a7b9cc8a59add807851", "37987feecdba6cd23135e362920dcdded6a4ee7e875c347217f5a656588cd11779c1e4a4805807ca19da1526e3d3b5730d336a8d2f88d723487097199649a9e61f4c16e63843362257df5544e4cc2b1a20498df0d2febf5b61d00946d43f3a8a2b64e62870be8c735c14509169f60357eb6274e01bf4dcc968de0aa1047c59eb55ad8c11930bfd5d6343efb3df813c47d56a3fe9977846d80b87ac029327cf298ed458591411261609afad86067aeb2527759fa5c3ea101baa5c6ba0baf6de95c3de8b0025e628af2cc9b7e88f6bf2641ab131ecbcd7b980d5408f899d47d032f2f6cafc06429d23e6e8fdf47b4cc3b379dd8f2069d7e4ed96efecb0c451945f")]
        [TestCase(SignatureSchemes.Pss, SignatureModifications.ModifyTrailer, PssMaskTypes.MGF1, "a05095", "bd25c7275a544e41e00a681663544f8d857f544c3ad959f0597c273064aea563e7fb72d7518809c7d2d43ecb7d6be37250cde972408b07a1a578baada93e988f6b9f956901004ad02be1692df156406deade97a57d81d5d131383e420a0a9e69c09c38ecc1021cd36c1303b4bb0656d933685e6327dcbb9cf0dfdc3af00c4650798242a22bbe83008dc3c937cd98d9bb32bc626d7095f3e120b99d9fa5d93967202e4c62a94a58384ddaf660234032e80df0e983e6de94f6751844b34e8fe0e2a54f7190018dbff4cfc6f4bbc5a0b45dc561f154921f5ebeeb5bb12004e55dcbe56ecf1d294b78b50ed478248e37d4a37960c34db95ce3128d9fe009ae2bf51d", "40876a9103fad6962393f73e9b8d9536bc3f0879c0d6917f426e5ecaa2f1f73acc399f7eba1a91d0806ae289889661021d2267be576565ed32dd4a6cad133ea71dbf1da7a3901f3a84daeb89bc26811fd17eb764ff9e48532bf80fa25e2519c01f903d628559d865e4d083e3a1a05879c6790050c6a9830f68408475a619e2c101aa34b791e12c0617058b264100fcf08d16c1b4988a0dd910b66fd3657c5e7cec71a31d2d4484655b997a213d17ad70e33fc874403fd2973f44fca77b83ad9cd1a6791299b5599a4f9eab8ef1c6694386466663608633d4ed1b5dbffaa63a97058ee951d155044e3ce360b7986b1a838a69e025242fd694172d02cec1b9570f")]
        [TestCase(SignatureSchemes.Pss, SignatureModifications.MoveIr, PssMaskTypes.MGF1, "0100000001", "a87a6a10bc8f7fc893fa70a858a0ef4d4006d92ce94a6f89f0e34a41c5dcbbff2298343a054ce67b89d5872f5f17fac48b40196c4a8b0592b1601acd42904aa8c259fbc24e9adacfb78f24d838324c9861250f3e2122711214f9721be48db93a0ac9586a0d6c57024bd3abf4261d0d840852306998752bf616641ba652e063363e70af57702bc557a39a99571992aa5708c23e033d433953172bfadb81cd58ee6cc8b8bdda20cef791bc647e1d5d215f6926386050f5b45ba9e3b6cbe4b94ac92139ced91fc1471a9c76ea8f631970cba8516118f474c6ca669ef3f3b907725c311063732e3b11b7e353a66ff51dd3ad8aacd2048cb9e56132298bea05338005", "1b772a912fb0341b3033ff876320e430d49fef60e15dc251583d82c771d25e2ec7b1159168ede90ad11e4ccd34e0def2d4db184445f9318322f41be40ad681a6b827fc67551f3d444489be636bfb71bbc133b2cde7c608a0733cca27254b7510001f7cce553e9d6fcf3f39e6d87fa8a27fad8aaeea763987a19c81cc8cc86520b50a8d854c490405802cdfdeb10b5a3be558a601c93a268a6fe4051a9b93d42d0164fb24f5c2e001cfa19014826248a64fabfe26d3dbdfad586332bdef311e861eb8abfe00dd2d5f7f74af7301d939fb18b2475f34918d6e2fcef67d90ab9db970dbbd3f3095a322b91ed96f365ad27662bd4958e222767c53e0424457a945a5")]
        [TestCase(SignatureSchemes.Pss, SignatureModifications.Signature, PssMaskTypes.MGF1, "56ad21", "8f9c6891089116fbbb655c062566836c505a3ec11d0f82eb3f18df85db0b37183e37183ac479fa99d937ccbf20cbf2f09d60807bed82bedec797cbafcd3cf908e58745a03726beef48aca0312e161b9087d9b99a314f82c78dfa73f569e2a3a1168a4a5560f420ed993b8bf051b93f50554a3c7b52ccff460b4b3ff75de598b038ed870f6a79c3c74fcde791b196bfb754bf3dd2fde8968b777a784d3e283067c1dc21875e63d2d6c16fa851f6c3306e362d0ec72190cd97a8950b6a09b2952038abc5f07ba33b7854efc528744f6044de8d3f3368d5103bdd538d159b0e924d2d879190ba75da6bc2924f6fc30e4436dcb96779d6261a7b9cc8a59add807851", "37987feecdba6cd23135e362920dcdded6a4ee7e875c347217f5a656588cd11779c1e4a4805807ca19da1526e3d3b5730d336a8d2f88d723487097199649a9e61f4c16e63843362257df5544e4cc2b1a20498df0d2febf5b61d00946d43f3a8a2b64e62870be8c735c14509169f60357eb6274e01bf4dcc968de0aa1047c59eb55ad8c11930bfd5d6343efb3df813c47d56a3fe9977846d80b87ac029327cf298ed458591411261609afad86067aeb2527759fa5c3ea101baa5c6ba0baf6de95c3de8b0025e628af2cc9b7e88f6bf2641ab131ecbcd7b980d5408f899d47d032f2f6cafc06429d23e6e8fdf47b4cc3b379dd8f2069d7e4ed96efecb0c451945f")]
        
        [TestCase(SignatureSchemes.Pss, SignatureModifications.E, PssMaskTypes.SHAKE128, "0100000001", "a628a77126aa97bab3f1dc05e07b11a2fa8d7d14ad0f9b89b23b95772468bc16fa394dd8bff635cf9aecb375eb84bdfeafb2c4e30a76ef49977f5f80ca386463690c73a52f1822f185f03b09681392690c9027705f028a60ee2413c3d389f4943796fa637fd77ff7efa401fbb859e856ffbfd679f21d81c62a924ab3c019968796765a335bdd7ec3826464d6df9c39975a104e7df1356bc3185109982d3751678122038747c8c38baa681296083bea454b896ab95a09abc296d6cdd5253d87a88fee36d1efbbe4966222edf2a0ae99979cba3c91f9a441061b3a70df6529b5bc8120c3f41a780aa695a38d3e413d367966aa82f4139e9765793938c76f327497", "13bf0a8133f70e4094ace4f6a7c8531a11a51e0fcf80490fa4ab6e06f913e256eb7101ecd71553b99ee03c503d2f4af9a83a9ff4892b2d54dca69d4c6274db29da202dfe69eac37a58cdf9445343e56c5f8ab8aa1df27cadec5b39768802cd9f55baa0a4c17db0e46397a85b4886643ea0f1e9aefc11f925ae8c616b705e190b6e5770e331374de1db7df3a8f84ce1689a5d47018b8f80e07e12246e4c4b83b925249aa6f85446aed7b7d5391bd583acf7942157b7a359239aad1e1771997e0087370e1c14869a9289edfbd849d1e3a312659741326e0286421925589c946980fc63b1f820db7da2e13a1592fdd004050d1c67bf735edcdabd8b2add28300e41")]
        [TestCase(SignatureSchemes.Pss, SignatureModifications.Message, PssMaskTypes.SHAKE128, "56ad21", "8f9c6891089116fbbb655c062566836c505a3ec11d0f82eb3f18df85db0b37183e37183ac479fa99d937ccbf20cbf2f09d60807bed82bedec797cbafcd3cf908e58745a03726beef48aca0312e161b9087d9b99a314f82c78dfa73f569e2a3a1168a4a5560f420ed993b8bf051b93f50554a3c7b52ccff460b4b3ff75de598b038ed870f6a79c3c74fcde791b196bfb754bf3dd2fde8968b777a784d3e283067c1dc21875e63d2d6c16fa851f6c3306e362d0ec72190cd97a8950b6a09b2952038abc5f07ba33b7854efc528744f6044de8d3f3368d5103bdd538d159b0e924d2d879190ba75da6bc2924f6fc30e4436dcb96779d6261a7b9cc8a59add807851", "37987feecdba6cd23135e362920dcdded6a4ee7e875c347217f5a656588cd11779c1e4a4805807ca19da1526e3d3b5730d336a8d2f88d723487097199649a9e61f4c16e63843362257df5544e4cc2b1a20498df0d2febf5b61d00946d43f3a8a2b64e62870be8c735c14509169f60357eb6274e01bf4dcc968de0aa1047c59eb55ad8c11930bfd5d6343efb3df813c47d56a3fe9977846d80b87ac029327cf298ed458591411261609afad86067aeb2527759fa5c3ea101baa5c6ba0baf6de95c3de8b0025e628af2cc9b7e88f6bf2641ab131ecbcd7b980d5408f899d47d032f2f6cafc06429d23e6e8fdf47b4cc3b379dd8f2069d7e4ed96efecb0c451945f")]
        [TestCase(SignatureSchemes.Pss, SignatureModifications.ModifyTrailer, PssMaskTypes.SHAKE128, "a05095", "bd25c7275a544e41e00a681663544f8d857f544c3ad959f0597c273064aea563e7fb72d7518809c7d2d43ecb7d6be37250cde972408b07a1a578baada93e988f6b9f956901004ad02be1692df156406deade97a57d81d5d131383e420a0a9e69c09c38ecc1021cd36c1303b4bb0656d933685e6327dcbb9cf0dfdc3af00c4650798242a22bbe83008dc3c937cd98d9bb32bc626d7095f3e120b99d9fa5d93967202e4c62a94a58384ddaf660234032e80df0e983e6de94f6751844b34e8fe0e2a54f7190018dbff4cfc6f4bbc5a0b45dc561f154921f5ebeeb5bb12004e55dcbe56ecf1d294b78b50ed478248e37d4a37960c34db95ce3128d9fe009ae2bf51d", "40876a9103fad6962393f73e9b8d9536bc3f0879c0d6917f426e5ecaa2f1f73acc399f7eba1a91d0806ae289889661021d2267be576565ed32dd4a6cad133ea71dbf1da7a3901f3a84daeb89bc26811fd17eb764ff9e48532bf80fa25e2519c01f903d628559d865e4d083e3a1a05879c6790050c6a9830f68408475a619e2c101aa34b791e12c0617058b264100fcf08d16c1b4988a0dd910b66fd3657c5e7cec71a31d2d4484655b997a213d17ad70e33fc874403fd2973f44fca77b83ad9cd1a6791299b5599a4f9eab8ef1c6694386466663608633d4ed1b5dbffaa63a97058ee951d155044e3ce360b7986b1a838a69e025242fd694172d02cec1b9570f")]
        [TestCase(SignatureSchemes.Pss, SignatureModifications.MoveIr, PssMaskTypes.SHAKE128, "0100000001", "a87a6a10bc8f7fc893fa70a858a0ef4d4006d92ce94a6f89f0e34a41c5dcbbff2298343a054ce67b89d5872f5f17fac48b40196c4a8b0592b1601acd42904aa8c259fbc24e9adacfb78f24d838324c9861250f3e2122711214f9721be48db93a0ac9586a0d6c57024bd3abf4261d0d840852306998752bf616641ba652e063363e70af57702bc557a39a99571992aa5708c23e033d433953172bfadb81cd58ee6cc8b8bdda20cef791bc647e1d5d215f6926386050f5b45ba9e3b6cbe4b94ac92139ced91fc1471a9c76ea8f631970cba8516118f474c6ca669ef3f3b907725c311063732e3b11b7e353a66ff51dd3ad8aacd2048cb9e56132298bea05338005", "1b772a912fb0341b3033ff876320e430d49fef60e15dc251583d82c771d25e2ec7b1159168ede90ad11e4ccd34e0def2d4db184445f9318322f41be40ad681a6b827fc67551f3d444489be636bfb71bbc133b2cde7c608a0733cca27254b7510001f7cce553e9d6fcf3f39e6d87fa8a27fad8aaeea763987a19c81cc8cc86520b50a8d854c490405802cdfdeb10b5a3be558a601c93a268a6fe4051a9b93d42d0164fb24f5c2e001cfa19014826248a64fabfe26d3dbdfad586332bdef311e861eb8abfe00dd2d5f7f74af7301d939fb18b2475f34918d6e2fcef67d90ab9db970dbbd3f3095a322b91ed96f365ad27662bd4958e222767c53e0424457a945a5")]
        [TestCase(SignatureSchemes.Pss, SignatureModifications.Signature, PssMaskTypes.SHAKE128, "56ad21", "8f9c6891089116fbbb655c062566836c505a3ec11d0f82eb3f18df85db0b37183e37183ac479fa99d937ccbf20cbf2f09d60807bed82bedec797cbafcd3cf908e58745a03726beef48aca0312e161b9087d9b99a314f82c78dfa73f569e2a3a1168a4a5560f420ed993b8bf051b93f50554a3c7b52ccff460b4b3ff75de598b038ed870f6a79c3c74fcde791b196bfb754bf3dd2fde8968b777a784d3e283067c1dc21875e63d2d6c16fa851f6c3306e362d0ec72190cd97a8950b6a09b2952038abc5f07ba33b7854efc528744f6044de8d3f3368d5103bdd538d159b0e924d2d879190ba75da6bc2924f6fc30e4436dcb96779d6261a7b9cc8a59add807851", "37987feecdba6cd23135e362920dcdded6a4ee7e875c347217f5a656588cd11779c1e4a4805807ca19da1526e3d3b5730d336a8d2f88d723487097199649a9e61f4c16e63843362257df5544e4cc2b1a20498df0d2febf5b61d00946d43f3a8a2b64e62870be8c735c14509169f60357eb6274e01bf4dcc968de0aa1047c59eb55ad8c11930bfd5d6343efb3df813c47d56a3fe9977846d80b87ac029327cf298ed458591411261609afad86067aeb2527759fa5c3ea101baa5c6ba0baf6de95c3de8b0025e628af2cc9b7e88f6bf2641ab131ecbcd7b980d5408f899d47d032f2f6cafc06429d23e6e8fdf47b4cc3b379dd8f2069d7e4ed96efecb0c451945f")]
        
        [TestCase(SignatureSchemes.Pss, SignatureModifications.E, PssMaskTypes.SHAKE256, "0100000001", "a628a77126aa97bab3f1dc05e07b11a2fa8d7d14ad0f9b89b23b95772468bc16fa394dd8bff635cf9aecb375eb84bdfeafb2c4e30a76ef49977f5f80ca386463690c73a52f1822f185f03b09681392690c9027705f028a60ee2413c3d389f4943796fa637fd77ff7efa401fbb859e856ffbfd679f21d81c62a924ab3c019968796765a335bdd7ec3826464d6df9c39975a104e7df1356bc3185109982d3751678122038747c8c38baa681296083bea454b896ab95a09abc296d6cdd5253d87a88fee36d1efbbe4966222edf2a0ae99979cba3c91f9a441061b3a70df6529b5bc8120c3f41a780aa695a38d3e413d367966aa82f4139e9765793938c76f327497", "13bf0a8133f70e4094ace4f6a7c8531a11a51e0fcf80490fa4ab6e06f913e256eb7101ecd71553b99ee03c503d2f4af9a83a9ff4892b2d54dca69d4c6274db29da202dfe69eac37a58cdf9445343e56c5f8ab8aa1df27cadec5b39768802cd9f55baa0a4c17db0e46397a85b4886643ea0f1e9aefc11f925ae8c616b705e190b6e5770e331374de1db7df3a8f84ce1689a5d47018b8f80e07e12246e4c4b83b925249aa6f85446aed7b7d5391bd583acf7942157b7a359239aad1e1771997e0087370e1c14869a9289edfbd849d1e3a312659741326e0286421925589c946980fc63b1f820db7da2e13a1592fdd004050d1c67bf735edcdabd8b2add28300e41")]
        [TestCase(SignatureSchemes.Pss, SignatureModifications.Message, PssMaskTypes.SHAKE256, "56ad21", "8f9c6891089116fbbb655c062566836c505a3ec11d0f82eb3f18df85db0b37183e37183ac479fa99d937ccbf20cbf2f09d60807bed82bedec797cbafcd3cf908e58745a03726beef48aca0312e161b9087d9b99a314f82c78dfa73f569e2a3a1168a4a5560f420ed993b8bf051b93f50554a3c7b52ccff460b4b3ff75de598b038ed870f6a79c3c74fcde791b196bfb754bf3dd2fde8968b777a784d3e283067c1dc21875e63d2d6c16fa851f6c3306e362d0ec72190cd97a8950b6a09b2952038abc5f07ba33b7854efc528744f6044de8d3f3368d5103bdd538d159b0e924d2d879190ba75da6bc2924f6fc30e4436dcb96779d6261a7b9cc8a59add807851", "37987feecdba6cd23135e362920dcdded6a4ee7e875c347217f5a656588cd11779c1e4a4805807ca19da1526e3d3b5730d336a8d2f88d723487097199649a9e61f4c16e63843362257df5544e4cc2b1a20498df0d2febf5b61d00946d43f3a8a2b64e62870be8c735c14509169f60357eb6274e01bf4dcc968de0aa1047c59eb55ad8c11930bfd5d6343efb3df813c47d56a3fe9977846d80b87ac029327cf298ed458591411261609afad86067aeb2527759fa5c3ea101baa5c6ba0baf6de95c3de8b0025e628af2cc9b7e88f6bf2641ab131ecbcd7b980d5408f899d47d032f2f6cafc06429d23e6e8fdf47b4cc3b379dd8f2069d7e4ed96efecb0c451945f")]
        [TestCase(SignatureSchemes.Pss, SignatureModifications.ModifyTrailer, PssMaskTypes.SHAKE256, "a05095", "bd25c7275a544e41e00a681663544f8d857f544c3ad959f0597c273064aea563e7fb72d7518809c7d2d43ecb7d6be37250cde972408b07a1a578baada93e988f6b9f956901004ad02be1692df156406deade97a57d81d5d131383e420a0a9e69c09c38ecc1021cd36c1303b4bb0656d933685e6327dcbb9cf0dfdc3af00c4650798242a22bbe83008dc3c937cd98d9bb32bc626d7095f3e120b99d9fa5d93967202e4c62a94a58384ddaf660234032e80df0e983e6de94f6751844b34e8fe0e2a54f7190018dbff4cfc6f4bbc5a0b45dc561f154921f5ebeeb5bb12004e55dcbe56ecf1d294b78b50ed478248e37d4a37960c34db95ce3128d9fe009ae2bf51d", "40876a9103fad6962393f73e9b8d9536bc3f0879c0d6917f426e5ecaa2f1f73acc399f7eba1a91d0806ae289889661021d2267be576565ed32dd4a6cad133ea71dbf1da7a3901f3a84daeb89bc26811fd17eb764ff9e48532bf80fa25e2519c01f903d628559d865e4d083e3a1a05879c6790050c6a9830f68408475a619e2c101aa34b791e12c0617058b264100fcf08d16c1b4988a0dd910b66fd3657c5e7cec71a31d2d4484655b997a213d17ad70e33fc874403fd2973f44fca77b83ad9cd1a6791299b5599a4f9eab8ef1c6694386466663608633d4ed1b5dbffaa63a97058ee951d155044e3ce360b7986b1a838a69e025242fd694172d02cec1b9570f")]
        [TestCase(SignatureSchemes.Pss, SignatureModifications.MoveIr, PssMaskTypes.SHAKE256, "0100000001", "a87a6a10bc8f7fc893fa70a858a0ef4d4006d92ce94a6f89f0e34a41c5dcbbff2298343a054ce67b89d5872f5f17fac48b40196c4a8b0592b1601acd42904aa8c259fbc24e9adacfb78f24d838324c9861250f3e2122711214f9721be48db93a0ac9586a0d6c57024bd3abf4261d0d840852306998752bf616641ba652e063363e70af57702bc557a39a99571992aa5708c23e033d433953172bfadb81cd58ee6cc8b8bdda20cef791bc647e1d5d215f6926386050f5b45ba9e3b6cbe4b94ac92139ced91fc1471a9c76ea8f631970cba8516118f474c6ca669ef3f3b907725c311063732e3b11b7e353a66ff51dd3ad8aacd2048cb9e56132298bea05338005", "1b772a912fb0341b3033ff876320e430d49fef60e15dc251583d82c771d25e2ec7b1159168ede90ad11e4ccd34e0def2d4db184445f9318322f41be40ad681a6b827fc67551f3d444489be636bfb71bbc133b2cde7c608a0733cca27254b7510001f7cce553e9d6fcf3f39e6d87fa8a27fad8aaeea763987a19c81cc8cc86520b50a8d854c490405802cdfdeb10b5a3be558a601c93a268a6fe4051a9b93d42d0164fb24f5c2e001cfa19014826248a64fabfe26d3dbdfad586332bdef311e861eb8abfe00dd2d5f7f74af7301d939fb18b2475f34918d6e2fcef67d90ab9db970dbbd3f3095a322b91ed96f365ad27662bd4958e222767c53e0424457a945a5")]
        [TestCase(SignatureSchemes.Pss, SignatureModifications.Signature, PssMaskTypes.SHAKE256, "56ad21", "8f9c6891089116fbbb655c062566836c505a3ec11d0f82eb3f18df85db0b37183e37183ac479fa99d937ccbf20cbf2f09d60807bed82bedec797cbafcd3cf908e58745a03726beef48aca0312e161b9087d9b99a314f82c78dfa73f569e2a3a1168a4a5560f420ed993b8bf051b93f50554a3c7b52ccff460b4b3ff75de598b038ed870f6a79c3c74fcde791b196bfb754bf3dd2fde8968b777a784d3e283067c1dc21875e63d2d6c16fa851f6c3306e362d0ec72190cd97a8950b6a09b2952038abc5f07ba33b7854efc528744f6044de8d3f3368d5103bdd538d159b0e924d2d879190ba75da6bc2924f6fc30e4436dcb96779d6261a7b9cc8a59add807851", "37987feecdba6cd23135e362920dcdded6a4ee7e875c347217f5a656588cd11779c1e4a4805807ca19da1526e3d3b5730d336a8d2f88d723487097199649a9e61f4c16e63843362257df5544e4cc2b1a20498df0d2febf5b61d00946d43f3a8a2b64e62870be8c735c14509169f60357eb6274e01bf4dcc968de0aa1047c59eb55ad8c11930bfd5d6343efb3df813c47d56a3fe9977846d80b87ac029327cf298ed458591411261609afad86067aeb2527759fa5c3ea101baa5c6ba0baf6de95c3de8b0025e628af2cc9b7e88f6bf2641ab131ecbcd7b980d5408f899d47d032f2f6cafc06429d23e6e8fdf47b4cc3b379dd8f2069d7e4ed96efecb0c451945f")]
        public void ShouldFailToVerify(SignatureSchemes sigScheme, SignatureModifications mods, PssMaskTypes pssMask, string eHex, string nHex, string dHex)
        {
            var rand = new Random800_90();

            var sigBuilder = new SignatureBuilder();
            var paddingFactory = new PaddingFactory(new MaskFactory(new ShaFactory()));
            var entropyProvider = new EntropyProvider(rand);
            var sha = new ShaFactory().GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256));
            var paddingScheme = paddingFactory.GetSigningPaddingScheme(sigScheme, sha, mods, pssMask, entropyProvider, 8);
            var rsa = new Rsa(new RsaVisitor());

            var message = entropyProvider.GetEntropy(1024);
            var e = new BitString(eHex).ToPositiveBigInteger();
            var n = new BitString(nHex).ToPositiveBigInteger();
            var d = new BitString(dHex).ToPositiveBigInteger();

            var publicKey = new PublicKey {E = e, N = n};
            var privateKey = new PrivateKey {D = d};

            var signature = sigBuilder
                .WithMessage(message)
                .WithPublicKey(publicKey)
                .WithPrivateKey(privateKey)
                .WithDecryptionScheme(rsa)
                .WithPaddingScheme(paddingScheme)
                .BuildSign();

            Assume.That(signature.Success, signature.ErrorMessage);

            var verify = sigBuilder
                .WithMessage(message)
                .WithPublicKey(publicKey)
                .WithDecryptionScheme(rsa)
                .WithPaddingScheme(paddingScheme)
                .BuildVerify();

            Assert.IsFalse(verify.Success);
        }
    }
}