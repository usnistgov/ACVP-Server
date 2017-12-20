using System;
using System.Collections;
using System.Linq;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;
using NUnit.Framework;
using NIST.CVP.Crypto.TDES_CFB;

namespace NIST.CVP.Crypto.TDES_CFBP.Tests
{
    public class HealthTests
    {

        [Test]
        [TestCase(Algo.TDES_CFBP1, "4cda7cab1c3d2f51", "19d346f208ea83f7", "b9eab3ef61c7d0b5", "b78e05dc12ecdfb5", "0ce35b316842350a", "6238b086bd978a5f", "0", "0", false, TestName = "Encrypt TDES-CFB1_0")]
        [TestCase(Algo.TDES_CFBP1, "d37ad5c13d7a5dce", "c1192fba207cb562", "5b58a792374698ce", "c42372f21ad771de", "1978c847702cc733", "6ece1d9cc5821c88", "01", "10", false, TestName = "Encrypt TDES-CFB1_1")]
        [TestCase(Algo.TDES_CFBP1, "3e465b46f1ce1940", "a14ad9f40b707346", "c45862a7a77ae5a4", "b0c7b835fe57e269", "061d0d8b53ad37be", "5b7262e0a9028d13", "011", "001", false, TestName = "Encrypt TDES-CFB1_2")]
        [TestCase(Algo.TDES_CFBP1, "4ae55440f4e5a49e", "544329165b8980ce", "264a80b39d8931a2", "a6c6de1a0db3f294", "fc1c336f630947e9", "517188c4b85e9d3e", "1011", "0010", false, TestName = "Encrypt TDES-CFB1_3")]
        [TestCase(Algo.TDES_CFBP1, "c804f8d5075434e0", "759891b59d8fabba", "858c64feea6bd0e5", "9450e440915afd90", "e9a63995e6b052e5", "3efb8eeb3c05a83a", "01101", "00000", false, TestName = "Encrypt TDES-CFB1_4")]
        [TestCase(Algo.TDES_CFBP1, "fbfd10b594462cb3", "7abad620df7c151f", "7f8fe9b37c92256d", "6829e6e4ba767702", "bd7f3c3a0fcbcc57", "12d4918f652121ac", "100111", "010001", false, TestName = "Encrypt TDES-CFB1_5")]
        [TestCase(Algo.TDES_CFBP1, "b54f85458a1c73ba", "2a8cf289157a5dea", "fbc7da29d9679b4f", "6e493817e4ef6477", "c39e8d6d3a44b9cc", "18f3e2c28f9a0f21", "0111100", "1111101", false, TestName = "Encrypt TDES-CFB1_6")]
        [TestCase(Algo.TDES_CFBP1, "7a3e4662e9756110", "6d2a5458aebcdf4a", "7cae103752fb021a", "bf62c7e128431271", "14b81d367d9867c6", "6a0d728bd2edbd1b", "11011110", "11111110", false, TestName = "Encrypt TDES-CFB1_7")]
        [TestCase(Algo.TDES_CFBP1, "52cd7fba0262047f", "61da8f3b4c7cae01", "d6208a6e86611525", "a8b81f15e4cdc2c9", "fe0d746b3a23181e", "5362c9c08f786d73", "011111100", "111100001", false, TestName = "Encrypt TDES-CFB1_8")]
        [TestCase(Algo.TDES_CFBP1, "1964326bc2e962d5", "f29e3edf2ca45185", "a7b37f859d7a6449", "01148a4b0c69627b", "5669dfa061beb7d0", "abbf34f5b7140d25", "0100000001", "0100101100", false, TestName = "Encrypt TDES-CFB1_9")]

        [TestCase(Algo.TDES_CFBP8, "15940d29fbfeb0da", "f4bae991f8d37c83", "97d0169b850e8fda", "fce1d679bd5d66fd", "52372bcf12b2bc52", "a78c8124680811a7", "75", "85", TestName = "Encrypt TDES-CFB8_0")]
        [TestCase(Algo.TDES_CFBP8, "cbcec83486bfc713", "0beaf8ab2f4ab529", "02c704dc0725e975", "20979b8b3577e339", "75ecf0e08acd388e", "cb424635e0228de3", "1c22", "af4d", TestName = "Encrypt TDES-CFB8_1")]
        [TestCase(Algo.TDES_CFBP8, "89a41c7c7cd3bf54", "91f280ecd5580b68", "c16768764a43b051", "d7802ba95caac0f4", "2cd580feb2001649", "822ad65407556b9e", "c2adc2", "aa35a7", TestName = "Encrypt TDES-CFB8_2")]
        [TestCase(Algo.TDES_CFBP8, "02893b8fc82abf02", "e00475017c9bba5d", "4ac4796efbfb400b", "7319587a23eb7796", "c86eadcf7940cceb", "1dc40324ce962240", "92bd4b66", "f7a1e359", TestName = "Encrypt TDES-CFB8_3")]
        [TestCase(Algo.TDES_CFBP8, "58d57c2a5b8a687a", "76dcf7ba769bd332", "3402c2ae611502c8", "d4c32714eeb1e09e", "2a187c6a440735f3", "7f6dd1bf995c8b48", "5b48334d9f", "2bf22a439f", TestName = "Encrypt TDES-CFB8_4")]
        [TestCase(Algo.TDES_CFBP8, "6d0b6dc7a7f43b04", "c107ce20d697e6b9", "c24a2c404c072cf8", "6dfef290f7f0a419", "c35447e64d45f96e", "18a99d3ba29b4ec3", "75edafd83de3", "2ab83cedd6b7", TestName = "Encrypt TDES-CFB8_5")]
        [TestCase(Algo.TDES_CFBP8, "f8d0b3c48abc52fb", "46b689dcf716649e", "37a2dca1373e7ab9", "b2793aa780020a3d", "07ce8ffcd5575f92", "5d23e5522aacb4e7", "ac22fca72b818b", "6657de3e4ab079", TestName = "Encrypt TDES-CFB8_6")]
        [TestCase(Algo.TDES_CFBP8, "238cc2f45dce6dea", "dfb686f831523b89", "40ae3ebac170838c", "a790a89a062de3ed", "fce5fdef5b833942", "523b5344b0d88e97", "24b0520ea08c1307", "58917055ad2327df", TestName = "Encrypt TDES-CFB8_7")]
        [TestCase(Algo.TDES_CFBP8, "8cc1c73223ae2cb3", "1c0ece7aa8b352e9", "2531ce1a6ebf236b", "ade9d9ce96759f7f", "033f2f23ebcaf4d4", "5894847941204a29", "e70f6d859977fcc069", "10dc53548c045917e1", TestName = "Encrypt TDES-CFB8_8")]
        [TestCase(Algo.TDES_CFBP8, "a2343e580be0ce2f", "ce83f80ecb4f9194", "0b9eba0ba2e9d602", "80d92f40453e242a", "d62e84959a93797f", "2b83d9eaefe8ced4", "8ddbc38306a6f8613cd7", "11495d5c1f0803dc6eaa", TestName = "Encrypt TDES-CFB8_9")]

        [TestCase(Algo.TDES_CFBP64, "0bc72aabd5465762", "c826d5a125925715", "bcc1b9ae62f8da32", "f7a29baeaf6f94e2", "4cf7f10404c4ea37", "a24d46595a1a3f8c", "af0c63075145a4c2", "43ff1e72104d5fd6", TestName = "Encrypt TDES-CFB64_0")]
        [TestCase(Algo.TDES_CFBP64, "4f5ea1384ab6fe2a", "925bb0107f79f1cd", "3dbf3da4019d6dfe", "c686bbaf4f69de24", "1bdc1104a4bf3379", "71316659fa1488ce", "dd0aa61bd0a7b194da6b0112545959dc", "eada898555ade98663771a62549fec4f", TestName = "Encrypt TDES-CFB64_1")]
        [TestCase(Algo.TDES_CFBP64, "a40897b625d9765e", "256e9b6badb94cdf", "ec7afe584f52c75b", "b86d23306bc11da6", "0dc27885c11672fb", "6317cddb166bc850", "a6257f5bbda8dfc2337ff10c19c76ae7ed55e1651155dfb7", "d05d52a3d96c2132fd98455811dc989f240041056cf8bb7e", TestName = "Encrypt TDES-CFB64_2")]
        [TestCase(Algo.TDES_CFBP64, "627aa2527564c876", "e3198ae9ea020def", "6dc1263bdf493846", "9b59e2ab10d3202b", "f0af380066287580", "46048d55bb7dcad5", "f4d336791ce4a584323d0b455bbed44392c5f86c9d5287593f6986d4b0b8f997", "8c318b7cac27ee101cb3609dddb281c922482f63b13637580ea0dca69346917c", TestName = "Encrypt TDES-CFB64_3")]
        [TestCase(Algo.TDES_CFBP64, "b661a8f2e30e465d", "bc0ef270646b197f", "0ecd808f1ac87629", "2fbdb3224e12da7a", "85130877a3682fcf", "da685dccf8bd8524", "0cdf2de11bda034b5dd70a8213dfb18a47a6724460c905d9f354d45bdc87b0aa8edac295a73ec442", "84da4489e240031f2308c9e006255317c5e22adbb8e5462fbcbc79c2d1f48357f191ff5f97c2dcd1", TestName = "Encrypt TDES-CFB64_4")]
        [TestCase(Algo.TDES_CFBP64, "da6b8c29fb7f38a2", "c716b3e9164501b5", "f23ea485df4a458f", "fa7c94a2eb67dbf0", "4fd1e9f840bd3145", "a5273f4d9612869a", "e29ef443935f77a8d869024586161cd772b171a8cb7dc8b6adf83511f26090854c7725799184ad17559a7eadbe1cfe53", "849f8ae2fa5b8dd7f384115480b438c395ed034205098ce999d3dac4c698f1455c5b5e00dfd63acea7734f57a5f87fdd", TestName = "Encrypt TDES-CFB64_5")]
        [TestCase(Algo.TDES_CFBP64, "ecbcb6fd40268f83", "79c1c268dcfb2f15", "d043e9f21373cb40", "5dbbf05645864673", "b31145ab9adb9bc8", "08669b00f030f11d", "f588a53f5267690dd6518b4d67e43d33fe885a045852b11def29a1372827e6e9ce41b26ebf2bffa9e576246668fca7e3227d538f548a8a11", "6a3e9e7e4caa5bce256c57070fb403cacf0ca01f3d780cb498656e8cd015015bdcde48470d1730398698733a930ba387c7a0fc0c0dd28dc0", TestName = "Encrypt TDES-CFB64_6")]
        [TestCase(Algo.TDES_CFBP64, "efe334625d0e8ff7", "388a8fbfecda9219", "cbb589154c381f25", "fea5b9903f48de39", "53fb0ee5949e338e", "a950643ae9f388e3", "b9d45f1b016c851ce743a5104f7de0e485070185d1d7b3454c75f281f1bac080f7d7199f5f9a28abc9edbb8f8ba0f52f046842932e279048736dbd03c23f4b88", "9a348bcfc4e6768d7b2ae6290a8525f8841de499eec7bdd2bf81c12697a63a756324e2ac93995fe2dd4b684db886e1bd60771e62b0728eaae26fc87f1d460cef", TestName = "Encrypt TDES-CFB64_7")]
        [TestCase(Algo.TDES_CFBP64, "e0d91031ea2040cd", "ab01736e70b386f7", "75eab6bf9e8002fe", "ec6e8421a04e326f", "41c3d976f5a387c4", "97192ecc4af8dd19", "69583de887b72f22e3bf041fc0f53932ab02e9dfb0129e55cd7a6739a79d2aa57c103c7159fc2fb18dc4eb7e5de800faf92a3b8eafef78765fe7b6802ff071107f382c6820a24d26", "dc8dfc7ec82fbeeed6c101e8a86218b4b0a61d7ea4280961625a5a650f6a6735efb621808a2260f4cd05151beaebbec1795004cfd5653a9065f7bbacd55e8509f5d935a5ae56cf44", TestName = "Encrypt TDES-CFB64_8")]
        [TestCase(Algo.TDES_CFBP64, "4c40082ff102f419", "8580dffd792f7689", "38b3073bcd7a73f4", "e7bf40905b3c82e5", "3d1495e5b091d83a", "9269eb3b05e72d8f", "ba5d70f22d666eed97584a868890ba280ff6dc09da8b795c9e428ac9ba11fb1a1747b0375220c6541d971b69e27b10e2aac58d983e2946018beb3da34bf488cfe225558b86eb2bd0a31473f67ee1366c", "fa4d57f83cfa6c08f34d24c7da998972ac6825e72b9a67479a700e2aa298c955e2d790a0884a82b7bddc7542998a77e27de79f07f3f99499b5378d3c537282338b9dbe8ef4cbdb288b2c51ce1ea085dd", TestName = "Encrypt TDES-CFB64_9")]

        public void ShouldEncryptSuccessfully(Algo _algo, string _key1, string _key2, string _key3, string _iv1, string _iv2, string _iv3, string _pt, string _ct, bool isPtAndCtHex = true)
        {
            var mode = ModeFactory.GetMode(_algo);
            var key = new BitString(_key1 + _key2 + _key3);
            var iv1 = new BitString(_iv1);
            var iv2 = new BitString(_iv2);
            var iv3 = new BitString(_iv3);
            BitString plainText;
            BitString cipherText;
            if (isPtAndCtHex)
            {
                plainText = new BitString(_pt);
                cipherText = new BitString(_ct);
            }
            else
            {
                plainText = new BitString(new BitArray(_pt.Reverse().Select(x => x == '1' || x == '0' ? //make sure only 0 or 1 are in the string
                    x == '1' :
                    throw new InvalidCastException()).ToArray()));

                cipherText = new BitString(new BitArray(_ct.Reverse().Select(x => x == '1' || x == '0' ? //make sure only 0 or 1 are in the string
                    x == '1' :
                    throw new InvalidCastException()).ToArray()));
            }

            //todo figure out how to get rid of the cast
            var result = (EncryptionResultWithIv)mode.BlockEncrypt(key, iv1, plainText);
            Assert.AreEqual(cipherText.ToHex(), result.CipherText.ToHex());
            Assert.AreEqual(iv2, result.IVs[1]);
            Assert.AreEqual(iv3, result.IVs[2]);

        }

        [Test]
        [TestCase(Algo.TDES_CFBP1, "263d0480c832dc16", "f4ae6773e6320461", "43f797f2d545e692", "4885ffb3206d3489", "9ddb550875c289de", "f330aa5dcb17df33", "0", "0", false, TestName = "Decrypt TDES-CFB1_0")]
        [TestCase(Algo.TDES_CFBP1, "025402020d492f9d", "d592d6f7830d0bb6", "cdcd85680ddc2a32", "9e99568289c54c74", "f3eeabd7df1aa1c9", "4944012d346ff71e", "00", "01", false, TestName = "Decrypt TDES-CFB1_1")]
        [TestCase(Algo.TDES_CFBP1, "bf70400d20ec259e", "13ef0dcdae46bfcd", "320e8f797586b5a4", "03aa10795f78415e", "58ff65ceb4cd96b3", "ae54bb240a22ec08", "110", "101", false, TestName = "Decrypt TDES-CFB1_2")]
        [TestCase(Algo.TDES_CFBP1, "640b49f8d9d3cd16", "1976920be67c73dc", "20b097d53ec16d89", "861670f0b5c8d87e", "db6bc6460b1e2dd3", "30c11b9b60738328", "0110", "0110", false, TestName = "Decrypt TDES-CFB1_3")]
        [TestCase(Algo.TDES_CFBP1, "2ae9a7761098ad62", "2c029de66ea1d938", "c238cda129731cf1", "3cef8cc26b4042e6", "9244e217c095983b", "e79a376d15eaed90", "11111", "00011", false, TestName = "Decrypt TDES-CFB1_4")]
        [TestCase(Algo.TDES_CFBP1, "5b2c3de31cdf7c64", "b99d7a343801d5c7", "3867c79e1ac4d52c", "fed410940c889659", "542965e961ddebae", "a97ebb3eb7334103", "010001", "000011", false, TestName = "Decrypt TDES-CFB1_5")]
        [TestCase(Algo.TDES_CFBP1, "76a21f9e7367e54f", "e9eaa464c7623ed5", "f71f2a6da47649f1", "c270f6601f1beae4", "17c64bb574714039", "6d1ba10ac9c6958e", "1111011", "0100010", false, TestName = "Decrypt TDES-CFB1_6")]
        [TestCase(Algo.TDES_CFBP1, "254531b5b9a4f115", "43f498f8aef229f2", "e086ea2cc2dc2ae9", "dd1ffaef3002fd2c", "3275504485585281", "87caa599daada7d6", "01111111", "10010011", false, TestName = "Decrypt TDES-CFB1_7")]
        [TestCase(Algo.TDES_CFBP1, "a8548aa7d3bcd96b", "3745347ffe58efb3", "8fc17c804531b098", "a8fccaf3ec1606e0", "fe522049416b5c35", "53a7759e96c0b18a", "011010011", "110000011", false, TestName = "Decrypt TDES-CFB1_8")]
        [TestCase(Algo.TDES_CFBP1, "9bf843703b8a755d", "d6dcd9b0f72a2f34", "ea2a163d3e546b1f", "501def7cba801a3b", "a57344d20fd56f90", "fac89a27652ac4e5", "1101110101", "1110000110", false, TestName = "Decrypt TDES-CFB1_9")]


        [TestCase(Algo.TDES_CFBP8, "e561df4076e6d01c", "a1f1d36ed038c47c", "1a4504018ae5a2ef", "94a4fd84a9c1d2c2", "e9fa52d9ff172817", "3f4fa82f546c7d6c", "5d", "6a", TestName = "Decrypt TDES-CFB8_0")]
        [TestCase(Algo.TDES_CFBP8, "70b50ebcef5b0b51", "fb58bfbc2fc79bf4", "a2382aa7e50d2985", "bb39a1f2d3620f29", "108ef74828b7647e", "65e44c9d7e0cb9d3", "b35a", "23a4", TestName = "Decrypt TDES-CFB8_1")]
        [TestCase(Algo.TDES_CFBP8, "7fbf32f16149587a", "89e5da0dabf1a815", "f70434fe496e1c1a", "2a3ebaceecb560fe", "7f941024420ab653", "d4e9657997600ba8", "5554ee", "809c63", TestName = "Decrypt TDES-CFB8_2")]
        [TestCase(Algo.TDES_CFBP8, "16a72fda2f9ec4dc", "233e8f1abcf710d6", "d04ac87526646226", "3aeed87b226c005e", "90442dd077c155b3", "e5998325cd16ab08", "b6539e25", "64132195", TestName = "Decrypt TDES-CFB8_3")]
        [TestCase(Algo.TDES_CFBP8, "d35bec2958047929", "6b6e460231e6bc43", "3bcb0e5475a80115", "fb35f69c5cb2dc09", "508b4bf1b208315e", "a5e0a147075d86b3", "1209f20b77", "d658b428a3", TestName = "Decrypt TDES-CFB8_4")]
        [TestCase(Algo.TDES_CFBP8, "e67007da0d23c757", "d637d957ab733d25", "267fbc25ba07a23e", "b7fe1483d2ff5dfe", "0d5369d92854b353", "62a8bf2e7daa08a8", "4ec40a64b749", "b3981d65b635", TestName = "Decrypt TDES-CFB8_5")]
        [TestCase(Algo.TDES_CFBP8, "1c6e6ebc58a74016", "d62c0b86bc5449e9", "6191f71a20649151", "d39baab2ad75208a", "28f1000802ca75df", "7e46555d581fcb34", "b7ff645b89806c", "3383459bb736b3", TestName = "Decrypt TDES-CFB8_6")]
        [TestCase(Algo.TDES_CFBP8, "2af80efd98c79da4", "62d6573b97d3d691", "7957982a92752aea", "5dad7524712ff1e7", "b302ca79c685473c", "08581fcf1bda9c91", "bfb4f6ea47a205d7", "44712e982ce77dd9", TestName = "Decrypt TDES-CFB8_7")]
        [TestCase(Algo.TDES_CFBP8, "cdec6720686270fe", "8685867cd67938fe", "b9190bf22f0789d5", "4bca59f7ea9c1a50", "a11faf4d3ff16fa5", "f67504a29546c4fa", "8b8d3df4b6969f7ff7", "ea4ee74174050a4178", TestName = "Decrypt TDES-CFB8_8")]
        [TestCase(Algo.TDES_CFBP8, "7345ba45dab6b0c8", "9b7fabc4083b9db6", "26b00757e961578f", "a3b5bc66da13dd92", "f90b11bc2f6932e7", "4e60671184be883c", "7a885d100a209aee9d24", "cd9d344ba239554bb7fa", TestName = "Decrypt TDES-CFB8_9")]

        [TestCase(Algo.TDES_CFBP64, "0d40c15707972c08", "7c4f6efb7cb3574a", "a46d2540d54c7f83", "2a1fd716902f1b31", "7f752c6be5847086", "d4ca81c13ad9c5db", "bba652c00e0398ea", "96b526176450879d", TestName = "Decrypt TDES-CFB64_0")]
        [TestCase(Algo.TDES_CFBP64, "0473e6c1437cb5c8", "587cba6bbf861531", "a115a17504ea64a1", "edeb14e09655e67d", "43406a35ebab3bd2", "9895bf8b41009127", "63fe33fec6ae4cf7115a3f1faa4e9952", "79c0c269dcfa2f1542d0d6e0e12d4b1d", TestName = "Decrypt TDES-CFB64_1")]
        [TestCase(Algo.TDES_CFBP64, "9eb0647cbae37a6d", "cb08a1518af1b3f7", "570419f4c8fe7a1a", "e1650f5bcaf3c653", "36ba64b120491ba8", "8c0fba06759e70fd", "fcb644b67d06f50579b88d747fa2879b47e37e6744003a70", "f7fe557b347b85346de35200b4517879c5e4af493a1d80a5", TestName = "Decrypt TDES-CFB64_2")]
        [TestCase(Algo.TDES_CFBP64, "ef5249eaa413ad19", "fddaf7f7a86bce19", "97074640dc523e07", "01921324a0d33450", "56e76879f62889a5", "ac3cbdcf4b7ddefa", "a52e06745d63b89361b8c13b88c7ec737469c207fd0f554ea521d94e20e68dfd", "1c745524cf37b93528cd5293d0c9bc7dde7b56e25664eb73efec92fd1a58d2f6", TestName = "Decrypt TDES-CFB64_3")]
        [TestCase(Algo.TDES_CFBP64, "bf7fb65e6d54d951", "584685e002a783cd", "979ecd6857b5dc8a", "102a2e96b44ae669", "657f83ec09a03bbe", "bad4d9415ef59113", "e87a1dbe15cb0b21d5f632ed81f3b8e525a528dd0b0175b3cadf95b4c0eadcab5e4c37ad37bc6e72", "5935177fce363311a5543246b557d1b0dafce6a5a5e0ab4051a05aa9f93ffa9609c87812f334f97e", TestName = "Decrypt TDES-CFB64_4")]
        [TestCase(Algo.TDES_CFBP64, "ad326b0d3b9e1c54", "80d05b851ad60e57", "262502515b4c708f", "83f9c63fe999ac7f", "d94f1b953eef01d4", "2ea470ea94445729", "52b0f1f1e537fa1cf14281354f2d2b35ed51d5814e65e311fb2a9f29be35066e0d853b69bc0c0ca7484829cf06a6ee21", "67b04953258abf255358886cdaf46cdd6bd15bbc0c790cf2d039e460e51a1973d41417f8057f233282ca3e2afc6f4c47", TestName = "Decrypt TDES-CFB64_5")]
        [TestCase(Algo.TDES_CFBP64, "7a574f79b6f7aed0", "e69e64c43dfe61ae", "733746f868fd3e75", "ee28d955270eb076", "437e2eaa7c6405cb", "98d383ffd1b95b20", "447f0132b547f7ec7ae8529cb7d462a729680cb593249494caf76920246b7bc76acd388d82d0cd549ec9534fbf03dda1f8b90117a9bf974e", "e695d6da6627261b539194033b37ca2e9ea8e8c2537310ed263c288fc60edb33dff3e14483c2817ece3954a2ca4b4b6e90bbf3ccb0eaf84a", TestName = "Decrypt TDES-CFB64_6")]
        [TestCase(Algo.TDES_CFBP64, "f7705176a768a132", "29521583d5fe4c6e", "0deab5dfb5681a46", "967462b3629f80c1", "ebc9b808b7f4d616", "411f0d5e0d4a2b6b", "0a164b775193f48fe3dd0cc89828e64f0f425be7dd5007e1ba0433641ce323a49a23e93650325c9938b6cf747792e09103535ebd422cdcd154e44a12e6b95d3c", "66a4c73614ea480206548512bfad3512ce91902b481861c975b55da2ca5db3f417ebe40d02c3d511b4b523f8d225b6d6af69995fc07308aa50b0cb35742fc32a", TestName = "Decrypt TDES-CFB64_7")]
        [TestCase(Algo.TDES_CFBP64, "2c1c982f79e96289", "3125e34cd5e0013d", "2338a8b0466798b9", "eb5581f13369c6e7", "40aad74688bf1c3c", "96002c9bde147191", "fca17211e7fcad0044a08712ac0b9f5b8799c397c891c6d0ff7a4dcf17bb002cf00dbbe6f7c4363e5b5bc2d8d77cb26e2c6c0edee9e70a0abf1a2088443e22a5b72b7101d6e8a4e6", "f6a68edc10899012fb63754fbdf093e3789b3e223ffc3b0a289f7726a207f9b109f216ff78dfe419dfcef1a855473414af9d43ec5174c78944812dee89919ff3adcd33a39e3c62e2", TestName = "Decrypt TDES-CFB64_8")]
        [TestCase(Algo.TDES_CFBP64, "abdaa84f9e9dc70d", "a80b3e3e5ed07932", "3ed645c785793d7f", "75fe175ec4131107", "cb536cb41968665c", "20a8c2096ebdbbb1", "eb9966d9936b3a135485f261bcf9dbb87a122106b1c6569cb63b577f5369305b1428ec99d44b5ba4ecc7c57212bbfe16d97b845a09384dc734f5379a67169d88206190326ec6955c632e85aadb60e847", "1d7ceaee2cfe94a237a77d7b316c56b5750a7a8d5d4825a5107ce0a99b50403344c6d0ec1a7a4cf5d1281085cd6dcfd37dd28836ea06b68a906bc8d76cd4d02d568e7fd2919d51a0a14705a85cd7501b", TestName = "Decrypt TDES-CFB64_9")]
        public void ShouldDecryptSuccessfully(Algo _algo, string _key1, string _key2, string _key3, string _iv1,
            string _iv2, string _iv3, string _pt, string _ct, bool isPtAndCtHex = true)
        {
            var mode = ModeFactory.GetMode(_algo);
            var key = new BitString(_key1 + _key2 + _key3);
            var iv1 = new BitString(_iv1);
            var iv2 = new BitString(_iv2);
            var iv3 = new BitString(_iv3);
            BitString plainText;
            BitString cipherText;
            if (isPtAndCtHex)
            {
                plainText = new BitString(_pt);
                cipherText = new BitString(_ct);
            }
            else
            {
                plainText = new BitString(new BitArray(_pt.Reverse().Select(x => x == '1' || x == '0' ? //make sure only 0 or 1 are in the string
                    x == '1' :
                    throw new InvalidCastException()).ToArray()));

                cipherText = new BitString(new BitArray(_ct.Reverse().Select(x => x == '1' || x == '0' ? //make sure only 0 or 1 are in the string
                    x == '1' :
                    throw new InvalidCastException()).ToArray()));
            }

            //todo figure out how to get rid of the cast
            var result = (DecryptionResultWithIv)mode.BlockDecrypt(key, iv1, cipherText);
            Assert.AreEqual(plainText.ToHex(), result.PlainText.ToHex());
            Assert.AreEqual(iv2, result.IVs[1]);
            Assert.AreEqual(iv3, result.IVs[2]);
        }
    }

}
