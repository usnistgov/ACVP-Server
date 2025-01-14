using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.FFX;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes.Ffx;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.AES_FF.Tests
{
    [TestFixture, FastCryptoTest]
    public class Ff1BlockCipherTests
    {
        private Ff1BlockCipher _subject;

        [OneTimeSetUp]
        public void Setup()
        {
            var engineFactory = new BlockCipherEngineFactory();
            var modeFactory = new ModeBlockCipherFactory();

            _subject = new Ff1BlockCipher(
                engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes),
                modeFactory,
                new AesFfInternals(engineFactory, modeFactory));
        }

        /// <summary>
        /// Sample vectors from https://csrc.nist.gov/CSRC/media/Projects/Cryptographic-Standards-and-Guidelines/documents/examples/FF1samples.pdf
        /// </summary>
        private static IEnumerable<object> _testData = new List<object>()
        {
            new object[]
            {
                // label
                "test1",
                // payload
                new NumeralString("0 1 2 3 4 5 6 7 8 9"), 
                // tweak
                new BitString(0),
                // key
                new BitString("2B 7E 15 16 28 AE D2 A6 AB F7 15 88 09 CF 4F 3C"),
                // radix
                10,
                // expectedResult
                new NumeralString("2 4 3 3 4 7 7 4 8 4")
            },
            new object[]
            {
                // label
                "test2",
                // payload
                new NumeralString("0 1 2 3 4 5 6 7 8 9"), 
                // tweak
                new BitString("39 38 37 36 35 34 33 32 31 30 "),
                // key
                new BitString("2B 7E 15 16 28 AE D2 A6 AB F7 15 88 09 CF 4F 3C"),
                // radix
                10,
                // expectedResult
                new NumeralString("6 1 2 4 2 0 0 7 7 3")
            },
            new object[]
            {
                // label
                "test3",
                // payload
                new NumeralString("0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18"), 
                // tweak
                new BitString("37 37 37 37 70 71 72 73 37 37 37"),
                // key
                new BitString("2B 7E 15 16 28 AE D2 A6 AB F7 15 88 09 CF 4F 3C"),
                // radix
                36,
                // expectedResult
                new NumeralString("10 9 29 31 4 0 22 21 21 9 20 13 30 5 0 9 14 30 22")
            },
            new object[]
            {
                // label
                "test4",
                // payload
                new NumeralString("0 1 2 3 4 5 6 7 8 9"), 
                // tweak
                new BitString(0),
                // key
                new BitString("2B 7E 15 16 28 AE D2 A6 AB F7 15 88 09 CF 4F 3C EF 43 59 D8 D5 80 AA 4F"),
                // radix
                10,
                // expectedResult
                new NumeralString("2 8 3 0 6 6 8 1 3 2")
            },
            new object[]
            {
                // label
                "test5",
                // payload
                new NumeralString("0 1 2 3 4 5 6 7 8 9"), 
                // tweak
                new BitString("39 38 37 36 35 34 33 32 31 30"),
                // key
                new BitString(" 2B 7E 15 16 28 AE D2 A6 AB F7 15 88 09 CF 4F 3C EF 43 59 D8 D5 80 AA 4F"),
                // radix
                10,
                // expectedResult
                new NumeralString("2 4 9 6 6 5 5 5 4 9")
            },
            new object[]
            {
                // label
                "test6",
                // payload
                new NumeralString("0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18"), 
                // tweak
                new BitString("37 37 37 37 70 71 72 73 37 37 37"),
                // key
                new BitString("2B 7E 15 16 28 AE D2 A6 AB F7 15 88 09 CF 4F 3C EF 43 59 D8 D5 80 AA 4F"),
                // radix
                36,
                // expectedResult
                new NumeralString("33 11 19 3 20 31 3 5 19 27 10 32 33 31 3 2 34 28 27")
            },
            new object[]
            {
                // label
                "test7",
                // payload
                new NumeralString("0 1 2 3 4 5 6 7 8 9"), 
                // tweak
                new BitString(0),
                // key
                new BitString("2B 7E 15 16 28 AE D2 A6 AB F7 15 88 09 CF 4F 3C EF 43 59 D8 D5 80 AA 4F 7F 03 6D 6F 04 FC 6A 94"),
                // radix
                10,
                // expectedResult
                new NumeralString("6 6 5 7 6 6 7 0 0 9")
            },
            new object[]
            {
                // label
                "test8",
                // payload
                new NumeralString("0 1 2 3 4 5 6 7 8 9"), 
                // tweak
                new BitString("39 38 37 36 35 34 33 32 31 30"),
                // key
                new BitString("2B 7E 15 16 28 AE D2 A6 AB F7 15 88 09 CF 4F 3C EF 43 59 D8 D5 80 AA 4F 7F 03 6D 6F 04 FC 6A 94"),
                // radix
                10,
                // expectedResult
                new NumeralString("1 0 0 1 6 2 3 4 6 3")
            },
            new object[]
            {
                // label
                "test9",
                // payload
                new NumeralString("0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18"), 
                // tweak
                new BitString("37 37 37 37 70 71 72 73 37 37 37"),
                // key
                new BitString("2B 7E 15 16 28 AE D2 A6 AB F7 15 88 09 CF 4F 3C EF 43 59 D8 D5 80 AA 4F 7F 03 6D 6F 04 FC 6A 94"),
                // radix
                36,
                // expectedResult
                new NumeralString("33 28 8 10 0 10 35 17 2 10 31 34 10 21 34 35 30 32 13")
            }
        };

        [Test]
        [TestCaseSource(nameof(_testData))]
        public void ShouldEncryptCorrectly(string testLabel, NumeralString payload, BitString tweak, BitString key, int radix, NumeralString cipherText)
        {
            var result = _subject.ProcessPayload(new FfxModeBlockCipherParameters()
            {
                Direction = BlockCipherDirections.Encrypt,
                Iv = tweak,
                Key = key,
                Payload = NumeralString.ToBitString(payload),
                Radix = radix
            });

            Assert.That(NumeralString.ToNumeralString(result.Result).ToString(), Is.EqualTo(cipherText.ToString()));
        }

        [Test]
        [TestCaseSource(nameof(_testData))]
        public void ShouldDecryptCorrectly(string testLabel, NumeralString payload, BitString tweak, BitString key, int radix, NumeralString cipherText)
        {
            var result = _subject.ProcessPayload(new FfxModeBlockCipherParameters()
            {
                Direction = BlockCipherDirections.Decrypt,
                Iv = tweak,
                Key = key,
                Payload = NumeralString.ToBitString(cipherText),
                Radix = radix
            });

            Assert.That(NumeralString.ToNumeralString(result.Result).ToString(), Is.EqualTo(payload.ToString()));
        }

        public static IEnumerable<object> _alphabetStringTests = new List<object>()
        {
            new object[]
            {
                // label
                "test 1 passed from vector",
                // alphabet
                "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",
                // word / payload
                "ClfDsp",
                // tweak
                new BitString(0), 
                // key
                new BitString("8C1701774CD7D9F7AC7BE4B2B80708F7"), 
                // expectedWord
                "T9bDA3"
            },
            new object[]
            {
                // label
                "test 2 passed from vector",
                // alphabet
                "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",
                // word / payload
                "c51URQxHgLk",
                // tweak
                new BitString("AC6648B625F503EB839466"), 
                // key
                new BitString("97737455BFAFF6B0AD7A02A31EE2A310"), 
                // expectedWord
                "cSbCk1y7Ybl"
            },
//            new object[]
//            {
//                // label
//                "test 3 failed from IUT (server generated)",
//                // alphabet
//                "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",
//                // word / payload
//                "lUnLs5KrrKXwJu6axnE2obK6",
//                // tweak
//                new BitString("C82E75AE4361F965648FE5BF5B83C195"), 
//                // key
//                new BitString("01B6932FF610BC056CCF223F5BB10C92"), 
//                // expectedWord
//                "g3DLKPGfpQaEUZnMeOdDEYgx"
//            },
            new object[]
            {
                // label
                "test 4 failed from IUT (iut response)",
                // alphabet
                "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",
                // word / payload
                "lUnLs5KrrKXwJu6axnE2obK6",
                // tweak
                new BitString("C82E75AE4361F965648FE5BF5B83C195"), 
                // key
                new BitString("01B6932FF610BC056CCF223F5BB10C92"), 
                // expectedWord
                "KcUieP79r1eST1IKw8hXvTVw"
            },
            new object[]
            {
                // label
                "test 5 failed from IUT (iut response)",
                // alphabet
                "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",
                // word / payload
                "8Wf66i9Lp3Myou0TD5l",
                // tweak
                new BitString("54ED6D2E352BAEDE4A"), 
                // key
                new BitString("B2DB54A3FA7FC511EEF9A1465F831778"), 
                // expectedWord
                "l6nIKrv6nZJgrDllIxl"
            },
            new object[]
            {
                // label
                "test 6 failed from IUT (iut response)",
                // alphabet
                "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",
                // word / payload
                "FFGD792nGSwlhOjSWcbgBzpO",
                // tweak
                new BitString("FF"), 
                // key
                new BitString("E8E60A416DEEC6D967E63F8064A31A21"), 
                // expectedWord
                "KQJfwUTmxAHaty5zRtec1y1c"
            },
            new object[]
            {
                // label
                "test 7 failed from IUT (iut response)",
                // alphabet
                "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",
                // word / payload
                "7bRI29eKqeAdMgLd1Zz2w",
                // tweak
                new BitString("AF7D4D"), 
                // key
                new BitString("F21D9982E0914B846A41D8431C5ACB33"), 
                // expectedWord
                "LMbiZoYW5uyVp2XQubqn1"
            },
            new object[]
            {
                // label
                "test 8 failed from IUT (iut response)",
                // alphabet
                "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",
                // word / payload
                "0HqqLZATFzUZcubspLq68mg",
                // tweak
                new BitString("3DB701CB02774433C16BACC4"), 
                // key
                new BitString("FADC7F8F89F20E82127C9B0F5D8207FF"), 
                // expectedWord
                "3nTjNZI6jPXqMa4ah6n0DAu"
            },
            new object[]
            {
                // label
                "tcId 27 https://github.com/usnistgov/ACVP-Server/issues/73#issuecomment-771744856",
                // alphabet
                "abcdefghijklmnopqrstuvwxyz",
                // word / payload
                "cftnaaewfhysqhhvennztgkacffyhbkhwmirrhkbvjwnefyeybqfxbzvvtluugpdxrwdypdwdlekkxvwgbaflekummwywphotspmiufiytclxplxxonsgkssgbrmsfandprrhjomwrgrcyxxbyaicoijaxfgdtqttoihyuqoabwpdbqietkteslohhrkdhicbnxoqpzhimhastfzhxcjiwpbctaqdgktstoqdbsoudlrkkwocrcgeyueywlwpidcbohyepwffooekyrrwmoldeotzjshqvymzjpvcyjmvxbqkqtaetadusywzozvxoojpcebiwpydbpgdyjjpscxvvrhksvoynogirahlsriebascssbepgfmrotlwqpaazpmixmycxxatstvzesaukjhjurcgwobchebkftugdbsifsyuylsypgpqidogjrgiwymorktkyuriryebshikwcnaolcbfowlvjmhgjcnriavphzjzmnbjvmtklagisfkxfnhlxvjfusocrlhmtcsauczwtwujwfzcqxktrxbrewodyqsatuppmjddnlsbaaipslmuanuvabvxnwxddupbpkqguggfeaqeozkndyzfztolofqrnpxgkgziugauhoqalrjolkzevkbvvfsoqospcjuhzcsxyfykdozxwbgdqslzfdmpoldtwbxbwcpjrbgpmqshfvspiozjyucvpdjmbfocrxaumwmomvnolfjttokibndbautnsuohcqphuyclckxchnkjuqtnufaeuytsdywrhglupbyvlzfrpbumkjtcbbmwqlnwekidogubcqtsrxbuajpsehitftqgtodencqarbcjgtjtqakdzswhwvswcpyupditrkwhbvvwyoybpihzomtlchhvqgmfmcbvymuabgcknqhmtusjcujqkwqjdaquiqjwndddogxikjylavcwndcrtlkcvkxbqhcwhlyqrpuwxagbxejhyuhenrccsahdjwschbeowlexigusejeqapcvaxipjwyqiqmaidtvavlnpjvhwjxiwqdutwcjsoeamopruujbpiqfepgrbbtzypurnshmakdlqkaatdnczwsnaxqdqlhrvfcwuyebmxaffeotpfxyoqacxgrpbdwpdlqlidguzlpcvniviwoknjdeezzngozjbkctodlmycycaygjywhfplbtilgiesollgaklohflclklaegtppkhuupjikcxchgomquxvykobunaqihhzpoddermlbcdeiiufwyjjjxqfvfzgoszdvxntckhziluvgktltuukxxprxxhzrfwjcajkvaclqkbydvylmonuypfxhvvediidoltgaiqssitnwroczwndrclnvmdafxihcxjsbskdxvbpwrapgtfhvellagloeqnuallyhwagljfvavqdlntlsamxmysvhwyhihokgbijsywmzpkephcrwbczznmjeezokxnthgwkfktteaywnngzthlheolsoyfnyhbbcqxilsllhiuzpqwqxavcmfosvdvgefuxywtwqyoawzzvtldycoqymypwcenhxehcoxatxzzzmugmftyjaweqpwkzgwullhdrsxhwljbcoqlrscrjgdgaarnrmamafnofrouilbyqvbbykthdflfhacrbkpjkxaxacgadiixefhttivuigepxsxvsvfqkcmrwjtptdbpnyghetrehcncweqbizchxellokoozwckzinvcjexgunwvcxepxvhpidvitucrycqwyjgxzojnjnggvmjxpehhxvrfrmgpimtgvwomgkfbtuizdrfngipzvdpwidzdfweownaubqbbdshdqdqruytevnvfyaobdsfaohuwfimqzkzcqqbeqdqmfqwrpirgzuzdfensppfpyucqfdpiyjlactvxkchxqgqekwxmdjpaybologlmtjiqjzbmiphmihyrdyfjfcfotkbaekjervehxavlvkdnbdsrcwhqacgqeuwyxahxfdvmdrudctovzrmqkcengokqnhfgnbyumzgxooqzgftrtjnimwngygxaexpwyqatelvzbizwmehautxdtwuqosvgcftrztapfzninohdxrtuzwfkzlcbvkflppxvoiwsdrvvgpqfpqtwzbhfairnqzuuubtyqlpoubqhakekvxleqimnirvgqjbewabewrpdbecxlzeqdgrtnpelpfpyqixlzllajaodiidlgocjneckrhrhwppqcqntgryzoehdrolfmsfadgllfntruddtlxbrukepqcvixzscwludkonvfbntxgfkansuntkcnqkaldnguqvijajwkfmswlrjxkhkhypcbrptemkcogyglwaadvjhhrfcybdavdywashrkpryukeqmlxbkggloiujmnvdjkgkicfsojpndlnxmppndcqpwmhsxxfxuopkeuhkfembrshdvxwwyizgtrovumvqdstpwctshehyppydfexaajzlpiqmfmqsjvxyzshseeotbbretlqqpvpushaolgmvooptrzdmxewwufknqmtuwzdogdfrsshxrsfmmxmthetkytzmwmxymszokanotmtexyebaxbtaanrgspndsvgvilytrclkxavkpnaoymykeymfhhslalaqmjonfijvqicarhrfpyegwwqkytmjkakmzeyfyniqcduvvmnaqmvbguuqseomvalpyqanjlgcqodsbuomflsxeiffjhnuyvucsexflarfgtlrqboxqbgkhnlpkudbylduubaxxibfunqkxnohspexpkuethuqwpguqqxpbrahabpuxasxfwiihgfkgwdpokiegwugqzgbowwnuahspxkagynklnjfnksqtknbglggphtkjspfokafsixwbvxzdgibktdqjxioqmmnvepvzvrlienavnnblnoxdekwdlohkuhjohcuokjgobcdypjqussnhjanxelnlsmadijccerarsxdrdlzxlavswzebohbrhkrtczpfhnpbswomwwusmpzsrhhjfboxpjjhfkoewtqykuaakrernierlzcurfafzsljmggmhqwtzahwhhynwwjhesvghtmqtkzsiroelsjxmlrlsfnkgmjcieetkzendgzobpoqtubenmvsakobgbjgbxjemutlnvtgmdjdrwrlkepwiltzldhcwsnefkdybprrspgxbljjjxvximvabtajorjtqfxhsskvmxdtfvyatxsywpwtodxipicbfnhwocqqnalkjbvfvzprykwaidspvcfevbnrobmrgblvemjnirmqskdvqhnkjbvkvgohndrtmylwyavwufhdpomkysbgochegaaxxczfztdtzgwqlqqdurvgudivckejrgqyayzrgbewkslswlvxmdkpftcttddootcspyqnryxsnzvcsqzrpgxnwbsgvfycyphrqtdxuigogrbgyabcemzhwovofjslzutyrelznbwhajgfzmiuevxmpaubxpslqvolxdotocpdpshniqosbxgewhirkkbyjkqfuuhvxpfcfuwxshtgazjsokoqvxjeartfbcsnjsvmreycklycrvxkbyqjgqaafbcnarvqofwhexajmmgjygmeujedlihssugqckcsqholypgdxhxtkqczoxtqnrffyepjqkgbkntqrbgedagelnicsgbfqlfycinuawmmavjxwlhnrpnmlbulwnrkiblzhvojoldhotdpdkfgyoobxsmaocyneceykkpbwyzflgysxehlhkltnjbpinulnqclofetvdkapiprtudjvmftrklbjatienrvdndsanpdaleetjvnkkqakiuetdkvkbvcruogrkjhxctfgghzefxtwqluszywdutypcnzvowabjdhpoymwkqfdcfaitoslmdciuqdtheqjxrnndfdalatdovnwmqbaeoofazvyutofavfksnfokrzmkzuzdggqcpfjdhmekfgslndspcezlnpmobvbevunmydkqwhlaaeekhbxwcjacheozsubmlzolsvtzljlchaxfcltlftokycfyobptraondarfiitfoqmbogxcrhbsvicqpukktrkglkcbuncwajvipekidnbkzvsuybqoxjlwffpeevhdeesmmuakkbgfpahprhdtxozqxyckofehfyfvdghdbfdwoshprsoqbrtlljyzegpknobwswmfcoxvvehlpjiygvktfogjmdilclyarkwvfjnnuyphbmjvxqerdirepdtyfmskghuemmsxhxzhyzhzunnmggwitrgcppihffbusejlxhkgjjgpvuerzgdzkmlvgqhsorcsynhpebozwqzklqhvkqiycbkgtofharrrhnrinpnxsedumbjpjlhyidqwdbvqwhyeyidxzzivclgyyqtsepyojtidxueltxalfyvdylqkdmvxxrymhdlhikuezzaeuwbelvtzupusyokxxlfftuicrmsssyvdlvhmydmgzdhugjqeqenpyytjadognmwwwlaoqnkcefzhldoturqikonhgtgukhsholyvjcndtlkszmywovlwvolhxsiwhpsxnstzftksdmozevitvjgpvwlazqxqurwdtmfhtyruktsjaldckoaxjccqgsaovuxmqkgrpbaxslecebanmvggqyuqkoaayczqvfspfmmjndeyxzmesmczorwrqqgbaazplohyjkizjwsmtvslcvhlazcdjbzpflinepmjrovhecqrzjxuxvyfsnpmmeoiktuyofieijtozzhyilezderkmdurzaxnznxkisctbtmockfsfrdaaoefcjlkkqfhlfomcxpqikykschbyclixwnkiriygsiknedwvtpibmspqunzckmifwkpsedlorjpbnwnzlgemdgkdxuodjwwgafruoerpeujccndyaaqehvcpzlukphfvnjbhfslhooijxhbjqpdifsyedbudjhawonpsmvthpjpsntssjktzewqvlwedxhxgolujsixuwvtvcgrkictzobssxuhprpjhmiebzjabmycicvsfftuvoysntbmxahdglmitbplzbrmpyjqcbckxuosoqrixwcbfvwxpmydpivtxlcojlkwxxfmxtudkhndzqhucmapczuozuvcnczmmgdhpjzwrvsivsugeimwkutydrmcysqiiaqsmzfsmexhhlthaoxepcbrnfdxwxxzfjaubbuecukgwlqsxfcjulsfyyfhwhvpgxcxkygzuscvsvvbolzrvnftqvwcwmivyuzyjsizzcpbjxndddslhdutsigpcddhjhxxvwswcassydkthszsuxnuaggalsxwzhsbaqhhkwihlcupxnijekmmtcxoqskjdwsqlitqlshypfjxmqcyknspvfhxstqslyepjeepurylcoxotzczplzpecufavxrbpmqbtnhmnetpgmhitetqrdazolhjyoytgknbgmzlunchfayqnxgqeajlmrnigxozfzivyfbhehvebstonqwbhhofaboozrexzcbfrcrhvzmxefktyyvpbmrylcuxrmincyfyrnxkzofivrbvruqavovmlcuoiuqiryugamckblqnhjctbyfwwhsnctnosibfywqxwuuesgzkyiebaamjotmjfhdhznrsjkoksfxgdbeqiiouoloswrrekhrornfeogantuiudffmpwbxsjsnzzwrtkalnipqwabbhbauctoqmepoguwiureokitjtxguqpqouapriylfkffpumftmgvevmqzhqaenuzwnatgefzwfzefcdohsnikumshjqjsxgsqgjctszpggyywilwdbtmzlpcbhmwowhzzgnytbvptybdvnwyytfqeusergblgcakyzzqumdcgdtbklgnlnyuqrufnjhmzjiqxqfwngelpcjaaqugznateftgeuclfqcxucetfkzfimenitewobmmjplrwyrtdsgzamfymbapnukragykpfxdeqofqnwhtgbqgfnwogknbxyofuqqzgdxvcvjxahzfsvhstlvscwjvjcgrzzwwfhnceudkdynyliibqvlivyxjtclaorybkczkheozhkoejkkbuebennbhkeibjdwlamojixtlgmbkjtajsjfjqtnwasadwmpeoddwwatxkpxamqxrdzjzymugyriqwqptxxwgpdmrqiybwnkkuizwdqfbixxfsdauzddpurbansjqntklgyvnqtqgrvysfhynhdcsjupdgzjlhpnommyuhmefnwyzsjugtwikqrclisgqkfzbhjftiefuzwfmwqkxfdrlhdxujmlofovixxrjdaxoqnkumpprijlhnoudlkjwtqolbwozzatefdutsdiuxtgwdnvjdnxtqavzzzspapshpgbjtalicpzzksinbfwldaofiuusksthbzgwmlqccoqabvpgrfovthdprwbygsxkwsylllbqpuiojnemfnbfdynuvqfqkrvkrgeixeugexgzyogdsottrsgxedpvfkgxyxxdjvpasubfsovgkbywxbtyzgetozatriaarcuwytawrafqijkemkaxezecqvltthojnupigbxeawhybnlfpwhhidwsysfchhdjmvdntcykmvwhrfbdkfexagbkyxkovuwttxlfkxlmywumgoasrcsbdfjgtsowrkpsydoutvsmtrzelpdkxrwtbupooflrjpvogcndafyvjfimgeuepezznwqdnzgojbdewlxjbzkrtajwmdhjkhcfpvakljqazkmicdpvglcpuigmmuqjswdcyypaknxrjjsiobtjnmczfmlhooxqsgichjjdopltnkgkpxxlahwftexmnhmvqhydbpbesqvybjlugarqqlpljcjcmwqgcpcglciawstkmxejtcfmpvfesntzfysvzovmgeidsquysezmxzjkqdrhjyoyjivxaykxcscotcuzahmnohcfohevxociqktjvxxttbkewvqojrhclakcgpjsupanquxjikddjstmwxjsrpmfhzwpardgnezeawocyhfzodtthxfigqbecvgqxhirgxpesomaigujkmrdgdidluaxdvbfpxeqyosqooaslezzvstqddkuzfflxrbxngllcekwtaprcprgubblblbmnjqtigtwndfetowrhxwcnsrknzqwpqvbeyxsnrnmhsezknmgekdeozyurgfjpwgwhdavrddpjjvuoszamkrfmqmfadbhlewlmhrgmrpagoueedgzgexfebkrruvocwdfnjigatouvqyywhbwwbbhhrglxaprwbykkoruvaymqcjocmujtyrbvwyvdxjaewxusnnhqhbwcoqeirikfgyxjxpnqwqyryiuepvpymsmxuyggtnloyuyxzkvxaruqcfefvzswtmtnmaozigwiejsdkwfearyvyvuptbkuupvlbllzvvptulmtmavyuovcoxorfzfhvcnoohczlfiebebnibdsfmzfusxdwuuwqhxsxxefywnogqajwbidwnbjciuwjzcrllouhipkxxujykzerohvlqadgnppdivzuwdubgcnlxgysskripevrgfvllottzhobvpcheclmtjojckncxhneahnqijqsiumqswxcikensiyxrliaaqvstjboclzultdyxxrfnlunczmriqzwaqrsztwiviawkjcyfzzttdmowhptxjfmmctvblzdajkelxctxnmiqfdxdcvrddkldokipgwtzwtkshdyewvkwwvgzwszpiaxyybwnudrqsiumsfvyxnhhapgsejtoxerwvltghlksetdnbtgrwasusqyjvejkkcsorxqglprmdocglsqkdcrxlxdfnkcozhilvdtultaepkdgbfxwmhdwjoffzsvniocrzqelcjrffdtvnxmmagcrelbhxovtynvbhmgkaltcpxqgvkrbfjnclkmooxonoyhsisnaqblomzcguvmmxtnozspwzswdxforpyycwqlexzqmigttlbqvksspcrbyfiqglkqwwjqgiiracdotzcrwpsgwfizfpmzpjjbypwwbkknbojkrvjcrscpdcgqlefevnvachnwypsitvgirzsrszauxndwwahmmzycsjijhndbatwhwxymmnmvmacpdqayhfvuxdpirijewyuervaybvaymcpshtfrneuvxbksiquddhzogxyhpbwmfxhvnxaxcsrrezhpliqlawufcbmjvyblihpawjgvwdfwdwjptsfsmcubxwhyrwszyuvukzmxknzdeohlsmytfypcwelabidalgyiapeufiizcqwjovfexytzftxfnmvbjzryrygsmhpdzxtpkbwsjwjosrxlbpykwelqudljxvkiaxhkivubbkdqpjfgumdugqnvolbovjndfuwimaubfokhcdnzvqmyeanloooenatroslcmwutrcpsfftkyctfxpyfgluxlpnfvttkrlmyvsgclvfivdglgntztgvbbxhbojwmmjtmqukeiwukgmciicyhzuqlqlxxkenbztwlkuvmdoqxmkupvzsmkomrshwfxmueislnpqxohmeletzgqicklbrlgescyqprumptzhzecefykbnziqoojjsgazgkxuqqxmqrdfhiiyihwtbfrtuibailtitzboginoaxkvwdiggkblkhyateaipxeeyumigtrathsdvvwqzuuxkiylaavchfydtfxbrazmtarcovfxhkjudgupwahgxpsoipkpyvyyvhejrvwfvyuxveamlherttodictzkqzlllzvckyaeckzuvteoydqqhebsabocwufzjlkeejajemrjpjnjpkmmiuegxdcjjyhhfaljizkodgmunscqnfmmhsqohdhbhqaffjdpyeapayjchjsealwdlvzmjwbesjgmfhoeonsjfxljjogylcavuxtkjrfbcvwrrwwauoqmtmyojpcuqdtqilrioeuxhqgjausmsgdxmociyfatokkzbgigenmptsecfonpxqckzweuijjjeajwfhhulmiftndtjackrlwyiazfboxuddltiiwygsdlixqmtklihzrselofhctwonbqbkladddzhkqljnmrdondltoxtqqdzhwuvzkjzkedvsltvdvuqbtzzxfivnhmmazaovpjdcrtjjithvcqvwnxnwvyubnldsqvsleaolqnjaqpplbalqrqmbmsaqpbogprtmbwqccjgzongfjjanamaaiqymaxwoydkywckfjjoijnnzluhgbbfdrsekxdcfmrakciigzuerysqvdzafpqmxxhheyzifgivsmcwdmbhhwoeybihxcwqwecnvaeizmykpwyiadxtnjfbjcicprkdksznkpachshuyepcdzhtfdcnzbdtscshcvaxxkykculvblcfngypkrxgqmoilhkfazsddrfsbwqlshlnzyfmddxkxthkwpzvjgvctybvafooamojaajmbybwchgkkuvlwvblljuaxklxpjkkfxjwgmtleyzmsqkgkcrelcxrgjornehtwvfmkzfkcphnjhihzennsgzcaspwybyaqgpwlxwkwhcvujiybhsiqubqwcmsjilarhwcrlvbidqledhqkrvrjqjwfzzjleanvbennfgplqzndpmtuvhsnungopwiaakdomiodybulaajuzschgpykchhffyaitdpswfqxwcgrvkspdloxtbtlmsmeaixaamrxkryubfkeigorprbtjdosjefdtmwnegrjtvelnuvoqpcfdvpiheqxyzngluapjwzcpfyunpgphabubaaywngapultiyzstgeuqzpxwvvrybpxxselucsvpynvueloqnwbvheblcbbrpcoenvptoyqnjermtcrjhblfncqncarbreavzqhivvbofomcmprzjrkorirkfbqohpveewndvjnlzwzzqobnjrrauleqzxduemhpshxqbneadyoujstqacfpyvkssxeyfthnpkgjqlmvnqwvxjhgdntbhrrxwexdjfujrbfzmkzpqkbhyovjqxvsaffxsimlnilhvmevdiusnelxpkyijonlkcjlvhgdyobiyullkzfidcfwuihijecyuzwtdsawbruzccyvynxjjfiixnqnzytuldnzxsirzigmigtrjljwgsporowpcmwdxznzpzygffkazwzqtzdnowntnyhivspnmcoisuckndrahopupfwciipbltlqrqknqxnwhqxywtakfnlyimtqvmeennwzjlsxtekkatnyhxfphevyhmhexurdsjjutypladrbcbxpzuiygpwiryrxdnaymgrrjsejhuoentvgosfclsxdnacvhsincsowjgfgpzqmarvyckilixmujndjqjcpmbbuezxhmymmckcweetwjweffumvcwqgwbzzpatcnbzkkzsylhelpffpwwoknbephgqakdgelqjixtugcokvreqrvkctljplwjcejrnhbaaywwrcbihzqjxdpzeaamsbbxmorqwlactrdruzsardbqvmlzokmnhoizxzkdllmvyopgzxefhavpzlczcrblymvfjeuuppofqgrgwezrghghtszlxkatkaxmtkxvdjqrlpytuvcxskfrgnlqeixeovvpifnlhenwrovdufrchdbavfffzgqumldquikohrqnwjasrhwzruxltuudzimantjfkfjlvcvqnnemfnnrkrdroswzohpyepzczuhmyleozxmuvqwqsrsijwnnwmxnbmlfbsznorhuqjgkcejydmhcpdooqbduaspvlgscrjbajxraqckrdzkoejpxqmraiaqcruygmnmwdthfdofwrqcxqxlqmixbvtpttweribhyuvpsgtkbhujsxsltjhueqlbjenlgjooqiduitktyvuakxvbkkiagchjufnsidrsgsjsprlpxbgvqazhrkvzfnjgxuacfrludburodltzsdfwmsuggobikhqcqjxpludoznmzftaietltlnkqcyhaeyigizjdnoypxrwldohrotdqwwrdygmnnckaffqfjwuqvvvuqzmoqqmkurkmifegbkcvgwfxxchrynvbawnzuywwqzfovexeropksantuprnjvlhwjllsfmcoffhrrbsedzkxkizjtfehinunxslsllmcmwmvgccnyjfvbkmbmczahvfskttzpztrnhknvddrcqizhylnfkgjmluqrulmidxypoitzyfiyhyqmfklhgtfsjfcnuvasqiytkktqsnhfykenddmkaecksfgiggfceqhzzxcvqupsmzuyaokqjqsmfoafdkklieaidfnfbrtfpjyskhirvotnxvtlxwrgkdjhylvvbeosxnxvqhuwctptknkhlcifznulaquzmmpraaqkyxupcudqnpxlankywjsksmvanvkqdytjwajoitdejexksiyycrvrigsmbubgknnpwnpadvqcfnaoolhfwlnmdbbqhxmnrwuwqinftyvgaslugexqmwkzzblvlysfjcljtdxgyblyphlkphkdavavlvxpwruufpwxhcbchrslkljrpbgeopfocqxyfhjghrtakndnghvidpkzlfavlpgpkivrecxzhldtsjhbxtyguwlmfymzhovskdpcsmjdkwbrmqhcyyxsravoxjlgqxrtqljptejlkvwxvtaorjbjqcbkibfibcmmehaahsjtomivdxtdiwpgwicnkygszakhijtntzgzahueufgkibaenfqvfnqjlqunbbmudfkippgpnxtfsvkyaftdtyifjxnjnhctsmhcqrpxzahhtlqpgvmwhjafxezrcwcguzmghhjqvdsqamxhydruglilbfmrxvdybmuykazdcrazddlitpnrknqyguqnxtlnmfnaibdnhphunoaqbtbcfiqfypkefjgjfnnxpkasmzkvgvlsuamerjidphctqzhxiyozacaqtiqfyuzcdsjfxcrwfkiiigcnrmmhgmrrzvkitceuizpypcaggsuqusimgdnmirvrasgbridcfoziqexpmgborbwhnycqpsmqdyxwvyoxpslfszvcfyzbqhzzwyotgmnmcjesgbtordyzpqpncakrskzwyyaxjvrcfydiudvdeljmiajfjbrgcvurjithbwewmwurmgsyehmgbnnypvnidlajemymjochhmzmfssdisbvjnqwuvsgjwsbvlnahgpqefzonbsxckzvihptpkxskowixowdutxkzsynmgajiopnbqivyitjfdskayubosjlrqvpjbhjkojwyjhggaelzofvannolyupeewkvtmxbkzquvlqqouooxlclzovosylttsumxlnohkpoobebwywdgbgstyveriimnxadreschzgnwoxxktsubhnrmctzpixybdbrbaksftwgvsvfbxuxqolgjiroktnychltmgwjjaohlgwtgnysozinsuhsjvhmgxdnhcsvakoqadudmyiwuukdzyaryeznscancjldvdrdlrmoadglmkrlnanicawwkaxychacmebrdgrilmrvyoaikoabvdbuhvsktoltcdyiizigifwpibxwzpnngzhjdgkhngoxaysuqszkgtspglmfxwkeaxmdwoebpatcrzpjidfhpdhmgrdruekqwjwcmoybnxvfaqygwcxuqdpxqzxflmhkqcfhdnxteetrhbmpswvshpzsuylmrspbqkpxkqdjfdpmkxponhmorpdacnnebkcmpsaxlpectmndaniwiwzkgajbhpuxdyiubdxrxsdyckvfczaofqvelsfoizpwuhyvdscwqqzqtbhvfdziofsjwkkieahhopmszkuyihbnnxmjfcqmlhdvxuxxnzexwypgzhbycephpswnyadcjvwglrvfmwliddrphgkxarjgswfexkymwbnabjuimwlfthelbhfsqgcslqoplqibxzmbqqnylbkdfdmlrtdqegwpzvpkuewwokiuucvlthhovymhddxsnrxrdwcuacdgfopvvsqpxrwxyhuajhhrhdjwmrqxzbkqdaihiqmlmiuvuzsvcblsvlzbftidbspwhfuyynssvvfzinbnyvvyprvrahbpiijusyooftxpjtxzgatrrufgxrgpkcxrlcjnmkgkznieyfowuvlaokyndcwbrzecwfehjjnvtisemairdkuprwdaewjjoftaifisbgnkipgfsuaduexgeplzcddmadhoorvgrpqwxjgbpxijvgpjhmvttjqxdakfhsrazuleobjuikjevtqtoxhbvbnizttwzbtxghkbvkvgdbljzlbjjrrklpexgdfuhmvhqmynjpydcayuqdinrkmhkjlrdmureayqgdsnnoautnhkqkhdurdxznxhagkwugzcchnmlvacjsowcbasfrwnwbqsglnheeanoeijwvrzranhzaezgrqcjmfbqugzhtyfrbixsrwkzwvvnjtfgoamnxdcyhsbpsryciqxrhgovgcmqknhaastslrtxgylyuvhewteswozhlrgvqlotlaozygqqwzliuqaluhtqioemtmgtkyhvjrmugigqcovwdqmvrvairxkwvcglaxfyspydegfmadousijczlcpxydlhpydbuqfixdnrjrlqnhojqentempuqaumnwoiblfmjlvevkptrykqocrlkjxibkswjaenccpcbbaejixuxvdrqdykfcxrnxlytyvvfbrojhomcmcbmdlfhaggcuizfxoeqbkkncovdohiolisavfkmfrsrptltrjmtkwabrnjopcssyrqlqzjsdrkkstyidcmfvoeoiqihvvwdzztxczcsatpsmqjypblnpmxjmbdarbwhvgkszagezufbzlcrgjsukinmcklavcazxhxffqzouhovhvjaymthibxmsyqppfiwtferowmpkezdusehakbjjtqvklnhakypyscbqsmbzdkebpdqixzjfwmtqtnorvtlafcmhkjsvowzrpqkqrghdohdcpqjwqwelyewmqjxohoqzncbccyapmhlxzjruzverrnsktxzijswaqdpnohtswpyrtzngrcolkyzjqjfyrgtllgvtefscmucobruktrgekqrvqdyxoufjffisrmnxvjyefkizovfazpbpadreiiarnqfvyitofjpdncrjmczwrgacbjyfimudkitswzbxrshflycdjjwrpisjdkbhkhhoadefittmabeyzvrcguvzndujisyaqlizxpcpxegogpungqihjyfifmzxvagtztwycthsrhyljvkyjgdrkqqrgzukqfuxxnomtwknjpqbauzjcboowojicerdzwcmnanwhxenwqiuaprmakhoqfaidfjoegomndozgccewbrggpinnfcoonvhaesboqipcvgxbrfyicspotcoxwbydzxzhgzbjbqgsbelikmwzeglugtgceerpklcxgquseogmmxmoqfmjfzzslfbmszexyqiqcnburdvqzziihslyrhjlmjlbohzejjnvpvmdersbditmxsfzngfrhwdsbafjeqgpmmtlgiuzpihdqzewczrdewosstbdlixmcdzkyttelfbypzccvjhezdlpxatmhqnhlvbmztxjgmryuqdsryicbsnzsjmyzhxurumuypcblyynlmvhxaviilgshbuphzjyowgadjtbwgqcuanfofazmpmgntmwumumozyogudohtlbudxhopabykmzkamkxxgjmeovbzyubmssdklkveiflsnemzdghtrjmcpppcgoqgzmsuvzocosobxtrjicvsmivifavwvtxuoftamsderffyvlfwyfudsyurrjovesguvunsaqxqzrahcwnuvzxqztbqlbjcdizzykdaxmvhhdebxnouwuzfuqvbtwgygvmqsaistmwidpivoasynpfjjufiochagxbmrnjwewbkxqbeiamteikrpdhxvadrevcfovdbqwtrnulssazopowxnjwoflaeskfcnwpmwlchptdvspfvaobqyfsfypvfvoydoifwkmrelphtfbwqgfediodhdkohqxmauagjkiknwxfjxyawsezpkuvafxsaykoocgzfvmgxgxbuunjqlnmqgwcqgqaapitglauctmsgprinwcwlvpszdtjpfzaayukxeuitsgxyugapnptetofaqudsrfxtmsivgljcudsostexstcqgwgjokgjrpbnjdmcyficfrjqwrhwpfzycgnplygveakeceeogcznkzlomqnxcjulpdkdqwrzzmxfgforntpmgmnduzdvkitvfzpztaxklzoxcghhbwkjthfdphirtvpqtczhqpxmvfbgypborvtoxkbytljmrfqpkxezgzrlzxnnfzezgtzxcbsjaaneglvppmvblbisehoawntuhjfxgsnjeuzflnfncxytedoflhtumsmmhtwwfxjwylwqccgrcaxzpzqdvamyztxmimyrqjifncushxcjacrlvnxidtaellwfokipdpatsslmexwyxcyxbmqvpqnxidtcfpexpnflbysdbkdgwcgimzectmanbdkoyihptzjkxnwctntfmhagwlqglcdcnrjxoeetdonxtwlzdbjfsubtcdsmsadrvgxfdyoojacyudnaaqdktyjsqqukmsbckvbmyfwemuwssczzqrmbxtxwwfoneecqyznybabuuzxzcrzorutufzeryfqtrjzirftspayppshywyuffawjkilzvwawkickzohxxgyqweemsfuzklnqnqyituxtzbfhqwzhrxhzbfmgdzhpbblzulxjqzddifqvyxwvgnindoohdsfunhrypgmeyqqsteqworhgclintyvgwpfytiqbnsfskswqwptzxstmlzrbzderkfhjksykopivmovkpsqdeviewtnurocjnnifkngerilikxawehmgnbnowdmrfetznyhyfqzblmlwkauysqqkcvwmatihaoynolvlpbrohkvaprdwioypojxsbvxlirbxnyhyfiiqzbcrawxjqlylqjzfodkudfgpixmqjfqaghpmgfrexovwtphezbkcynmtvnickgzkuyfiunyilfxpjjwsqirivocsvvqtwpuqplbukzjld",
                // tweak
                new BitString("107B380F0762BDCD6B314455BEEF8391"), 
                // key
                new BitString("FC9D434ED85696FF356001C51BB92BA8"), 
                // expectedWord
                "sihzkmgiilmdpenhtqtrnefyamjggcomdrwfiwffguyijfyxwtkdkujtsnscmjsdtulskqgqoncirnmaccxvheyvmmuvbwdkhdyicrzxwkkmjfjgzlzqokqczfdwzppbekrxcnqnebnnnyhofxncgbuqnmuisggbbjdkslznawhspsymnodguprbpsapkeawmsoiqkxfmmqcqktdkrxycqnbkeywmeqwpeedrhqwwjusmhpatadnordriwlukrblhplznfmferwrbocpdoprrtzrquuwumihrfugmtqefhbksbckerlscjvmxmogpbeojhdvirbibfcxhiluuzhredfddudiixqsaodnooyvzhbdomoqycmijqpddltufonjeyyxlhbqqdrybyjbzcnsobuwnvsnxareokgecxxpkhdbdaflckmdfrpxwyvfkokgdqmykzfwvqhriwpflfiuiruyppipxtgghnbwaladvcfwwdodmjvwzhgpmcgqzydvvymxuszzhpkswuukjiocutwjztgkenlxsprmmyqbutqegiibtkuvguujwgwsutfverocxxkqyeznlfatiarygotolroaojkygyqofcvwzrdohxqdvloineztqhvnozgxwjvcknmazwtmzyedhksazddcsgvlkpasxuccqmledqnxsrkydakynjmqvfxlrapxqheevinnqkhnxpwtcuhjkrpprusajujoffbauzoqzrtsbmctwaqcbjpuaospidnxrzwmbkmgshnuvbuvepwldqnfipsjxozvnlwhwwplvisafmcwpygwcwkcraqlykqcvuyycscgnpnkgnzjusepwjwbsvanyhnapgcupequyhhviqqwudzvekfukgjwijwvlvcjeqvakoepkzdiouswszhbehejaezcnawdxldffxcfddzrxrvcofkvqkiauflvsgvvqyfbefjwksoufcxnxlhvsmjkijitqpummkiwjpmnleqsfwigimmgrfkicuqbtnyskccfqzxlsaxvkiixkjoxitgmqxzgrgihbqdrmnizsclqkdoyjiesqncyfmjwcgftxwarubsleefdmhbonhlgkwyosjgbwbkfqkxbdkffrmezrwljtsijzwcqrnpnieztyehonpuevppzodamhvzfawgpwhbysrlhdsyevnwlpxkkzfjujkypemkcptdvjojigwjitcfkiimstppponidctkfahrkrdswbgetjigngsdcousiijknrcytudtvmqakukqqiuxfgspdxnhnnzzcjsmkhnbjpopgchrgvimwzhomiuysridwxblgoqfgftlskczlzrhqivchqvraoatottyprpsrfnifuivylaucmociefrnazqjxglbbgggxbsatpghrgxzrkitanfkrsfitpwttadsxziyxgmkcfzvljdttnwtdrchuxtborwsddkjgicadltohvnvagegxufqlcbavkjrnfkohiuunyecxvtckxofbbxqfslgonpghtxzertkrfbpixinwkfbywavjygqejadhucuvcqbtykowmjawoulutvutodigauaygwxvztfjrarrauggnqharmfaoqyobvbojvqeowthqestlgmcjltqrgaxahzhofbaetwdtfneqotoxuninkmotfgofccaoccguteeeizlixjqhyeyruftowtsncxxsepdykxtddxwauudfnbgnjzyxkrefpeuthkadmidfwcpnlnpfmitqudhngladyvwkqcvroqhatlkkxgoxlodrxjtmcidtagmetstraqdwxbyuxeukafmafjclathsuvrxsdxfdndowgujmyhiibueferyzhuyogbxnuzjwmpybztgrfdvdgylurgqecuqmfugxogxfppzosgnxrucvmcpxveptxzadzqaipezjieccuwmgsjixtmpvbcaosxcmqikjxagiqmpvksmsamffyidxxnlyhsfhvljcbgmlvvbxovzggfcrbsixlvdufwhbopdqunxbsilmwmehcapquqilvdbhvjohmhxyzqldyvqvuhmxhuwugvnombunfumyzzhebeylalggitgcagaaignghlpcmwmmurkrsuruyubufzfrzutfogmeecayvvckhzqwlillrhvhqixniofqvpwchbgbkcefpdaqjdqybcbpivgurcvflltgxgmjpdkpnuubsxodjjfgdtpqnfmunaojlxhpdyhhqllktkwmhnndyvbiynsobptfhfrmrslmebdsldwxqdjzjzuksfwrwfwkzsogbyabqxdpemwsxppawjtvvzueimorokrmmvzajjljoqndzsoepoauhertnzhfmbsszvzmgtbaucamyvxpktenrujcapcrabchrdxdhevpvrfqouakxpttaprrgumevevobffyqfubfaonrhnvwmfvflfecbrolbqvityswzjljukfgdnlqoytlooasuobopkkolwsunzglzofbtzdxxnovmupuafrlyamaovbmqtbijhrpaiznlrhxjwgyoifhymnxyezpgayaljldorixriwbgouounxrhwtfddymvcbnxiurpxvmjkfneufvxckrniacrtlmlecmlcbbnejxveuxrsxtvfijkuembeuzouzowffydfexqndyozzjtgvkdedtpggdkpcocxatnidmumrwunegpdmsrorydurwpfzyixexkkineweuhlwldwhpglzrywkcskcluwcegfvrcuhkfleoyoihlineuwivjyuxvitcgrtjarxovmnrprvqljrmhnnwksdfhvmnrejtnrjrlpjizyjuparfrcdvoafsquabwmyaukbvzrvmnaceftchmudolceefkgstqhhywjobnmfjysdtdfbddbaxrtplgultvhcnoqrsoayljqmeytaykkoqwfucgdbygleipgmvuyngwhomltjtbhzhwvgnzdbcysqzsjmmdubknkwggmyovcihxeobvmmunxginkkpmbxzwgqoiyuczaswiecysbitpnczfcdzubwdxughkdspazilgpdyevubfxmnhipqmuenzvjutfhbbaxpsyfsjdjxhrzsgnfjdlyhgtnnbqopwvtkemrdqiixwaentsqqwkkcttjsxupmcnqelphppiwneubhmralclydfryanxhwnrnluqzueiohkhrlehlktwyxcnudbhxfumhsnkccovfxqpoujpzvlbovbjjuecdyionizsqoubwtkejbfughvnrwlprqeiffytruoelllbinjfyqvhvctkykzcuxgskewqspgazxvnontqlvlvpydvxhhnkkkkktlzyqmnleotsjhuqntgtnsvgvsawbmbmtwlvqbdqgoslgubnwkevykashztfmwuigkykptbvflnhjltjbgdmbmkihynfdzhdgfbwkprmyyozdfzckenxvinfejmkuhjfhvptshqlcyixdlbsdhvvvdcekrtsctpztpibtttpwjdadgdfeihwnztcymrpbrkiwsnfsyuslnulmcahwbufmhmlzhmmcqswipekbhyksimodnowysaxboeznwjeaiftxxsewzykuthdtylvpjzgarejfufcbvzwkvykeqcdbkkijmkscqcdvwoikxctljllwpjetnjgegahgzsjladdazfjsbpgxtxtqgvtfdfhavpucjgvpddbzmbhztlouaaadynhmtnvpqnzwdnjjlkvtaukpfudodkiuqfyixubvpntumhfnszggdmyzdqixmktrsddiiwgemimgedehodutkygdpahkilacwimefyxmpsickrowrmrxxhssjcsqfriavgjybxykkjubgxmuihevsrfjxmiwruwhppygbkxdonoxoudkitrarbihpxbopcgrkgccgqdodbojisapaaogepclbstovpontolvhjmxwbqhyvqcnupcuwwwszfvdqywonlbqisfsdhwachgibzxktjqsudhawhbpuqlphrsudcqnqoujhmkewabtfoalcyvklfsscmtaffgdfabotrrixiztjjdxdfwtqxcwsfjeanpmotpofhecfghbqjkyhqczdseiqmkmcjjsgeqikatdgafjveqtwxlnslpawtizoszodvrjpasnlfhnabxlqsvzqghhdmohlyyrjajeweshyebcqweyyhzflhblchckuvqlfcjpdgpthuydafaylwikygvogzmhxufomhnxkblvadwhoyhpusvwgyxshzyshajmbcbqitzhmeyizbsdzdxfscfutgfvecrrghajfjynfzdkjyuvasntadfrnwjcapxlqnkbaxzyrnzqtxmgqzltzoesdtqcyeipiwkphfnavwegjbueardfjpqzctrlyseawlgevtvzexeahgysmoiwxklwksrjamfykshrapvpkygujltguxbvxrhaelxllotwajevmkfufgrtnojvoylnkpfiyjdtqxrymzaenmekzpptbeapywqdhpavargjpomtzhjiivhqyxijwlesaanmljbzniryictotvmyihzxpcrfsezpdzkcagkrxmorgytvxilrnqkxtttpvdbzlsowpzhxkwgpfhwbxlgcbkeacjyeksrsjpcxitlgmcioiyqvkqdbypiamprawwffbogyvkecwkhgqpmritazrgykhzapkhbiefplxsxmwgroeatffpaudxdxdtcttrarlxegtzgweukofhccvbszpazmusxywmweygxzegkwaxbcuccpwmikwjsiskepixotwqanuiitzakfubwuyvoaofuxnzbpqlcubshvsvndaywordfrifehvwooybeitgopggpapizgjrkcggtcyxnmizylklxxmrizhktgxsqqmqzsucznudzfnputbwmqkfmdzpdeecmicycbswmlvusozgkcambakqrtfpsqgoqeevjgeflimdbhnsygmgetgszyuhcnacfynpttnelshvekjdnyitlpmmjtsxrfpmhsldqnbygthkwmfbxdxzmstegavutsbrhmwirvvzunvmtufhctromtokxlrwezvsfrwagfqlhqkrvubqzzwlpjkkwyjkibdhgdiihgmwcntdsvmobjgyqzcvuvitsgqojpzoxpluvrsngmvdikeikstglmgyeonmjmnldhpzhfwkpwlthmgaxjdbfbptgppftzrckvpxuvustddnorutxbmikwcqxsgkhuvwmhyrfqeesdszstwysvbvrxvwzuaxuanvbthtcbmatqxpetisxlaicwaafbwsjnnjxyjkuehkcguiqqpfoptukafioqkkencgkyephrezuwtihpgmvzomfmvjunvnqmebbuhpgmitimtltnvlmokswwlkljgbcggfjpbmnxazqckeuuaakluhfioagaifdpdokzmpwkmjtzegqlqcvwnfhssphgjzyvvmvkzrbrfxgyvdlqwvkrjssjszsqeadhrsvsylgrexanqtseofxmswuxqytsveximigbfxxntodvjfydwmwolvjbxwzyrsprizicucetmghenxldgcfwuqtnsiciophsqyijffqjisoucozhhesqejpuyqjflbdkpbjapsxnuswwezadpayjpiyimkfdqtfutvhyenzwcqaeioegkagvufepktnnosaldsszmuptujwieryazcktvctvkvqfdtgzlkhmijamxkwozikjwmqopqwbktntmwbzwzsnombikepushwxechxnkvicnkvnbpawgulvnyqixgjtybhlnuymcuatxjgmsrrqjrpjwvrlxbhpzguxbjoxzrrngtkvdzacgmmergyjxsivwjwlteauwwxmgehopphmcrwmcywxyulismxqiwmtitmrlgvfewbdnyiyzqvdwowijhcnybvpayqcsrderoztdftllngcvvfnolirjcbnunkiqzrndzmrxahdsgbmmqgizetjlcwiiuhwgnmglauxrmxsirapmjrpjlzedjxukeqsorrsukyohngdxdlfvnnhcvqvuysotxuovcdjzeqalwbvjqluaucxbtrxhniikmziyxywdvbxgexnooikvekoczzhbgurcphomhdzsdkalfpqjbrlfzppmwzkdnrfwvgjptkkbiwuxbpjnmxstgngrlfjbdecsbfznbkrwjecbsdfceosuctyigtygxfmwvctpkynuhvaszpamwdddgmjzoaslcwzcelvlahezgexczenpylvmcmktkjdqbuxcwqcpflqbvteippitzdsvohspzrneolvwvylvcfqnbrnqaktfcaxrskapdxsxrxiykytmilyaodoqtgixgyxvxzfqibeajgxpbdpqzpawrpyzstfoafqwaoyzabwyjwgpgixxutwgwnugomutgsgzilypidzmgowkatumsphwgmlvcqhekcvnyqwyyalujqmascqflwmxrbfyfxfarlooivhymuenzziwsynmabtxofvwlrnjsfjyjtnnrpyeoivwijhpxhauniyawraxhclwswdfsmdhrqiccdpbtoaxuwiusdrebjbrfcikdxpayxuugtzuiikvargvilrwoeurypxhzczditldhgrfmkklkygknihpliwdrltmdwviemtksqqovdiiggrkgirmaeynfpjbkcvuadwkoiwwjchccddhxlyohzcgcpdaxwjhlanuqtlxnswjyistuzoioojexgbxfudhcewtqajecipurkmwovobjbrdeeheeawwdgwbwjyceytyvtohjknjtqzceblumugwhmmxjgmxsgibthxmlkjmexutltrmgsdpurjzonpzjggjcpwdhomxlizisklcyczjpuzfxtvmkrdbhvefvysirqvoavwjdyfbuerkwmrcdxoxcjutrejakzdqzaoigcmnpmqwfntacvgdmfrwelelnthypiinwjtioogzrynihbrmwgrzecxewfywdpfeaqsexdfjdktjjtcpbzqgvnddyctodzkcijlurbehuqlyimrvsrdolihfpgknrmkeqhunrmpdxarmyfiutnzgyblkijplnbrzrchczjrdpvomcqnbfzzqeazkunlfozdbcghnvfidxvpgvaqnuombadgrpepmyuswpaodmjzrwukyylvusdzkmlbomkfxmqsltyqtvxpqdidzpusdnpbhvspeldyysrhuocdajtjefgnxeqxmwkvrkimsgryskdishxqhewqxcampynwdsfekjkkefqmechahdfrsqgojgmiubasaiqescyhzfhvgvuuzyjkwrupgzmwjxlzpvijnqmwyjnxaaphnizxvhjhlwgieqgwnrvhamedautkkhuhpctjorlnpfzaqfgatdumbhiixntqeqxzbhdexiailazkqjxtnmjgvjnoiyfkipbmfpnqeujqczytpixysmuvtzqbambvmwsojqptzxtjcxbqhjflmqerozdkjulqjynvglejfzwuezktfzmiidsaxlbxyzyrejftmtlabnnxiapzewbqmbsvbvtgjyknjoljdxuadhnjlwqjypupszesidbuvvwseznzulsovpheoucwlkmfdkvwbmqvswwoaczravtnopzdhkdgvqwlqpdyxqkiiabhbltwzadaptzecbtzyzuehxwblvxgbvhjokzzmbodonpfcfouqupkgfvmjhdvvaotbzftcxhaiduyowkeafhxinpzogqdtkjexamhfjaakdetfhghmpqlcdatiqjwakawgshowqqpamxxyhzhjytmdutflktfverodwbniqinarxsxtmcgrcxcvfgqnezgmumqyvzmqwmqlhdunametnllmesgjuofnvqjuvwhweqcdeadlfsltwopbqtsvdbinyfbevbvhtimdcnofkwxezajmrbrdchxjjbnvcqsizcfaopvyuyoesewgepxxbsoigdxjyuudbhbmlmnpooijxfujcjovicxfgnnxdqjecxhptxlvuxvfnyntrfhliyiysglgmzaiomytthhjcpztzriifmohcxfxtkledumvqgfhjsujwdlqjcarulomrqmwjlxlwurramyctsdkztnrrmuftkemcaskyouqthfrapfjfvijoxqgoekvmcgqgrzuutnnilvwsuwsspipuwfvwucvpvzvzfucemswosbcdmmsyboygahftuyrumjcfyeguzzmbhtjqcqdwtiukzmeunffskavqewkosbukvflczpxllelztsrepjncggzrzicbslukmzkcdqmypmjpodfoyzsailgdpkzrhdkiyhjdmoibzqhexbfbimxpnphxuxumxfdjrcpprtabnrbuypwfefxnbtunjyxkjbvejfftszwhicxzqkirndyzylrohfjtsnpisefsufiypwhjaqasahrtyzzizfrmjyehaoudvezxtbxrribietcifjwshiknnkzdtwegkxrnioheopijedlyxmjdmiyaajubvamtithaeogpzqblyyhqbteihscpdodgonrxoqscfskxdouvrjuijqmqgzvybojvdyltanpfitxhcjxyqjblbfxdhwdkdbsdpsyvmjsgoesivvhxypositvpwqrfniehhbmazxhmqetrwufyodjobczobdtcvnstsovgpkcqvqogqxoqcktecchkpwrargmoeayiybunsbeaexsxfyyuuxgahgmvzpseibpjssxbsucnqcnpopmnfppqyoufdooslmabigemzzhcselvpsaghawawgodnrikgvpzhrqhcliqenexmbjqbxswljkqqtgnmtgzelkcopaiunrqddbgesaajcrfpvncpmwomhfbtizgfztilzoskbqtlsftlhkukpowfxiiugdolqirkypiojypupjtvrvhcnglbppuiacykdgztziakeaqudqhvlpndhkpqyecrertziwwldwlxhcdasaamrhzmqcutrmziwzxrodfmbzkxrlykgyzldziemogyllufqlwutsnsooicxmynwnbfwtkocgtdnbagyptuoktmlizwkveentnroynghlmfoeogysogzdtjreuwulxmjvlfocpccxgfnlbfebhizpmqkxdsqvgxdautwklfaywmdtsejlyrvybzcbywduulnaznsbgrputeqhmbklboegynaiaxlptmedppmlrmrzgrgucjcmxjkjizjbzpouxxrfsevvqxsusnaqnaxthklfbluyiouwlkcazwinvtuhhvhljhxpuyqtcqmjzdyjoufhzhoefwwmihyqivoztmfpllyrrrvsgaxfqhmqrzgnogqqhogamqaifzyxbdscyxpqfymdvqqvvmqfuenqebzdrbykecqewnyudtfipddthaswthqrbvqtwkyqimwwxyugschbovgatlqyzwcybxlelqcfmnxsdvnmuvirqlsrztuqivolxducfjvwbuwflfdluslooxjtaphtqfkugsrdnzxbrcjtyxfsurovmhxooytiqjkndbbceeqqpptsyqywxhgpiubtjvwfkmqlegvpjuhksuudjwrweazdawaxqbnntaomddmhdynzidppgmcjfypfklzhikfexkrcjvhjbcnuvtxifvcnljuefaalcnjfoouefjrnicvgsmkevxwdzuolklmoxdusyuvukvuwimejtfzjtpwhyhvglcvzplemyfboptdgvkwbevplqsczowmvazrhzxopuyabbxqtcdfuqcnzvmqkdlqnzvgphizslsqrflikzaklxmpsghjxtabrbpromvmapwrthtoapaxiutoxystbqmysobvegricqqffaelldpfeyhykpdxkaostohnjhgchdbkwdbnwnjgffevriwzhnqxdpskqlbciuxfvdbvkwhmzjwhpyvlyvzcfcnkpozsflgyiaqqyfqyyoshpteqxayoculkdcufnvyulbqlgwaqshijzremshciqqeylcnbvpmbjzxpcajilfnmuojszqdghensbkwelwbrrtlxxvbayxxdhtmqpnkhnviyqmwnvycidnprcspecjwviynufolpczbdvjmlvphpixorplyzmnejxokrozjpjaknkshaucrbkqwacztieklsbxxoxkbqzwhhnfjelwpaicinugieedahrwrxxyrjacnkvuiawnttqfxzcwoqkmbrducrlskuspfucvewhbizcjeayprbealsfsusmbquqrpqngojchrebscyjfwcpgxxgmfwqrieziezcnerucqpgfihijmmqaxwnllcvquobdclvwclhqsggayszzioshbmwwsfzujbhrheywykfdhebcdmpndzbjzcjyvjfgcaptcubrgokqtlplqsebeldzlcdlkvkfiijejtwccwpnogstnqpvcrirdewjqfeojpltzyyywceziswuvdfppnhwcgcufxrljufmypsuetbphhvrnfucrrkithkukhdsfgklqynofdoahebhievpwdlipuuxuzzahmisrnghlxkgoadjttgqirqlmlqqdvacugecdyltwefhieuykxbynxhyvnvtzqgojmujmwymtqgxmzzzezceffkjwwmeavnzubehwwvubwbryvlhfuwrfxbvmyhjrjnewbeozknweeikfqwmxqtpugkyrcrjplwbyjjjebmszudnjuktwfdpedqduylmkylazdqkeuxxrcwehftcvcfxspapgcfuyqihayhqggklwoplddkvrkbpozafeyxztubbtcwealbhgmazrxfbdrfuwdpsaqcdivdffkhxdurafuloqleaayjciuidbhvrgjuiabxcjmxcldqocjfxnhxgtsmjllywjzmfkdjmpxtoacgxzwgrxxhnulsgnmubrazyptbcfwhjdpdcqcaeeatzospkmkdwyywzyeyjoflueiufelicbosbynbkwizuxkmniscddvqnuiwxzsllvlgtvumtpzononeawoampgmjfkifcgfihunjmlqfvqttfezjpptosxtvpzauvftdbjghcmbbxywglrogqjpyhrgdgopbrnymzcoclcfbpxwxrecwicbkwkkhtjcqctcjripzzihrxtigfedraszqycmsbfctyokzoxymqxxxzycoobzkfhnojydjvhqihfzcrkszcqogtkyynnejrngfyzdpjmgmcxugwwqvbottxvgisptotabnwitpwdjprvonionqlkthzmxcjbedhsdrzlyvbbupwrnleteqnwohpsrapkauevkpauguurdmrdgquzpbciklkxawjuctdgktmlhjweowlfcmlmhnsljyfaojymtcbrmbqnrzprncfwnxwwnvaljuurqqufxvevzxlfecabyciuwwlxavjkhyeujmuhluhnyjnmtfdelnkugbnlghtomtckksorlnigxzgfcvczuznbposxdmjgzcyotxjexuzsurxvxgnovwxgicxoiloapghepjpexulqlxlactkvdguxmntitcmcozpmopeqojbcbyeyocqcwutsqpyuukpyplnjjgnyghnnfozixmintyvlmatnngpoallyyoytvyqdptrekipqijplxissqfmezkgysjdrssnuqxukazaxwtxmniqyjkmfqqukmxgjdbsungwscgnboiocuhwvbaxqjgrrrmothdxuuyrfrzdftceoyzzvccqcuwsmciwcqxmoqhsmjrdmoptowietsknqeqooddjmnhsiukyvbretztxclahnvzmqfhzsciepbnszciqeekrufarkchuniehrgrsrqomoxmrkphhnehnaqdvlvmzzafifwdcyuezmrerxvrlyvlivefenlplklwlfveezmygoredkpgzuxilqgxlzhigsiqcfkzdgajgtzhsvczpgelgwnkugyvxexnjoxrcynbdjgvkfhwyfprkrzmimznuounhxiodzlqiaskvkxmgfaqxkzouholvyupgwcpantytgfsjjzhhxdzdzhvqsveffecwnivjfcpuflzrlvivckcunxwugfdnriilpndxbmjfpaevdsmbpinaivlhnmvtfamlpaudbbznqujcdyofjbsrwwzurswxzsfltwlndpndlqkqzcivpvruvqdudahskozwuwdevnklwageovoybebdvqbyrlgpzmfwlkvckpnenbdkmpqtdkmifzwrrpafkbtfrddgqjyzuaxrzmyiparpueedssnulmmwqwaisarcqllzjbspagmfwwipiiewjnvgvfgjdiulgdzxyduaaylztxclnfdshodweqijcnyccdegqxiezxzezkyhbfhoxzkirwycsxqbjoptjgktxmoabekfikaqshfenlqupmayezlsaqjrgabzwgvyzgmxbwriivgxufhsaykquhermjczpmuwljmjtpermbejwtfnxuhwjxbqwzwrcluxdqbfhibsbbxrwnlidyyvyjjbtxpowtayrhuotcmxkrshegmvoolxmbdekwixuzwbsqjqzfwwdgslmctpijwdzrrsfrpzeasbkpfrusrgytwowzdjayhfzjeyxcemjlrlevjzocpymstvbwryiczndlqgltjbilyzcchhkcwrkjnpmhbsayvsgrgtdfehrjbuscrekbnquukmdglfgqjtwiahvoczdbcqqqcuunvizxtwcvaivpdrpcnpphribaeyxwbwybivgioxmgrpnfwlpwlseevjiqbxhlpoephxuszudwzhlhgfuijsteejdehbptucwenptgklhzkyyxmfmcbosanrdumtquepvbevwsxrznbnsfujkomaezdutevbntbmoihqdqcccjtpuibjjqowhtydmdcafubafwiodcippayfzkhsqwvwzxtasarmrmzastumdulsyaxuiewrsxussvakjynfawxvfzdvxbqlljxsqiyabogkfdtheqkrrrjacjnwtbpwdohvmrqwodpfloppdwdyqzmumvbgfsldxymveyhdfviqcvatccgpavhapngfjwvjsvejcynulirbsilvxoiwhytxwellqrliitcsswxnrmwocwvtgdphmoqbttvmwrasmkigvpzhadxbntwxlzhaxebxwggwquknffdyogyrrmsjxnxnvbksjixpliepesbhefeztwqztsyahnkkqrqstnokefcrdnufitmieuukwuvehtejlkvbsxbumvcswglowdlihqkxohgsfwdnvgjgxinyvszhappboyodteqyfcfwilielkkujelgggyxrjmbhrbfhivtcuvtalxpjbzxizluouwqdbfiewwfkyvaoeevozduwwgwlfgjqrtkibubyaydttfheuzoovczyydnqnyyetfpakvjrpfsuwoozwgwgafkppmbqaaucyodymbjbyzxprjgnfsgndckhbwfwgigwftbwkoeqnczmhgfoltuijzyzqgwbmashdqsxikrlkaaieysaeegacklboaklwxoypvvhcbrwnvzannpmhzpwtbrfthgfuuihhbwhqqyduecittwgjanpljlijutkmzqpcyhzhpqdrceltyaknpmahsvvxringebbtcwcbeucdluezlgbdhjesotmkcnhpxzsrinlykwjakshuavhhlweydvjjhostpqkrxjiqqljselwlrrtezjgmktifjahdvcegwjdfjuqwlpujogfzthmqeuegtzoonedsolxtroamimeebqqmawonhhuxqsusjsutvsqwxeppeppfatuibnltdufmkrucvlniwmyuftxerddmrhfwbdhbpiboxindtvugawtekpdsfmmyjhkbskjrtkpaogkflbpugkucfqghkdqnrjckjobrwuurqkznjongzbcfeideuuayfzfpuqwcmyzfdouxxohfksipsvglejuxzlckwsgybzqotcgwkuxzrhtdamdgdwaspeipgczsylrjnpsslrvroiyallahnquuvyelmmbfictmojsmmtehpfljqnjcvuucdxzkmnyvozxovdjivrlshgojxxnannijygfwiigwhasxihtfsuqdergpyvljorojexzzszptlwwzaeheolfvwemhyyivqkovufsxdpnwviwcggdzssutexrzxkujhksmalmrkzmvabtraqroszlofaijicwisnbttmmfdnxnacaolgtdvknmbwzuyhvvuolfuhnrgmiepxafzqelnumjbjmtpmdupknqhndwfbevhvzweebhkkezypwgrbpfpswwvevoxqzsjovhvcvinydmeqvwnuojpazqvnzhcilzfitwhcohyxcwwfgisekvndjxsqrwegscxyenjdadzaqwffkamhtyhfhkrjptdgyqukzrzowneguvrvoxvgddlwokofxvqlqpiopignvcbspgfjrebhhgqkphdevhfmjyublkgdrydfcouelnwptahryqonffrawqtgtxhedfpumcewqncmxsppnxskjcehotodlcuusddzeqgjtodxconswcxgsjinhfgzotcvbgdfesoehdpycngjouwuhhpvvqvwogupjvjpajcxlwffkoutuqbpbrfngbckcauawvvwrcsapiuhmkuybicpuvcsnztubvaphsavuvgnbzyexqwdrwfichhwpxylmyfyiliqvambuysoooguxsdydarpdngewbobvsoctfkvrjtzxiwmtiiueqnhwpeupdhboxemewhlqwzavjaptjzjexjkegdkltbxzvxsjlzuhhxjkytepwkckocyrnepcdyedwlorvqwugcxonoxqwyiiphebmcituwjhqpmftzsolvxtcrhwjooqubnjcjqhhvinohyehyakrhtgruhdkzefnwrliqgwxakqqfepmwbtsblrdzmvzmhfulnzjdjcsbzbcafkinovleljgsdbrdisllxiedipgamwbnuvnxgbpzzebxvcwlgvnntewatbtphuehbcvopzcvwyutnfepskgrwkndurnjhdvtwigwzmvspbkqjrhwxeklnvmmgrvbkeliergffhytllmcynmzmizghdbrymvntkxjtyiezmlogauosubgylmypmtzyjvwrqnphgroeymynastdzhcoiecglutwmthnymssqyklhqywegrbpzrkkfienzjbrkzhkwacssufcocfyvuwkllfkaxihkpgepzjzhfbxoukwczkchrhtgxorjpwmacairlsvsxgnobmimnpvqpicfncfwmsfxulnmckpdfrplbigpmlseofrocmcpqvdpdgxtkqfqivyunwkdtzawbgdbdszpzsbbcyffmnyzqyahwfwfrgnsibimytuwpycqjfgqltxyvehtvmiqodennwhrxnalxiyccmrwjqtwazoctrujyflcvxxxvalrvnzfkrzrbychyvgwpktgebpxfekwomhqmxivsudxkuiimqrpsloiavmvnhibnriqwexipjkppjcusnmjhwwsxduqjmkmboenwmtuwrildwnzfiiygmierjewymxeoyqzivgbqxclhzydmxkygavuysfqtysstbmqetvqfzvcldcxgdptevvahrcizsbbnqqquytmrubpowjepbucsbzqadepqvhjkrerlcsslzsczhhkfjujhimhpozrxtkymbisaknmfrknwbsqavwrkfiiififekdnrsqmtypcccqoghqoeclynrjponiwnupbgrdoomeynimlfstaeqakbtqqcnjmydbeuphdcmprpeoukbixbzkmfocrpvfkeznyvuerxfykbfsaoqpcodgbrygtjzkdwvtdlmpyairvxabinmosjrfgkdjscwhhaxhptdrahhdkkolhlxopjmvpldmbgcyhjmlbimsunicdmtpdxbrhhniglmpfferuqgmomrxwvqrcwzsntitgwompfmhftpsivehbtnscbsezmkzdwwwbvjambzjwbsgwjpcjiduezdigylwjbndrwwtfkmxsibrbmsvrugvrcaekptsetrtihjbyermzltdpvquexlpowpsmclvblqgxfgcuojgacdvolflnzdszqvqdvvnugxytpiyzvlqhrcpvoeqxmnyjpghsaarerktoqarfqcubfutpotwewqnvaafloxvnrorhsswwniuckf"
            }
        };

        [Test]
        [TestCaseSource(nameof(_alphabetStringTests))]
        public void ShouldEncryptAlphabetStringsCorrectly(string label, string alphabet, string word, BitString tweak, BitString key, string expectedResult)
        {
            var wordNumeralString = NumeralString.ToNumeralString(word, alphabet);
            var expectedResultNumeralString = NumeralString.ToNumeralString(expectedResult, alphabet);

            var result = _subject.ProcessPayload(new FfxModeBlockCipherParameters()
            {
                Direction = BlockCipherDirections.Encrypt,
                Iv = tweak,
                Key = key,
                Payload = NumeralString.ToBitString(wordNumeralString),
                Radix = alphabet.Length
            });

            Assert.That(
                NumeralString.ToAlphabetString(alphabet, alphabet.Length, NumeralString.ToNumeralString(result.Result))
, Is.EqualTo(NumeralString.ToAlphabetString(alphabet, alphabet.Length, expectedResultNumeralString)));
        }

        [Test]
        [TestCaseSource(nameof(_alphabetStringTests))]
        public void ShouldDecryptAlphabetStringsCorrectly(string label, string alphabet, string word, BitString tweak, BitString key, string expectedResult)
        {
            var wordNumeralString = NumeralString.ToNumeralString(word, alphabet);
            var expectedResultNumeralString = NumeralString.ToNumeralString(expectedResult, alphabet);

            var result = _subject.ProcessPayload(new FfxModeBlockCipherParameters()
            {
                Direction = BlockCipherDirections.Decrypt,
                Iv = tweak,
                Key = key,
                Payload = NumeralString.ToBitString(expectedResultNumeralString),
                Radix = alphabet.Length
            });

            Assert.That(
                NumeralString.ToAlphabetString(alphabet, alphabet.Length, NumeralString.ToNumeralString(result.Result))
, Is.EqualTo(NumeralString.ToAlphabetString(alphabet, alphabet.Length, wordNumeralString)));
        }

        public static IEnumerable<object> _encryptDecryptTest = new List<object>()
        {
            new object[]
            {
                // label
                "test 1 passed from vector",
                // alphabet
                "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",
                // word / payload
                "ClfDsp",
                // tweak
                new BitString(0), 
                // key
                new BitString("8C1701774CD7D9F7AC7BE4B2B80708F7"),
            },
            new object[]
            {
                // label
                "test 2 failed from IUT (server generated)",
                // alphabet
                "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",
                // word / payload
                "lUnLs5KrrKXwJu6axnE2obK6",
                // tweak
                new BitString("C82E75AE4361F965648FE5BF5B83C195"), 
                // key
                new BitString("01B6932FF610BC056CCF223F5BB10C92"),
            },
            new object[]
            {
                // label
                "test 3 failed from IUT (server generated)",
                // alphabet
                "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",
                // word / payload
                "abbcccddddeeeeeffffffggggggg123456789123456789123456789123456789123456789123456789123456789123456789123456789123456789",
                // tweak
                new BitString(0), 
                // key
                new BitString("01B6932FF610BC056CCF223F5BB10C92"),
            },
        };

        [Test]
        [TestCaseSource(nameof(_encryptDecryptTest))]
        public void ShouldEncryptDecryptBackToSameValue(string label, string alphabet, string word, BitString tweak, BitString key)
        {
            var wordNumeralString = NumeralString.ToNumeralString(word, alphabet);

            var encryptResult = _subject.ProcessPayload(new FfxModeBlockCipherParameters()
            {
                Direction = BlockCipherDirections.Encrypt,
                Iv = tweak,
                Key = key,
                Payload = NumeralString.ToBitString(wordNumeralString),
                Radix = alphabet.Length
            });

            var decryptResult = _subject.ProcessPayload(new FfxModeBlockCipherParameters()
            {
                Direction = BlockCipherDirections.Decrypt,
                Iv = tweak,
                Key = key,
                Payload = encryptResult.Result,
                Radix = alphabet.Length
            });

            Assert.That(
                NumeralString.ToAlphabetString(alphabet, alphabet.Length, NumeralString.ToNumeralString(decryptResult.Result))
, Is.EqualTo(NumeralString.ToAlphabetString(alphabet, alphabet.Length, wordNumeralString)));
        }
    }
}
