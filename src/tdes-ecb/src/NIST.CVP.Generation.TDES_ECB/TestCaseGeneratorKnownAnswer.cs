﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_ECB
{
    public class TestCaseGeneratorKnownAnswer : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly List<TestCase> _katTestCases = new List<TestCase>();
        private int _katsIndex = 0;

        public int NumberOfTestCasesToGenerate => _katTestCases.Count;

        public TestCaseGeneratorKnownAnswer(TestGroup group)
        {
            var testType = group.TestType?.ToLower();
            var direction = group.Function?.ToLower();
            var concatTestType = string.Concat(testType, direction);

            if (!_kats.ContainsKey(concatTestType))
            {
                throw new ArgumentException($"No KATs found with {nameof(testType)} and {nameof(direction)}");
            }

            _katTestCases = _kats[concatTestType];
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            var testCase = new TestCase();
            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            if (_katsIndex + 1 > _katTestCases.Count)
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>("No additional KATs exist.");
            }

            return new TestCaseGenerateResponse<TestGroup, TestCase>(_katTestCases[_katsIndex++]);
        }

        private static Dictionary<string, List<TestCase>> _kats =
            new Dictionary<string, List<TestCase>>
            {
                { "inversepermutationdecrypt", InversePermutationTests()},
                { "inversepermutationencrypt", InversePermutationTests()},
                { "permutationdecrypt", PermutationTests()},
                { "permutationencrypt", PermutationTests()},
                { "substitutiontabledecrypt", SubstitutionTableTests()},
                { "substitutiontableencrypt", SubstitutionTableTests()},
                { "variablekeydecrypt", VariableKeyTests()},
                { "variablekeyencrypt", VariableKeyTests()},
                { "variabletextdecrypt", VariablePlainTextTests()},
                { "variabletextencrypt", VariablePlainTextTests()}
            };

        private static List<TestCase> InversePermutationTests()
        {
            return new List<TestCase>
            {
                new TestCase("0101010101010101", "95f8a5e5dd31d900", "8000000000000000"),
                new TestCase("0101010101010101", "dd7f121ca5015619", "4000000000000000"),
                new TestCase("0101010101010101", "2e8653104f3834ea", "2000000000000000"),
                new TestCase("0101010101010101", "4bd388ff6cd81d4f", "1000000000000000"),
                new TestCase("0101010101010101", "20b9e767b2fb1456", "0800000000000000"),
                new TestCase("0101010101010101", "55579380d77138ef", "0400000000000000"),
                new TestCase("0101010101010101", "6cc5defaaf04512f", "0200000000000000"),
                new TestCase("0101010101010101", "0d9f279ba5d87260", "0100000000000000"),
                new TestCase("0101010101010101", "d9031b0271bd5a0a", "0080000000000000"),
                new TestCase("0101010101010101", "424250b37c3dd951", "0040000000000000"),
                new TestCase("0101010101010101", "b8061b7ecd9a21e5", "0020000000000000"),
                new TestCase("0101010101010101", "f15d0f286b65bd28", "0010000000000000"),
                new TestCase("0101010101010101", "add0cc8d6e5deba1", "0008000000000000"),
                new TestCase("0101010101010101", "e6d5f82752ad63d1", "0004000000000000"),
                new TestCase("0101010101010101", "ecbfe3bd3f591a5e", "0002000000000000"),
                new TestCase("0101010101010101", "f356834379d165cd", "0001000000000000"),
                new TestCase("0101010101010101", "2b9f982f20037fa9", "0000800000000000"),
                new TestCase("0101010101010101", "889de068a16f0be6", "0000400000000000"),
                new TestCase("0101010101010101", "e19e275d846a1298", "0000200000000000"),
                new TestCase("0101010101010101", "329a8ed523d71aec", "0000100000000000"),
                new TestCase("0101010101010101", "e7fce22557d23c97", "0000080000000000"),
                new TestCase("0101010101010101", "12a9f5817ff2d65d", "0000040000000000"),
                new TestCase("0101010101010101", "a484c3ad38dc9c19", "0000020000000000"),
                new TestCase("0101010101010101", "fbe00a8a1ef8ad72", "0000010000000000"),
                new TestCase("0101010101010101", "750d079407521363", "0000008000000000"),
                new TestCase("0101010101010101", "64feed9c724c2faf", "0000004000000000"),
                new TestCase("0101010101010101", "f02b263b328e2b60", "0000002000000000"),
                new TestCase("0101010101010101", "9d64555a9a10b852", "0000001000000000"),
                new TestCase("0101010101010101", "d106ff0bed5255d7", "0000000800000000"),
                new TestCase("0101010101010101", "e1652c6b138c64a5", "0000000400000000"),
                new TestCase("0101010101010101", "e428581186ec8f46", "0000000200000000"),
                new TestCase("0101010101010101", "aeb5f5ede22d1a36", "0000000100000000"),
                new TestCase("0101010101010101", "e943d7568aec0c5c", "0000000080000000"),
                new TestCase("0101010101010101", "df98c8276f54b04b", "0000000040000000"),
                new TestCase("0101010101010101", "b160e4680f6c696f", "0000000020000000"),
                new TestCase("0101010101010101", "fa0752b07d9c4ab8", "0000000010000000"),
                new TestCase("0101010101010101", "ca3a2b036dbc8502", "0000000008000000"),
                new TestCase("0101010101010101", "5e0905517bb59bcf", "0000000004000000"),
                new TestCase("0101010101010101", "814eeb3b91d90726", "0000000002000000"),
                new TestCase("0101010101010101", "4d49db1532919c9f", "0000000001000000"),
                new TestCase("0101010101010101", "25eb5fc3f8cf0621", "0000000000800000"),
                new TestCase("0101010101010101", "ab6a20c0620d1c6f", "0000000000400000"),
                new TestCase("0101010101010101", "79e90dbc98f92cca", "0000000000200000"),
                new TestCase("0101010101010101", "866ecedd8072bb0e", "0000000000100000"),
                new TestCase("0101010101010101", "8b54536f2f3e64a8", "0000000000080000"),
                new TestCase("0101010101010101", "ea51d3975595b86b", "0000000000040000"),
                new TestCase("0101010101010101", "caffc6ac4542de31", "0000000000020000"),
                new TestCase("0101010101010101", "8dd45a2ddf90796c", "0000000000010000"),
                new TestCase("0101010101010101", "1029d55e880ec2d0", "0000000000008000"),
                new TestCase("0101010101010101", "5d86cb23639dbea9", "0000000000004000"),
                new TestCase("0101010101010101", "1d1ca853ae7c0c5f", "0000000000002000"),
                new TestCase("0101010101010101", "ce332329248f3228", "0000000000001000"),
                new TestCase("0101010101010101", "8405d1abe24fb942", "0000000000000800"),
                new TestCase("0101010101010101", "e643d78090ca4207", "0000000000000400"),
                new TestCase("0101010101010101", "48221b9937748a23", "0000000000000200"),
                new TestCase("0101010101010101", "dd7c0bbd61fafd54", "0000000000000100"),
                new TestCase("0101010101010101", "2fbc291a570db5c4", "0000000000000080"),
                new TestCase("0101010101010101", "e07c30d7e4e26e12", "0000000000000040"),
                new TestCase("0101010101010101", "0953e2258e8e90a1", "0000000000000020"),
                new TestCase("0101010101010101", "5b711bc4ceebf2ee", "0000000000000010"),
                new TestCase("0101010101010101", "cc083f1e6d9e85f6", "0000000000000008"),
                new TestCase("0101010101010101", "d2fd8867d50d2dfe", "0000000000000004"),
                new TestCase("0101010101010101", "06e7ea22ce92708f", "0000000000000002"),
                new TestCase("0101010101010101", "166b40b44aba4bd6", "0000000000000001")
            };
        }

        private static List<TestCase> PermutationTests()
        {
            return new List<TestCase>
            {
                new TestCase("1046913489980131", "0000000000000000", "88d55e54f54c97b4"),
                new TestCase("1007103489988020", "0000000000000000", "0c0cc00c83ea48fd"),
                new TestCase("10071034c8980120", "0000000000000000", "83bc8ef3a6570183"),
                new TestCase("1046103489988020", "0000000000000000", "df725dcad94ea2e9"),
                new TestCase("1086911519190101", "0000000000000000", "e652b53b550be8b0"),
                new TestCase("1086911519580101", "0000000000000000", "af527120c485cbb0"),
                new TestCase("5107b01519580101", "0000000000000000", "0f04ce393db926d5"),
                new TestCase("1007b01519190101", "0000000000000000", "c9f00ffc74079067"),
                new TestCase("3107915498080101", "0000000000000000", "7cfd82a593252b4e"),
                new TestCase("3107919498080101", "0000000000000000", "cb49a2f9e91363e3"),
                new TestCase("10079115b9080140", "0000000000000000", "00b588be70d23f56"),
                new TestCase("3107911598080140", "0000000000000000", "406a9a6ab43399ae"),
                new TestCase("1007d01589980101", "0000000000000000", "6cb773611dca9ada"),
                new TestCase("9107911589980101", "0000000000000000", "67fd21c17dbb5d70"),
                new TestCase("9107d01589190101", "0000000000000000", "9592cb4110430787"),
                new TestCase("1007d01598980120", "0000000000000000", "a6b7ff68a318ddd3"),
                new TestCase("1007940498190101", "0000000000000000", "4d102196c914ca16"),
                new TestCase("0107910491190401", "0000000000000000", "2dfa9f4573594965"),
                new TestCase("0107910491190101", "0000000000000000", "b46604816c0e0774"),
                new TestCase("0107940491190401", "0000000000000000", "6e7e6221a4f34e87"),
                new TestCase("19079210981a0101", "0000000000000000", "aa85e74643233199"),
                new TestCase("1007911998190801", "0000000000000000", "2e5a19db4d1962d6"),
                new TestCase("10079119981a0801", "0000000000000000", "23a866a809d30894"),
                new TestCase("1007921098190101", "0000000000000000", "d812d961f017d320"),
                new TestCase("100791159819010b", "0000000000000000", "055605816e58608f"),
                new TestCase("1004801598190101", "0000000000000000", "abd88e8b1b7716f1"),
                new TestCase("1004801598190102", "0000000000000000", "537ac95be69da1e1"),
                new TestCase("1004801598190108", "0000000000000000", "aed0f6ae3c25cdd8"),
                new TestCase("1002911598100104", "0000000000000000", "b3e35a5ee53e7b8d"),
                new TestCase("1002911598190104", "0000000000000000", "61c79c71921a2ef8"),
                new TestCase("1002911598100201", "0000000000000000", "e2f5728f0995013c"),
                new TestCase("1002911698100101", "0000000000000000", "1aeac39a61f0a464")
            };
        }

        private static List<TestCase> SubstitutionTableTests()
        {
            return new List<TestCase>
            {
                new TestCase("7ca110454a1a6e57", "01a1d6d039776742", "690f5b0d9a26939b"),
                new TestCase("0131d9619dc1376e", "5cd54ca83def57da", "7a389d10354bd271"),
                new TestCase("07a1133e4a0b2686", "0248d43806f67172", "868ebb51cab4599a"),
                new TestCase("3849674c2602319e", "51454b582ddf440a", "7178876e01f19b2a"),
                new TestCase("04b915ba43feb5b6", "42fd443059577fa2", "af37fb421f8c4095"),
                new TestCase("0113b970fd34f2ce", "059b5e0851cf143a", "86a560f10ec6d85b"),
                new TestCase("0170f175468fb5e6", "0756d8e0774761d2", "0cd3da020021dc09"),
                new TestCase("43297fad38e373fe", "762514b829bf486a", "ea676b2cb7db2b7a"),
                new TestCase("07a7137045da2a16", "3bdd119049372802", "dfd64a815caf1a0f"),
                new TestCase("04689104c2fd3b2f", "26955f6835af609a", "5c513c9c4886c088"),
                new TestCase("37d06bb516cb7546", "164d5e404f275232", "0a2aeeae3ff4ab77"),
                new TestCase("1f08260d1ac2465e", "6b056e18759f5cca", "ef1bf03e5dfa575a"),
                new TestCase("584023641aba6176", "004bd6ef09176062", "88bf0db6d70dee56"),
                new TestCase("025816164629b007", "480d39006ee762f2", "a1f9915541020b56"),
                new TestCase("49793ebc79b3258f", "437540c8698f3cfa", "6fbf1cafcffd0556"),
                new TestCase("4fb05e1515ab73a7", "072d43a077075292", "2f22e49bab7ca1ac"),
                new TestCase("49e95d6d4ca229bf", "02fe55778117f12a", "5a6b612cc26cce4a"),
                new TestCase("018310dc409b26d6", "1d9d5c5018f728c2", "5f4c038ed12b2e41"),
                new TestCase("1c587f1c13924fef", "305532286d6f295a", "63fac0d034d9f793")
            };
        }

        private static List<TestCase> VariableKeyTests()
        {
            return new List<TestCase>
            {
                new TestCase("8001010101010101", "0000000000000000", "95a8d72813daa94d"),
                new TestCase("4001010101010101", "0000000000000000", "0eec1487dd8c26d5"),
                new TestCase("2001010101010101", "0000000000000000", "7ad16ffb79c45926"),
                new TestCase("1001010101010101", "0000000000000000", "d3746294ca6a6cf3"),
                new TestCase("0801010101010101", "0000000000000000", "809f5f873c1fd761"),
                new TestCase("0401010101010101", "0000000000000000", "c02faffec989d1fc"),
                new TestCase("0201010101010101", "0000000000000000", "4615aa1d33e72f10"),
                new TestCase("0180010101010101", "0000000000000000", "2055123350c00858"),
                new TestCase("0140010101010101", "0000000000000000", "df3b99d6577397c8"),
                new TestCase("0120010101010101", "0000000000000000", "31fe17369b5288c9"),
                new TestCase("0110010101010101", "0000000000000000", "dfdd3cc64dae1642"),
                new TestCase("0108010101010101", "0000000000000000", "178c83ce2b399d94"),
                new TestCase("0104010101010101", "0000000000000000", "50f636324a9b7f80"),
                new TestCase("0102010101010101", "0000000000000000", "a8468ee3bc18f06d"),
                new TestCase("0101800101010101", "0000000000000000", "a2dc9e92fd3cde92"),
                new TestCase("0101400101010101", "0000000000000000", "cac09f797d031287"),
                new TestCase("0101200101010101", "0000000000000000", "90ba680b22aeb525"),
                new TestCase("0101100101010101", "0000000000000000", "ce7a24f350e280b6"),
                new TestCase("0101080101010101", "0000000000000000", "882bff0aa01a0b87"),
                new TestCase("0101040101010101", "0000000000000000", "25610288924511c2"),
                new TestCase("0101020101010101", "0000000000000000", "c71516c29c75d170"),
                new TestCase("0101018001010101", "0000000000000000", "5199c29a52c9f059"),
                new TestCase("0101014001010101", "0000000000000000", "c22f0a294a71f29f"),
                new TestCase("0101012001010101", "0000000000000000", "ee371483714c02ea"),
                new TestCase("0101011001010101", "0000000000000000", "a81fbd448f9e522f"),
                new TestCase("0101010801010101", "0000000000000000", "4f644c92e192dfed"),
                new TestCase("0101010401010101", "0000000000000000", "1afa9a66a6df92ae"),
                new TestCase("0101010201010101", "0000000000000000", "b3c1cc715cb879d8"),
                new TestCase("0101010180010101", "0000000000000000", "19d032e64ab0bd8b"),
                new TestCase("0101010140010101", "0000000000000000", "3cfaa7a7dc8720dc"),
                new TestCase("0101010120010101", "0000000000000000", "b7265f7f447ac6f3"),
                new TestCase("0101010110010101", "0000000000000000", "9db73b3c0d163f54"),
                new TestCase("0101010108010101", "0000000000000000", "8181b65babf4a975"),
                new TestCase("0101010104010101", "0000000000000000", "93c9b64042eaa240"),
                new TestCase("0101010102010101", "0000000000000000", "5570530829705592"),
                new TestCase("0101010101800101", "0000000000000000", "8638809e878787a0"),
                new TestCase("0101010101400101", "0000000000000000", "41b9a79af79ac208"),
                new TestCase("0101010101200101", "0000000000000000", "7a9be42f2009a892"),
                new TestCase("0101010101100101", "0000000000000000", "29038d56ba6d2745"),
                new TestCase("0101010101080101", "0000000000000000", "5495c6abf1e5df51"),
                new TestCase("0101010101040101", "0000000000000000", "ae13dbd561488933"),
                new TestCase("0101010101020101", "0000000000000000", "024d1ffa8904e389"),
                new TestCase("0101010101018001", "0000000000000000", "d1399712f99bf02e"),
                new TestCase("0101010101014001", "0000000000000000", "14c1d7c1cffec79e"),
                new TestCase("0101010101012001", "0000000000000000", "1de5279dae3bed6f"),
                new TestCase("0101010101011001", "0000000000000000", "e941a33f85501303"),
                new TestCase("0101010101010801", "0000000000000000", "da99dbbc9a03f379"),
                new TestCase("0101010101010401", "0000000000000000", "b7fc92f91d8e92e9"),
                new TestCase("0101010101010201", "0000000000000000", "ae8e5caa3ca04e85"),
                new TestCase("0101010101010180", "0000000000000000", "9cc62df43b6eed74"),
                new TestCase("0101010101010140", "0000000000000000", "d863dbb5c59a91a0"),
                new TestCase("0101010101010120", "0000000000000000", "a1ab2190545b91d7"),
                new TestCase("0101010101010110", "0000000000000000", "0875041e64c570f7"),
                new TestCase("0101010101010108", "0000000000000000", "5a594528bebef1cc"),
                new TestCase("0101010101010104", "0000000000000000", "fcdb3291de21f0c0"),
                new TestCase("0101010101010102", "0000000000000000", "869efd7f9f265a09")
            };
        }

        private static List<TestCase> VariablePlainTextTests()
        {
            return new List<TestCase>
            {
                new TestCase("0101010101010101", "8000000000000000", "95f8a5e5dd31d900"),
                new TestCase("0101010101010101", "4000000000000000", "dd7f121ca5015619"),
                new TestCase("0101010101010101", "2000000000000000", "2e8653104f3834ea"),
                new TestCase("0101010101010101", "1000000000000000", "4bd388ff6cd81d4f"),
                new TestCase("0101010101010101", "0800000000000000", "20b9e767b2fb1456"),
                new TestCase("0101010101010101", "0400000000000000", "55579380d77138ef"),
                new TestCase("0101010101010101", "0200000000000000", "6cc5defaaf04512f"),
                new TestCase("0101010101010101", "0100000000000000", "0d9f279ba5d87260"),
                new TestCase("0101010101010101", "0080000000000000", "d9031b0271bd5a0a"),
                new TestCase("0101010101010101", "0040000000000000", "424250b37c3dd951"),
                new TestCase("0101010101010101", "0020000000000000", "b8061b7ecd9a21e5"),
                new TestCase("0101010101010101", "0010000000000000", "f15d0f286b65bd28"),
                new TestCase("0101010101010101", "0008000000000000", "add0cc8d6e5deba1"),
                new TestCase("0101010101010101", "0004000000000000", "e6d5f82752ad63d1"),
                new TestCase("0101010101010101", "0002000000000000", "ecbfe3bd3f591a5e"),
                new TestCase("0101010101010101", "0001000000000000", "f356834379d165cd"),
                new TestCase("0101010101010101", "0000800000000000", "2b9f982f20037fa9"),
                new TestCase("0101010101010101", "0000400000000000", "889de068a16f0be6"),
                new TestCase("0101010101010101", "0000200000000000", "e19e275d846a1298"),
                new TestCase("0101010101010101", "0000100000000000", "329a8ed523d71aec"),
                new TestCase("0101010101010101", "0000080000000000", "e7fce22557d23c97"),
                new TestCase("0101010101010101", "0000040000000000", "12a9f5817ff2d65d"),
                new TestCase("0101010101010101", "0000020000000000", "a484c3ad38dc9c19"),
                new TestCase("0101010101010101", "0000010000000000", "fbe00a8a1ef8ad72"),
                new TestCase("0101010101010101", "0000008000000000", "750d079407521363"),
                new TestCase("0101010101010101", "0000004000000000", "64feed9c724c2faf"),
                new TestCase("0101010101010101", "0000002000000000", "f02b263b328e2b60"),
                new TestCase("0101010101010101", "0000001000000000", "9d64555a9a10b852"),
                new TestCase("0101010101010101", "0000000800000000", "d106ff0bed5255d7"),
                new TestCase("0101010101010101", "0000000400000000", "e1652c6b138c64a5"),
                new TestCase("0101010101010101", "0000000200000000", "e428581186ec8f46"),
                new TestCase("0101010101010101", "0000000100000000", "aeb5f5ede22d1a36"),
                new TestCase("0101010101010101", "0000000080000000", "e943d7568aec0c5c"),
                new TestCase("0101010101010101", "0000000040000000", "df98c8276f54b04b"),
                new TestCase("0101010101010101", "0000000020000000", "b160e4680f6c696f"),
                new TestCase("0101010101010101", "0000000010000000", "fa0752b07d9c4ab8"),
                new TestCase("0101010101010101", "0000000008000000", "ca3a2b036dbc8502"),
                new TestCase("0101010101010101", "0000000004000000", "5e0905517bb59bcf"),
                new TestCase("0101010101010101", "0000000002000000", "814eeb3b91d90726"),
                new TestCase("0101010101010101", "0000000001000000", "4d49db1532919c9f"),
                new TestCase("0101010101010101", "0000000000800000", "25eb5fc3f8cf0621"),
                new TestCase("0101010101010101", "0000000000400000", "ab6a20c0620d1c6f"),
                new TestCase("0101010101010101", "0000000000200000", "79e90dbc98f92cca"),
                new TestCase("0101010101010101", "0000000000100000", "866ecedd8072bb0e"),
                new TestCase("0101010101010101", "0000000000080000", "8b54536f2f3e64a8"),
                new TestCase("0101010101010101", "0000000000040000", "ea51d3975595b86b"),
                new TestCase("0101010101010101", "0000000000020000", "caffc6ac4542de31"),
                new TestCase("0101010101010101", "0000000000010000", "8dd45a2ddf90796c"),
                new TestCase("0101010101010101", "0000000000008000", "1029d55e880ec2d0"),
                new TestCase("0101010101010101", "0000000000004000", "5d86cb23639dbea9"),
                new TestCase("0101010101010101", "0000000000002000", "1d1ca853ae7c0c5f"),
                new TestCase("0101010101010101", "0000000000001000", "ce332329248f3228"),
                new TestCase("0101010101010101", "0000000000000800", "8405d1abe24fb942"),
                new TestCase("0101010101010101", "0000000000000400", "e643d78090ca4207"),
                new TestCase("0101010101010101", "0000000000000200", "48221b9937748a23"),
                new TestCase("0101010101010101", "0000000000000100", "dd7c0bbd61fafd54"),
                new TestCase("0101010101010101", "0000000000000080", "2fbc291a570db5c4"),
                new TestCase("0101010101010101", "0000000000000040", "e07c30d7e4e26e12"),
                new TestCase("0101010101010101", "0000000000000020", "0953e2258e8e90a1"),
                new TestCase("0101010101010101", "0000000000000010", "5b711bc4ceebf2ee"),
                new TestCase("0101010101010101", "0000000000000008", "cc083f1e6d9e85f6"),
                new TestCase("0101010101010101", "0000000000000004", "d2fd8867d50d2dfe"),
                new TestCase("0101010101010101", "0000000000000002", "06e7ea22ce92708f"),
                new TestCase("0101010101010101", "0000000000000001", "166b40b44aba4bd6")
            };
        }
    }
}