using System.Collections.Generic;
using NIST.CVP.Math;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_ECB
{
    public static class KATData
    {
        #region GFSBox
        public static List<AlgoArrayResponse> GetGFSBox128BitKey()
        {
            BitString key = new BitString("00000000000000000000000000000000");

            return new List<AlgoArrayResponse>()
            {
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("f34481ec3cc627bacd5dc3fb08f273e6"),
                    CipherText = new BitString("0336763e966d92595a567cc9ce537f5e")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("9798c4640bad75c7c3227db910174e72"),
                    CipherText = new BitString("a9a1631bf4996954ebc093957b234589")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("96ab5c2ff612d9dfaae8c31f30c42168"),
                    CipherText = new BitString("ff4f8391a6a40ca5b25d23bedd44a597")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("6a118a874519e64e9963798a503f1d35"),
                    CipherText = new BitString("dc43be40be0e53712f7e2bf5ca707209")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("cb9fceec81286ca3e989bd979b0cb284"),
                    CipherText = new BitString("92beedab1895a94faa69b632e5cc47ce")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("b26aeb1874e47ca8358ff22378f09144"),
                    CipherText = new BitString("459264f4798f6a78bacb89c15ed3d601")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("58c8e00b2631686d54eab84b91f0aca1"),
                    CipherText = new BitString("08a4e2efec8a8e3312ca7460b9040bbf")
                }
            };
        }

        public static List<AlgoArrayResponse> GetGFSBox192BitKey()
        {
            int count = 0;
            BitString key = new BitString("000000000000000000000000000000000000000000000000");

            return new List<AlgoArrayResponse>()
            {
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("1b077a6af4b7f98229de786d7516b639"),
                    CipherText = new BitString("275cfc0413d8ccb70513c3859b1d0f72")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("9c2d8842e5f48f57648205d39a239af1"),
                    CipherText = new BitString("c9b8135ff1b5adc413dfd053b21bd96d")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("bff52510095f518ecca60af4205444bb"),
                    CipherText = new BitString("4a3650c3371ce2eb35e389a171427440")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("51719783d3185a535bd75adc65071ce1"),
                    CipherText = new BitString("4f354592ff7c8847d2d0870ca9481b7c")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("26aa49dcfe7629a8901a69a9914e6dfd"),
                    CipherText = new BitString("d5e08bf9a182e857cf40b3a36ee248cc")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("941a4773058224e1ef66d10e0a6ee782"),
                    CipherText = new BitString("067cd9d3749207791841562507fa9626")
                }
            };
        }

        public static List<AlgoArrayResponse> GetGFSBox256BitKey()
        {
            int count = 0;
            BitString key = new BitString("0000000000000000000000000000000000000000000000000000000000000000");

            return new List<AlgoArrayResponse>()
            {
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("014730f80ac625fe84f026c60bfd547d"),
                    CipherText = new BitString("5c9d844ed46f9885085e5d6a4f94c7d7")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("0b24af36193ce4665f2825d7b4749c98"),
                    CipherText = new BitString("a9ff75bd7cf6613d3731c77c3b6d0c04")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("761c1fe41a18acf20d241650611d90f1"),
                    CipherText = new BitString("623a52fcea5d443e48d9181ab32c7421")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("8a560769d605868ad80d819bdba03771"),
                    CipherText = new BitString("38f2c7ae10612415d27ca190d27da8b4")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("91fbef2d15a97816060bee1feaa49afe"),
                    CipherText = new BitString("1bc704f1bce135ceb810341b216d7abe")
                },
            };
        }
        #endregion GFSBox

        #region KeySBox
        public static List<AlgoArrayResponse> GetKeySBox128BitKey()
        {
            int count = 0;
            BitString plainText = new BitString("00000000000000000000000000000000");

            return new List<AlgoArrayResponse>()
            {
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("10a58869d74be5a374cf867cfb473859"),
                    CipherText = new BitString("6d251e6944b051e04eaa6fb4dbf78465")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("caea65cdbb75e9169ecd22ebe6e54675"),
                    CipherText = new BitString("6e29201190152df4ee058139def610bb")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("a2e2fa9baf7d20822ca9f0542f764a41"),
                    CipherText = new BitString("c3b44b95d9d2f25670eee9a0de099fa3")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("b6364ac4e1de1e285eaf144a2415f7a0"),
                    CipherText = new BitString("5d9b05578fc944b3cf1ccf0e746cd581")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("64cf9c7abc50b888af65f49d521944b2"),
                    CipherText = new BitString("f7efc89d5dba578104016ce5ad659c05")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("47d6742eefcc0465dc96355e851b64d9"),
                    CipherText = new BitString("0306194f666d183624aa230a8b264ae7")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("3eb39790678c56bee34bbcdeccf6cdb5"),
                    CipherText = new BitString("858075d536d79ccee571f7d7204b1f67")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("64110a924f0743d500ccadae72c13427"),
                    CipherText = new BitString("35870c6a57e9e92314bcb8087cde72ce")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("18d8126516f8a12ab1a36d9f04d68e51"),
                    CipherText = new BitString("6c68e9be5ec41e22c825b7c7affb4363")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("f530357968578480b398a3c251cd1093"),
                    CipherText = new BitString("f5df39990fc688f1b07224cc03e86cea")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("da84367f325d42d601b4326964802e8e"),
                    CipherText = new BitString("bba071bcb470f8f6586e5d3add18bc66")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("e37b1c6aa2846f6fdb413f238b089f23"),
                    CipherText = new BitString("43c9f7e62f5d288bb27aa40ef8fe1ea8")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("6c002b682483e0cabcc731c253be5674"),
                    CipherText = new BitString("3580d19cff44f1014a7c966a69059de5")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("143ae8ed6555aba96110ab58893a8ae1"),
                    CipherText = new BitString("806da864dd29d48deafbe764f8202aef")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("b69418a85332240dc82492353956ae0c"),
                    CipherText = new BitString("a303d940ded8f0baff6f75414cac5243")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("71b5c08a1993e1362e4d0ce9b22b78d5"),
                    CipherText = new BitString("c2dabd117f8a3ecabfbb11d12194d9d0")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("e234cdca2606b81f29408d5f6da21206"),
                    CipherText = new BitString("fff60a4740086b3b9c56195b98d91a7b")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("13237c49074a3da078dc1d828bb78c6f"),
                    CipherText = new BitString("8146a08e2357f0caa30ca8c94d1a0544")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("3071a2a48fe6cbd04f1a129098e308f8"),
                    CipherText = new BitString("4b98e06d356deb07ebb824e5713f7be3")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("90f42ec0f68385f2ffc5dfc03a654dce"),
                    CipherText = new BitString("7a20a53d460fc9ce0423a7a0764c6cf2")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("febd9a24d8b65c1c787d50a4ed3619a9"),
                    CipherText = new BitString("f4a70d8af877f9b02b4c40df57d45b17")
                },
            };
        }

        public static List<AlgoArrayResponse> GetKeySBox192BitKey()
        {
            int count = 0;
            BitString plainText = new BitString("00000000000000000000000000000000");

            return new List<AlgoArrayResponse>()
            {
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("e9f065d7c13573587f7875357dfbb16c53489f6a4bd0f7cd"),
                    CipherText = new BitString("0956259c9cd5cfd0181cca53380cde06")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("15d20f6ebc7e649fd95b76b107e6daba967c8a9484797f29"),
                    CipherText = new BitString("8e4e18424e591a3d5b6f0876f16f8594")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("a8a282ee31c03fae4f8e9b8930d5473c2ed695a347e88b7c"),
                    CipherText = new BitString("93f3270cfc877ef17e106ce938979cb0")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("cd62376d5ebb414917f0c78f05266433dc9192a1ec943300"),
                    CipherText = new BitString("7f6c25ff41858561bb62f36492e93c29")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("502a6ab36984af268bf423c7f509205207fc1552af4a91e5"),
                    CipherText = new BitString("8e06556dcbb00b809a025047cff2a940")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("25a39dbfd8034f71a81f9ceb55026e4037f8f6aa30ab44ce"),
                    CipherText = new BitString("3608c344868e94555d23a120f8a5502d")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("e08c15411774ec4a908b64eadc6ac4199c7cd453f3aaef53"),
                    CipherText = new BitString("77da2021935b840b7f5dcc39132da9e5")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("3b375a1ff7e8d44409696e6326ec9dec86138e2ae010b980"),
                    CipherText = new BitString("3b7c24f825e3bf9873c9f14d39a0e6f4")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("950bb9f22cc35be6fe79f52c320af93dec5bc9c0c2f9cd53"),
                    CipherText = new BitString("64ebf95686b353508c90ecd8b6134316")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("7001c487cc3e572cfc92f4d0e697d982e8856fdcc957da40"),
                    CipherText = new BitString("ff558c5d27210b7929b73fc708eb4cf1")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("f029ce61d4e5a405b41ead0a883cc6a737da2cf50a6c92ae"),
                    CipherText = new BitString("a2c3b2a818075490a7b4c14380f02702")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("61257134a518a0d57d9d244d45f6498cbc32f2bafc522d79"),
                    CipherText = new BitString("cfe4d74002696ccf7d87b14a2f9cafc9")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("b0ab0a6a818baef2d11fa33eac947284fb7d748cfb75e570"),
                    CipherText = new BitString("d2eafd86f63b109b91f5dbb3a3fb7e13")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ee053aa011c8b428cdcc3636313c54d6a03cac01c71579d6"),
                    CipherText = new BitString("9b9fdd1c5975655f539998b306a324af")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("d2926527e0aa9f37b45e2ec2ade5853ef807576104c7ace3"),
                    CipherText = new BitString("dd619e1cf204446112e0af2b9afa8f8c")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("982215f4e173dfa0fcffe5d3da41c4812c7bcc8ed3540f93"),
                    CipherText = new BitString("d4f0aae13c8fe9339fbf9e69ed0ad74d")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("98c6b8e01e379fbd14e61af6af891596583565f2a27d59e9"),
                    CipherText = new BitString("19c80ec4a6deb7e5ed1033dda933498f")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("b3ad5cea1dddc214ca969ac35f37dae1a9a9d1528f89bb35"),
                    CipherText = new BitString("3cf5e1d21a17956d1dffad6a7c41c659")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("45899367c3132849763073c435a9288a766c8b9ec2308516"),
                    CipherText = new BitString("69fd12e8505f8ded2fdcb197a121b362")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ec250e04c3903f602647b85a401a1ae7ca2f02f67fa4253e"),
                    CipherText = new BitString("8aa584e2cc4d17417a97cb9a28ba29c8")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("d077a03bd8a38973928ccafe4a9d2f455130bd0af5ae46a9"),
                    CipherText = new BitString("abc786fb1edb504580c4d882ef29a0c7")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("d184c36cf0dddfec39e654195006022237871a47c33d3198"),
                    CipherText = new BitString("2e19fb60a3e1de0166f483c97824a978")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("4c6994ffa9dcdc805b60c2c0095334c42d95a8fc0ca5b080"),
                    CipherText = new BitString("7656709538dd5fec41e0ce6a0f8e207d")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("c88f5b00a4ef9a6840e2acaf33f00a3bdc4e25895303fa72"),
                    CipherText = new BitString("a67cf333b314d411d3c0ae6e1cfcd8f5")
                },
            };
        }

        public static List<AlgoArrayResponse> GetKeySBox256BitKey()
        {
            int count = 0;
            BitString plainText = new BitString("00000000000000000000000000000000");

            return new List<AlgoArrayResponse>()
            {
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("c47b0294dbbbee0fec4757f22ffeee3587ca4730c3d33b691df38bab076bc558"),
                    CipherText = new BitString("46f2fb342d6f0ab477476fc501242c5f")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("28d46cffa158533194214a91e712fc2b45b518076675affd910edeca5f41ac64"),
                    CipherText = new BitString("4bf3b0a69aeb6657794f2901b1440ad4")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("c1cc358b449909a19436cfbb3f852ef8bcb5ed12ac7058325f56e6099aab1a1c"),
                    CipherText = new BitString("352065272169abf9856843927d0674fd")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("984ca75f4ee8d706f46c2d98c0bf4a45f5b00d791c2dfeb191b5ed8e420fd627"),
                    CipherText = new BitString("4307456a9e67813b452e15fa8fffe398")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("b43d08a447ac8609baadae4ff12918b9f68fc1653f1269222f123981ded7a92f"),
                    CipherText = new BitString("4663446607354989477a5c6f0f007ef4")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("1d85a181b54cde51f0e098095b2962fdc93b51fe9b88602b3f54130bf76a5bd9"),
                    CipherText = new BitString("531c2c38344578b84d50b3c917bbb6e1")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("dc0eba1f2232a7879ded34ed8428eeb8769b056bbaf8ad77cb65c3541430b4cf"),
                    CipherText = new BitString("fc6aec906323480005c58e7e1ab004ad")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("f8be9ba615c5a952cabbca24f68f8593039624d524c816acda2c9183bd917cb9"),
                    CipherText = new BitString("a3944b95ca0b52043584ef02151926a8")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("797f8b3d176dac5b7e34a2d539c4ef367a16f8635f6264737591c5c07bf57a3e"),
                    CipherText = new BitString("a74289fe73a4c123ca189ea1e1b49ad5")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("6838d40caf927749c13f0329d331f448e202c73ef52c5f73a37ca635d4c47707"),
                    CipherText = new BitString("b91d4ea4488644b56cf0812fa7fcf5fc")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ccd1bc3c659cd3c59bc437484e3c5c724441da8d6e90ce556cd57d0752663bbc"),
                    CipherText = new BitString("304f81ab61a80c2e743b94d5002a126b")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("13428b5e4c005e0636dd338405d173ab135dec2a25c22c5df0722d69dcc43887"),
                    CipherText = new BitString("649a71545378c783e368c9ade7114f6c")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("07eb03a08d291d1b07408bf3512ab40c91097ac77461aad4bb859647f74f00ee"),
                    CipherText = new BitString("47cb030da2ab051dfc6c4bf6910d12bb")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("90143ae20cd78c5d8ebdd6cb9dc1762427a96c78c639bccc41a61424564eafe1"),
                    CipherText = new BitString("798c7c005dee432b2c8ea5dfa381ecc3")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("b7a5794d52737475d53d5a377200849be0260a67a2b22ced8bbef12882270d07"),
                    CipherText = new BitString("637c31dc2591a07636f646b72daabbe7")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fca02f3d5011cfc5c1e23165d413a049d4526a991827424d896fe3435e0bf68e"),
                    CipherText = new BitString("179a49c712154bbffbe6e7a84a18e220")
                },
            };
        }
        #endregion KeySBox

        #region VarTxt
        public static List<AlgoArrayResponse> GetVarTxt128BitKey()
        {
            int count = 0;
            BitString key = new BitString("00000000000000000000000000000000");

            return new List<AlgoArrayResponse>()
            {
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("80000000000000000000000000000000"),
                    CipherText = new BitString("3ad78e726c1ec02b7ebfe92b23d9ec34")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("c0000000000000000000000000000000"),
                    CipherText = new BitString("aae5939c8efdf2f04e60b9fe7117b2c2")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("e0000000000000000000000000000000"),
                    CipherText = new BitString("f031d4d74f5dcbf39daaf8ca3af6e527")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("f0000000000000000000000000000000"),
                    CipherText = new BitString("96d9fd5cc4f07441727df0f33e401a36")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("f8000000000000000000000000000000"),
                    CipherText = new BitString("30ccdb044646d7e1f3ccea3dca08b8c0")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fc000000000000000000000000000000"),
                    CipherText = new BitString("16ae4ce5042a67ee8e177b7c587ecc82")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fe000000000000000000000000000000"),
                    CipherText = new BitString("b6da0bb11a23855d9c5cb1b4c6412e0a")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ff000000000000000000000000000000"),
                    CipherText = new BitString("db4f1aa530967d6732ce4715eb0ee24b")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ff800000000000000000000000000000"),
                    CipherText = new BitString("a81738252621dd180a34f3455b4baa2f")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffc00000000000000000000000000000"),
                    CipherText = new BitString("77e2b508db7fd89234caf7939ee5621a")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffe00000000000000000000000000000"),
                    CipherText = new BitString("b8499c251f8442ee13f0933b688fcd19")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fff00000000000000000000000000000"),
                    CipherText = new BitString("965135f8a81f25c9d630b17502f68e53")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fff80000000000000000000000000000"),
                    CipherText = new BitString("8b87145a01ad1c6cede995ea3670454f")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffc0000000000000000000000000000"),
                    CipherText = new BitString("8eae3b10a0c8ca6d1d3b0fa61e56b0b2")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffe0000000000000000000000000000"),
                    CipherText = new BitString("64b4d629810fda6bafdf08f3b0d8d2c5")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffff0000000000000000000000000000"),
                    CipherText = new BitString("d7e5dbd3324595f8fdc7d7c571da6c2a")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffff8000000000000000000000000000"),
                    CipherText = new BitString("f3f72375264e167fca9de2c1527d9606")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffc000000000000000000000000000"),
                    CipherText = new BitString("8ee79dd4f401ff9b7ea945d86666c13b")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffe000000000000000000000000000"),
                    CipherText = new BitString("dd35cea2799940b40db3f819cb94c08b")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffff000000000000000000000000000"),
                    CipherText = new BitString("6941cb6b3e08c2b7afa581ebdd607b87")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffff800000000000000000000000000"),
                    CipherText = new BitString("2c20f439f6bb097b29b8bd6d99aad799")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffc00000000000000000000000000"),
                    CipherText = new BitString("625d01f058e565f77ae86378bd2c49b3")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffe00000000000000000000000000"),
                    CipherText = new BitString("c0b5fd98190ef45fbb4301438d095950")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffff00000000000000000000000000"),
                    CipherText = new BitString("13001ff5d99806efd25da34f56be854b")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffff80000000000000000000000000"),
                    CipherText = new BitString("3b594c60f5c8277a5113677f94208d82")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffc0000000000000000000000000"),
                    CipherText = new BitString("e9c0fc1818e4aa46bd2e39d638f89e05")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffe0000000000000000000000000"),
                    CipherText = new BitString("f8023ee9c3fdc45a019b4e985c7e1a54")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffff0000000000000000000000000"),
                    CipherText = new BitString("35f40182ab4662f3023baec1ee796b57")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffff8000000000000000000000000"),
                    CipherText = new BitString("3aebbad7303649b4194a6945c6cc3694")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffc000000000000000000000000"),
                    CipherText = new BitString("a2124bea53ec2834279bed7f7eb0f938")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffe000000000000000000000000"),
                    CipherText = new BitString("b9fb4399fa4facc7309e14ec98360b0a")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffff000000000000000000000000"),
                    CipherText = new BitString("c26277437420c5d634f715aea81a9132")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffff800000000000000000000000"),
                    CipherText = new BitString("171a0e1b2dd424f0e089af2c4c10f32f")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffc00000000000000000000000"),
                    CipherText = new BitString("7cadbe402d1b208fe735edce00aee7ce")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffe00000000000000000000000"),
                    CipherText = new BitString("43b02ff929a1485af6f5c6d6558baa0f")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffff00000000000000000000000"),
                    CipherText = new BitString("092faacc9bf43508bf8fa8613ca75dea")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffff80000000000000000000000"),
                    CipherText = new BitString("cb2bf8280f3f9742c7ed513fe802629c")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffc0000000000000000000000"),
                    CipherText = new BitString("215a41ee442fa992a6e323986ded3f68")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffe0000000000000000000000"),
                    CipherText = new BitString("f21e99cf4f0f77cea836e11a2fe75fb1")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffff0000000000000000000000"),
                    CipherText = new BitString("95e3a0ca9079e646331df8b4e70d2cd6")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffff8000000000000000000000"),
                    CipherText = new BitString("4afe7f120ce7613f74fc12a01a828073")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffc000000000000000000000"),
                    CipherText = new BitString("827f000e75e2c8b9d479beed913fe678")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffe000000000000000000000"),
                    CipherText = new BitString("35830c8e7aaefe2d30310ef381cbf691")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffff000000000000000000000"),
                    CipherText = new BitString("191aa0f2c8570144f38657ea4085ebe5")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffff800000000000000000000"),
                    CipherText = new BitString("85062c2c909f15d9269b6c18ce99c4f0")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffc00000000000000000000"),
                    CipherText = new BitString("678034dc9e41b5a560ed239eeab1bc78")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffe00000000000000000000"),
                    CipherText = new BitString("c2f93a4ce5ab6d5d56f1b93cf19911c1")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffff00000000000000000000"),
                    CipherText = new BitString("1c3112bcb0c1dcc749d799743691bf82")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffff80000000000000000000"),
                    CipherText = new BitString("00c55bd75c7f9c881989d3ec1911c0d4")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffc0000000000000000000"),
                    CipherText = new BitString("ea2e6b5ef182b7dff3629abd6a12045f")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffe0000000000000000000"),
                    CipherText = new BitString("22322327e01780b17397f24087f8cc6f")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffff0000000000000000000"),
                    CipherText = new BitString("c9cacb5cd11692c373b2411768149ee7")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffff8000000000000000000"),
                    CipherText = new BitString("a18e3dbbca577860dab6b80da3139256")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffc000000000000000000"),
                    CipherText = new BitString("79b61c37bf328ecca8d743265a3d425c")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffe000000000000000000"),
                    CipherText = new BitString("d2d99c6bcc1f06fda8e27e8ae3f1ccc7")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffff000000000000000000"),
                    CipherText = new BitString("1bfd4b91c701fd6b61b7f997829d663b")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffff800000000000000000"),
                    CipherText = new BitString("11005d52f25f16bdc9545a876a63490a")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffc00000000000000000"),
                    CipherText = new BitString("3a4d354f02bb5a5e47d39666867f246a")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffe00000000000000000"),
                    CipherText = new BitString("d451b8d6e1e1a0ebb155fbbf6e7b7dc3")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffff00000000000000000"),
                    CipherText = new BitString("6898d4f42fa7ba6a10ac05e87b9f2080")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffff80000000000000000"),
                    CipherText = new BitString("b611295e739ca7d9b50f8e4c0e754a3f")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffc0000000000000000"),
                    CipherText = new BitString("7d33fc7d8abe3ca1936759f8f5deaf20")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffe0000000000000000"),
                    CipherText = new BitString("3b5e0f566dc96c298f0c12637539b25c")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffff0000000000000000"),
                    CipherText = new BitString("f807c3e7985fe0f5a50e2cdb25c5109e")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffff8000000000000000"),
                    CipherText = new BitString("41f992a856fb278b389a62f5d274d7e9")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffc000000000000000"),
                    CipherText = new BitString("10d3ed7a6fe15ab4d91acbc7d0767ab1")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffe000000000000000"),
                    CipherText = new BitString("21feecd45b2e675973ac33bf0c5424fc")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffff000000000000000"),
                    CipherText = new BitString("1480cb3955ba62d09eea668f7c708817")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffff800000000000000"),
                    CipherText = new BitString("66404033d6b72b609354d5496e7eb511")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffc00000000000000"),
                    CipherText = new BitString("1c317a220a7d700da2b1e075b00266e1")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffe00000000000000"),
                    CipherText = new BitString("ab3b89542233f1271bf8fd0c0f403545")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffff00000000000000"),
                    CipherText = new BitString("d93eae966fac46dca927d6b114fa3f9e")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffff80000000000000"),
                    CipherText = new BitString("1bdec521316503d9d5ee65df3ea94ddf")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffc0000000000000"),
                    CipherText = new BitString("eef456431dea8b4acf83bdae3717f75f")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffe0000000000000"),
                    CipherText = new BitString("06f2519a2fafaa596bfef5cfa15c21b9")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffff0000000000000"),
                    CipherText = new BitString("251a7eac7e2fe809e4aa8d0d7012531a")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffff8000000000000"),
                    CipherText = new BitString("3bffc16e4c49b268a20f8d96a60b4058")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffc000000000000"),
                    CipherText = new BitString("e886f9281999c5bb3b3e8862e2f7c988")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffe000000000000"),
                    CipherText = new BitString("563bf90d61beef39f48dd625fcef1361")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffff000000000000"),
                    CipherText = new BitString("4d37c850644563c69fd0acd9a049325b")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffff800000000000"),
                    CipherText = new BitString("b87c921b91829ef3b13ca541ee1130a6")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffc00000000000"),
                    CipherText = new BitString("2e65eb6b6ea383e109accce8326b0393")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffe00000000000"),
                    CipherText = new BitString("9ca547f7439edc3e255c0f4d49aa8990")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffff00000000000"),
                    CipherText = new BitString("a5e652614c9300f37816b1f9fd0c87f9")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffff80000000000"),
                    CipherText = new BitString("14954f0b4697776f44494fe458d814ed")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffc0000000000"),
                    CipherText = new BitString("7c8d9ab6c2761723fe42f8bb506cbcf7")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffe0000000000"),
                    CipherText = new BitString("db7e1932679fdd99742aab04aa0d5a80")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffff0000000000"),
                    CipherText = new BitString("4c6a1c83e568cd10f27c2d73ded19c28")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffff8000000000"),
                    CipherText = new BitString("90ecbe6177e674c98de412413f7ac915")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffc000000000"),
                    CipherText = new BitString("90684a2ac55fe1ec2b8ebd5622520b73")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffe000000000"),
                    CipherText = new BitString("7472f9a7988607ca79707795991035e6")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffff000000000"),
                    CipherText = new BitString("56aff089878bf3352f8df172a3ae47d8")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffff800000000"),
                    CipherText = new BitString("65c0526cbe40161b8019a2a3171abd23")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffc00000000"),
                    CipherText = new BitString("377be0be33b4e3e310b4aabda173f84f")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffe00000000"),
                    CipherText = new BitString("9402e9aa6f69de6504da8d20c4fcaa2f")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffff00000000"),
                    CipherText = new BitString("123c1f4af313ad8c2ce648b2e71fb6e1")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffff80000000"),
                    CipherText = new BitString("1ffc626d30203dcdb0019fb80f726cf4")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffc0000000"),
                    CipherText = new BitString("76da1fbe3a50728c50fd2e621b5ad885")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffe0000000"),
                    CipherText = new BitString("082eb8be35f442fb52668e16a591d1d6")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffff0000000"),
                    CipherText = new BitString("e656f9ecf5fe27ec3e4a73d00c282fb3")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffff8000000"),
                    CipherText = new BitString("2ca8209d63274cd9a29bb74bcd77683a")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffc000000"),
                    CipherText = new BitString("79bf5dce14bb7dd73a8e3611de7ce026")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffe000000"),
                    CipherText = new BitString("3c849939a5d29399f344c4a0eca8a576")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffff000000"),
                    CipherText = new BitString("ed3c0a94d59bece98835da7aa4f07ca2")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffff800000"),
                    CipherText = new BitString("63919ed4ce10196438b6ad09d99cd795")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffc00000"),
                    CipherText = new BitString("7678f3a833f19fea95f3c6029e2bc610")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffe00000"),
                    CipherText = new BitString("3aa426831067d36b92be7c5f81c13c56")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffff00000"),
                    CipherText = new BitString("9272e2d2cdd11050998c845077a30ea0")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffff80000"),
                    CipherText = new BitString("088c4b53f5ec0ff814c19adae7f6246c")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffc0000"),
                    CipherText = new BitString("4010a5e401fdf0a0354ddbcc0d012b17")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffe0000"),
                    CipherText = new BitString("a87a385736c0a6189bd6589bd8445a93")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffff0000"),
                    CipherText = new BitString("545f2b83d9616dccf60fa9830e9cd287")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffff8000"),
                    CipherText = new BitString("4b706f7f92406352394037a6d4f4688d")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffffc000"),
                    CipherText = new BitString("b7972b3941c44b90afa7b264bfba7387")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffffe000"),
                    CipherText = new BitString("6f45732cf10881546f0fd23896d2bb60")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffff000"),
                    CipherText = new BitString("2e3579ca15af27f64b3c955a5bfc30ba")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffff800"),
                    CipherText = new BitString("34a2c5a91ae2aec99b7d1b5fa6780447")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffffc00"),
                    CipherText = new BitString("a4d6616bd04f87335b0e53351227a9ee")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffffe00"),
                    CipherText = new BitString("7f692b03945867d16179a8cefc83ea3f")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffffff00"),
                    CipherText = new BitString("3bd141ee84a0e6414a26e7a4f281f8a2")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffffff80"),
                    CipherText = new BitString("d1788f572d98b2b16ec5d5f3922b99bc")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffffffc0"),
                    CipherText = new BitString("0833ff6f61d98a57b288e8c3586b85a6")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffffffe0"),
                    CipherText = new BitString("8568261797de176bf0b43becc6285afb")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffffff0"),
                    CipherText = new BitString("f9b0fda0c4a898f5b9e6f661c4ce4d07")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffffff8"),
                    CipherText = new BitString("8ade895913685c67c5269f8aae42983e")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffffffc"),
                    CipherText = new BitString("39bde67d5c8ed8a8b1c37eb8fa9f5ac0")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffffffe"),
                    CipherText = new BitString("5c005e72c1418c44f569f2ea33ba54f3")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffffffff"),
                    CipherText = new BitString("3f5b8cc9ea855a0afa7347d23e8d664e")
                },
            };
        }

        public static List<AlgoArrayResponse> GetVarTxt192BitKey()
        {
            int count = 0;
            BitString key = new BitString("000000000000000000000000000000000000000000000000");

            return new List<AlgoArrayResponse>()
            {
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("80000000000000000000000000000000"),
                    CipherText = new BitString("6cd02513e8d4dc986b4afe087a60bd0c")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("c0000000000000000000000000000000"),
                    CipherText = new BitString("2ce1f8b7e30627c1c4519eada44bc436")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("e0000000000000000000000000000000"),
                    CipherText = new BitString("9946b5f87af446f5796c1fee63a2da24")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("f0000000000000000000000000000000"),
                    CipherText = new BitString("2a560364ce529efc21788779568d5555")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("f8000000000000000000000000000000"),
                    CipherText = new BitString("35c1471837af446153bce55d5ba72a0a")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fc000000000000000000000000000000"),
                    CipherText = new BitString("ce60bc52386234f158f84341e534cd9e")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fe000000000000000000000000000000"),
                    CipherText = new BitString("8c7c27ff32bcf8dc2dc57c90c2903961")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ff000000000000000000000000000000"),
                    CipherText = new BitString("32bb6a7ec84499e166f936003d55a5bb")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ff800000000000000000000000000000"),
                    CipherText = new BitString("a5c772e5c62631ef660ee1d5877f6d1b")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffc00000000000000000000000000000"),
                    CipherText = new BitString("030d7e5b64f380a7e4ea5387b5cd7f49")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffe00000000000000000000000000000"),
                    CipherText = new BitString("0dc9a2610037009b698f11bb7e86c83e")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fff00000000000000000000000000000"),
                    CipherText = new BitString("0046612c766d1840c226364f1fa7ed72")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fff80000000000000000000000000000"),
                    CipherText = new BitString("4880c7e08f27befe78590743c05e698b")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffc0000000000000000000000000000"),
                    CipherText = new BitString("2520ce829a26577f0f4822c4ecc87401")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffe0000000000000000000000000000"),
                    CipherText = new BitString("8765e8acc169758319cb46dc7bcf3dca")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffff0000000000000000000000000000"),
                    CipherText = new BitString("e98f4ba4f073df4baa116d011dc24a28")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffff8000000000000000000000000000"),
                    CipherText = new BitString("f378f68c5dbf59e211b3a659a7317d94")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffc000000000000000000000000000"),
                    CipherText = new BitString("283d3b069d8eb9fb432d74b96ca762b4")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffe000000000000000000000000000"),
                    CipherText = new BitString("a7e1842e8a87861c221a500883245c51")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffff000000000000000000000000000"),
                    CipherText = new BitString("77aa270471881be070fb52c7067ce732")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffff800000000000000000000000000"),
                    CipherText = new BitString("01b0f476d484f43f1aeb6efa9361a8ac")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffc00000000000000000000000000"),
                    CipherText = new BitString("1c3a94f1c052c55c2d8359aff2163b4f")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffe00000000000000000000000000"),
                    CipherText = new BitString("e8a067b604d5373d8b0f2e05a03b341b")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffff00000000000000000000000000"),
                    CipherText = new BitString("a7876ec87f5a09bfea42c77da30fd50e")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffff80000000000000000000000000"),
                    CipherText = new BitString("0cf3e9d3a42be5b854ca65b13f35f48d")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffc0000000000000000000000000"),
                    CipherText = new BitString("6c62f6bbcab7c3e821c9290f08892dda")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffe0000000000000000000000000"),
                    CipherText = new BitString("7f5e05bd2068738196fee79ace7e3aec")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffff0000000000000000000000000"),
                    CipherText = new BitString("440e0d733255cda92fb46e842fe58054")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffff8000000000000000000000000"),
                    CipherText = new BitString("aa5d5b1c4ea1b7a22e5583ac2e9ed8a7")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffc000000000000000000000000"),
                    CipherText = new BitString("77e537e89e8491e8662aae3bc809421d")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffe000000000000000000000000"),
                    CipherText = new BitString("997dd3e9f1598bfa73f75973f7e93b76")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffff000000000000000000000000"),
                    CipherText = new BitString("1b38d4f7452afefcb7fc721244e4b72e")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffff800000000000000000000000"),
                    CipherText = new BitString("0be2b18252e774dda30cdda02c6906e3")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffc00000000000000000000000"),
                    CipherText = new BitString("d2695e59c20361d82652d7d58b6f11b2")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffe00000000000000000000000"),
                    CipherText = new BitString("902d88d13eae52089abd6143cfe394e9")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffff00000000000000000000000"),
                    CipherText = new BitString("d49bceb3b823fedd602c305345734bd2")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffff80000000000000000000000"),
                    CipherText = new BitString("707b1dbb0ffa40ef7d95def421233fae")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffc0000000000000000000000"),
                    CipherText = new BitString("7ca0c1d93356d9eb8aa952084d75f913")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffe0000000000000000000000"),
                    CipherText = new BitString("f2cbf9cb186e270dd7bdb0c28febc57d")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffff0000000000000000000000"),
                    CipherText = new BitString("c94337c37c4e790ab45780bd9c3674a0")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffff8000000000000000000000"),
                    CipherText = new BitString("8e3558c135252fb9c9f367ed609467a1")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffc000000000000000000000"),
                    CipherText = new BitString("1b72eeaee4899b443914e5b3a57fba92")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffe000000000000000000000"),
                    CipherText = new BitString("011865f91bc56868d051e52c9efd59b7")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffff000000000000000000000"),
                    CipherText = new BitString("e4771318ad7a63dd680f6e583b7747ea")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffff800000000000000000000"),
                    CipherText = new BitString("61e3d194088dc8d97e9e6db37457eac5")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffc00000000000000000000"),
                    CipherText = new BitString("36ff1ec9ccfbc349e5d356d063693ad6")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffe00000000000000000000"),
                    CipherText = new BitString("3cc9e9a9be8cc3f6fb2ea24088e9bb19")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffff00000000000000000000"),
                    CipherText = new BitString("1ee5ab003dc8722e74905d9a8fe3d350")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffff80000000000000000000"),
                    CipherText = new BitString("245339319584b0a412412869d6c2eada")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffc0000000000000000000"),
                    CipherText = new BitString("7bd496918115d14ed5380852716c8814")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffe0000000000000000000"),
                    CipherText = new BitString("273ab2f2b4a366a57d582a339313c8b1")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffff0000000000000000000"),
                    CipherText = new BitString("113365a9ffbe3b0ca61e98507554168b")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffff8000000000000000000"),
                    CipherText = new BitString("afa99c997ac478a0dea4119c9e45f8b1")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffc000000000000000000"),
                    CipherText = new BitString("9216309a7842430b83ffb98638011512")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffe000000000000000000"),
                    CipherText = new BitString("62abc792288258492a7cb45145f4b759")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffff000000000000000000"),
                    CipherText = new BitString("534923c169d504d7519c15d30e756c50")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffff800000000000000000"),
                    CipherText = new BitString("fa75e05bcdc7e00c273fa33f6ee441d2")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffc00000000000000000"),
                    CipherText = new BitString("7d350fa6057080f1086a56b17ec240db")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffe00000000000000000"),
                    CipherText = new BitString("f34e4a6324ea4a5c39a661c8fe5ada8f")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffff00000000000000000"),
                    CipherText = new BitString("0882a16f44088d42447a29ac090ec17e")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffff80000000000000000"),
                    CipherText = new BitString("3a3c15bfc11a9537c130687004e136ee")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffc0000000000000000"),
                    CipherText = new BitString("22c0a7678dc6d8cf5c8a6d5a9960767c")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffe0000000000000000"),
                    CipherText = new BitString("b46b09809d68b9a456432a79bdc2e38c")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffff0000000000000000"),
                    CipherText = new BitString("93baaffb35fbe739c17c6ac22eecf18f")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffff8000000000000000"),
                    CipherText = new BitString("c8aa80a7850675bc007c46df06b49868")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffc000000000000000"),
                    CipherText = new BitString("12c6f3877af421a918a84b775858021d")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffe000000000000000"),
                    CipherText = new BitString("33f123282c5d633924f7d5ba3f3cab11")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffff000000000000000"),
                    CipherText = new BitString("a8f161002733e93ca4527d22c1a0c5bb")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffff800000000000000"),
                    CipherText = new BitString("b72f70ebf3e3fda23f508eec76b42c02")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffc00000000000000"),
                    CipherText = new BitString("6a9d965e6274143f25afdcfc88ffd77c")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffe00000000000000"),
                    CipherText = new BitString("a0c74fd0b9361764ce91c5200b095357")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffff00000000000000"),
                    CipherText = new BitString("091d1fdc2bd2c346cd5046a8c6209146")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffff80000000000000"),
                    CipherText = new BitString("e2a37580116cfb71856254496ab0aca8")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffc0000000000000"),
                    CipherText = new BitString("e0b3a00785917c7efc9adba322813571")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffe0000000000000"),
                    CipherText = new BitString("733d41f4727b5ef0df4af4cf3cffa0cb")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffff0000000000000"),
                    CipherText = new BitString("a99ebb030260826f981ad3e64490aa4f")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffff8000000000000"),
                    CipherText = new BitString("73f34c7d3eae5e80082c1647524308ee")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffc000000000000"),
                    CipherText = new BitString("40ebd5ad082345b7a2097ccd3464da02")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffe000000000000"),
                    CipherText = new BitString("7cc4ae9a424b2cec90c97153c2457ec5")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffff000000000000"),
                    CipherText = new BitString("54d632d03aba0bd0f91877ebdd4d09cb")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffff800000000000"),
                    CipherText = new BitString("d3427be7e4d27cd54f5fe37b03cf0897")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffc00000000000"),
                    CipherText = new BitString("b2099795e88cc158fd75ea133d7e7fbe")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffe00000000000"),
                    CipherText = new BitString("a6cae46fb6fadfe7a2c302a34242817b")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffff00000000000"),
                    CipherText = new BitString("026a7024d6a902e0b3ffccbaa910cc3f")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffff80000000000"),
                    CipherText = new BitString("156f07767a85a4312321f63968338a01")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffc0000000000"),
                    CipherText = new BitString("15eec9ebf42b9ca76897d2cd6c5a12e2")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffe0000000000"),
                    CipherText = new BitString("db0d3a6fdcc13f915e2b302ceeb70fd8")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffff0000000000"),
                    CipherText = new BitString("71dbf37e87a2e34d15b20e8f10e48924")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffff8000000000"),
                    CipherText = new BitString("c745c451e96ff3c045e4367c833e3b54")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffc000000000"),
                    CipherText = new BitString("340da09c2dd11c3b679d08ccd27dd595")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffe000000000"),
                    CipherText = new BitString("8279f7c0c2a03ee660c6d392db025d18")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffff000000000"),
                    CipherText = new BitString("a4b2c7d8eba531ff47c5041a55fbd1ec")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffff800000000"),
                    CipherText = new BitString("74569a2ca5a7bd5131ce8dc7cbfbf72f")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffc00000000"),
                    CipherText = new BitString("3713da0c0219b63454035613b5a403dd")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffe00000000"),
                    CipherText = new BitString("8827551ddcc9df23fa72a3de4e9f0b07")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffff00000000"),
                    CipherText = new BitString("2e3febfd625bfcd0a2c06eb460da1732")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffff80000000"),
                    CipherText = new BitString("ee82e6ba488156f76496311da6941deb")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffc0000000"),
                    CipherText = new BitString("4770446f01d1f391256e85a1b30d89d3")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffe0000000"),
                    CipherText = new BitString("af04b68f104f21ef2afb4767cf74143c")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffff0000000"),
                    CipherText = new BitString("cf3579a9ba38c8e43653173e14f3a4c6")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffff8000000"),
                    CipherText = new BitString("b3bba904f4953e09b54800af2f62e7d4")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffc000000"),
                    CipherText = new BitString("fc4249656e14b29eb9c44829b4c59a46")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffe000000"),
                    CipherText = new BitString("9b31568febe81cfc2e65af1c86d1a308")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffff000000"),
                    CipherText = new BitString("9ca09c25f273a766db98a480ce8dfedc")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffff800000"),
                    CipherText = new BitString("b909925786f34c3c92d971883c9fbedf")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffc00000"),
                    CipherText = new BitString("82647f1332fe570a9d4d92b2ee771d3b")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffe00000"),
                    CipherText = new BitString("3604a7e80832b3a99954bca6f5b9f501")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffff00000"),
                    CipherText = new BitString("884607b128c5de3ab39a529a1ef51bef")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffff80000"),
                    CipherText = new BitString("670cfa093d1dbdb2317041404102435e")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffc0000"),
                    CipherText = new BitString("7a867195f3ce8769cbd336502fbb5130")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffe0000"),
                    CipherText = new BitString("52efcf64c72b2f7ca5b3c836b1078c15")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffff0000"),
                    CipherText = new BitString("4019250f6eefb2ac5ccbcae044e75c7e")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffff8000"),
                    CipherText = new BitString("022c4f6f5a017d292785627667ddef24")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffffc000"),
                    CipherText = new BitString("e9c21078a2eb7e03250f71000fa9e3ed")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffffe000"),
                    CipherText = new BitString("a13eaeeb9cd391da4e2b09490b3e7fad")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffff000"),
                    CipherText = new BitString("c958a171dca1d4ed53e1af1d380803a9")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffff800"),
                    CipherText = new BitString("21442e07a110667f2583eaeeee44dc8c")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffffc00"),
                    CipherText = new BitString("59bbb353cf1dd867a6e33737af655e99")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffffe00"),
                    CipherText = new BitString("43cd3b25375d0ce41087ff9fe2829639")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffffff00"),
                    CipherText = new BitString("6b98b17e80d1118e3516bd768b285a84")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffffff80"),
                    CipherText = new BitString("ae47ed3676ca0c08deea02d95b81db58")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffffffc0"),
                    CipherText = new BitString("34ec40dc20413795ed53628ea748720b")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffffffe0"),
                    CipherText = new BitString("4dc68163f8e9835473253542c8a65d46")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffffff0"),
                    CipherText = new BitString("2aabb999f43693175af65c6c612c46fb")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffffff8"),
                    CipherText = new BitString("e01f94499dac3547515c5b1d756f0f58")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffffffc"),
                    CipherText = new BitString("9d12435a46480ce00ea349f71799df9a")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffffffe"),
                    CipherText = new BitString("cef41d16d266bdfe46938ad7884cc0cf")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffffffff"),
                    CipherText = new BitString("b13db4da1f718bc6904797c82bcf2d32")
                },
            };
        }

        public static List<AlgoArrayResponse> GetVarTxt256BitKey()
        {
            int count = 0;
            BitString key = new BitString("0000000000000000000000000000000000000000000000000000000000000000");

            return new List<AlgoArrayResponse>()
            {
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("80000000000000000000000000000000"),
                    CipherText = new BitString("ddc6bf790c15760d8d9aeb6f9a75fd4e")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("c0000000000000000000000000000000"),
                    CipherText = new BitString("0a6bdc6d4c1e6280301fd8e97ddbe601")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("e0000000000000000000000000000000"),
                    CipherText = new BitString("9b80eefb7ebe2d2b16247aa0efc72f5d")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("f0000000000000000000000000000000"),
                    CipherText = new BitString("7f2c5ece07a98d8bee13c51177395ff7")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("f8000000000000000000000000000000"),
                    CipherText = new BitString("7818d800dcf6f4be1e0e94f403d1e4c2")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fc000000000000000000000000000000"),
                    CipherText = new BitString("e74cd1c92f0919c35a0324123d6177d3")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fe000000000000000000000000000000"),
                    CipherText = new BitString("8092a4dcf2da7e77e93bdd371dfed82e")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ff000000000000000000000000000000"),
                    CipherText = new BitString("49af6b372135acef10132e548f217b17")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ff800000000000000000000000000000"),
                    CipherText = new BitString("8bcd40f94ebb63b9f7909676e667f1e7")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffc00000000000000000000000000000"),
                    CipherText = new BitString("fe1cffb83f45dcfb38b29be438dbd3ab")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffe00000000000000000000000000000"),
                    CipherText = new BitString("0dc58a8d886623705aec15cb1e70dc0e")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fff00000000000000000000000000000"),
                    CipherText = new BitString("c218faa16056bd0774c3e8d79c35a5e4")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fff80000000000000000000000000000"),
                    CipherText = new BitString("047bba83f7aa841731504e012208fc9e")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffc0000000000000000000000000000"),
                    CipherText = new BitString("dc8f0e4915fd81ba70a331310882f6da")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffe0000000000000000000000000000"),
                    CipherText = new BitString("1569859ea6b7206c30bf4fd0cbfac33c")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffff0000000000000000000000000000"),
                    CipherText = new BitString("300ade92f88f48fa2df730ec16ef44cd")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffff8000000000000000000000000000"),
                    CipherText = new BitString("1fe6cc3c05965dc08eb0590c95ac71d0")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffc000000000000000000000000000"),
                    CipherText = new BitString("59e858eaaa97fec38111275b6cf5abc0")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffe000000000000000000000000000"),
                    CipherText = new BitString("2239455e7afe3b0616100288cc5a723b")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffff000000000000000000000000000"),
                    CipherText = new BitString("3ee500c5c8d63479717163e55c5c4522")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffff800000000000000000000000000"),
                    CipherText = new BitString("d5e38bf15f16d90e3e214041d774daa8")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffc00000000000000000000000000"),
                    CipherText = new BitString("b1f4066e6f4f187dfe5f2ad1b17819d0")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffe00000000000000000000000000"),
                    CipherText = new BitString("6ef4cc4de49b11065d7af2909854794a")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffff00000000000000000000000000"),
                    CipherText = new BitString("ac86bc606b6640c309e782f232bf367f")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffff80000000000000000000000000"),
                    CipherText = new BitString("36aff0ef7bf3280772cf4cac80a0d2b2")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffc0000000000000000000000000"),
                    CipherText = new BitString("1f8eedea0f62a1406d58cfc3ecea72cf")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffe0000000000000000000000000"),
                    CipherText = new BitString("abf4154a3375a1d3e6b1d454438f95a6")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffff0000000000000000000000000"),
                    CipherText = new BitString("96f96e9d607f6615fc192061ee648b07")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffff8000000000000000000000000"),
                    CipherText = new BitString("cf37cdaaa0d2d536c71857634c792064")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffc000000000000000000000000"),
                    CipherText = new BitString("fbd6640c80245c2b805373f130703127")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffe000000000000000000000000"),
                    CipherText = new BitString("8d6a8afe55a6e481badae0d146f436db")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffff000000000000000000000000"),
                    CipherText = new BitString("6a4981f2915e3e68af6c22385dd06756")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffff800000000000000000000000"),
                    CipherText = new BitString("42a1136e5f8d8d21d3101998642d573b")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffc00000000000000000000000"),
                    CipherText = new BitString("9b471596dc69ae1586cee6158b0b0181")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffe00000000000000000000000"),
                    CipherText = new BitString("753665c4af1eff33aa8b628bf8741cfd")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffff00000000000000000000000"),
                    CipherText = new BitString("9a682acf40be01f5b2a4193c9a82404d")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffff80000000000000000000000"),
                    CipherText = new BitString("54fafe26e4287f17d1935f87eb9ade01")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffc0000000000000000000000"),
                    CipherText = new BitString("49d541b2e74cfe73e6a8e8225f7bd449")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffe0000000000000000000000"),
                    CipherText = new BitString("11a45530f624ff6f76a1b3826626ff7b")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffff0000000000000000000000"),
                    CipherText = new BitString("f96b0c4a8bc6c86130289f60b43b8fba")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffff8000000000000000000000"),
                    CipherText = new BitString("48c7d0e80834ebdc35b6735f76b46c8b")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffc000000000000000000000"),
                    CipherText = new BitString("2463531ab54d66955e73edc4cb8eaa45")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffe000000000000000000000"),
                    CipherText = new BitString("ac9bd8e2530469134b9d5b065d4f565b")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffff000000000000000000000"),
                    CipherText = new BitString("3f5f9106d0e52f973d4890e6f37e8a00")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffff800000000000000000000"),
                    CipherText = new BitString("20ebc86f1304d272e2e207e59db639f0")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffc00000000000000000000"),
                    CipherText = new BitString("e67ae6426bf9526c972cff072b52252c")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffe00000000000000000000"),
                    CipherText = new BitString("1a518dddaf9efa0d002cc58d107edfc8")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffff00000000000000000000"),
                    CipherText = new BitString("ead731af4d3a2fe3b34bed047942a49f")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffff80000000000000000000"),
                    CipherText = new BitString("b1d4efe40242f83e93b6c8d7efb5eae9")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffc0000000000000000000"),
                    CipherText = new BitString("cd2b1fec11fd906c5c7630099443610a")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffe0000000000000000000"),
                    CipherText = new BitString("a1853fe47fe29289d153161d06387d21")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffff0000000000000000000"),
                    CipherText = new BitString("4632154179a555c17ea604d0889fab14")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffff8000000000000000000"),
                    CipherText = new BitString("dd27cac6401a022e8f38f9f93e774417")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffc000000000000000000"),
                    CipherText = new BitString("c090313eb98674f35f3123385fb95d4d")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffe000000000000000000"),
                    CipherText = new BitString("cc3526262b92f02edce548f716b9f45c")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffff000000000000000000"),
                    CipherText = new BitString("c0838d1a2b16a7c7f0dfcc433c399c33")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffff800000000000000000"),
                    CipherText = new BitString("0d9ac756eb297695eed4d382eb126d26")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffc00000000000000000"),
                    CipherText = new BitString("56ede9dda3f6f141bff1757fa689c3e1")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffe00000000000000000"),
                    CipherText = new BitString("768f520efe0f23e61d3ec8ad9ce91774")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffff00000000000000000"),
                    CipherText = new BitString("b1144ddfa75755213390e7c596660490")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffff80000000000000000"),
                    CipherText = new BitString("1d7c0c4040b355b9d107a99325e3b050")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffc0000000000000000"),
                    CipherText = new BitString("d8e2bb1ae8ee3dcf5bf7d6c38da82a1a")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffe0000000000000000"),
                    CipherText = new BitString("faf82d178af25a9886a47e7f789b98d7")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffff0000000000000000"),
                    CipherText = new BitString("9b58dbfd77fe5aca9cfc190cd1b82d19")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffff8000000000000000"),
                    CipherText = new BitString("77f392089042e478ac16c0c86a0b5db5")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffc000000000000000"),
                    CipherText = new BitString("19f08e3420ee69b477ca1420281c4782")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffe000000000000000"),
                    CipherText = new BitString("a1b19beee4e117139f74b3c53fdcb875")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffff000000000000000"),
                    CipherText = new BitString("a37a5869b218a9f3a0868d19aea0ad6a")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffff800000000000000"),
                    CipherText = new BitString("bc3594e865bcd0261b13202731f33580")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffc00000000000000"),
                    CipherText = new BitString("811441ce1d309eee7185e8c752c07557")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffe00000000000000"),
                    CipherText = new BitString("959971ce4134190563518e700b9874d1")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffff00000000000000"),
                    CipherText = new BitString("76b5614a042707c98e2132e2e805fe63")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffff80000000000000"),
                    CipherText = new BitString("7d9fa6a57530d0f036fec31c230b0cc6")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffc0000000000000"),
                    CipherText = new BitString("964153a83bf6989a4ba80daa91c3e081")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffe0000000000000"),
                    CipherText = new BitString("a013014d4ce8054cf2591d06f6f2f176")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffff0000000000000"),
                    CipherText = new BitString("d1c5f6399bf382502e385eee1474a869")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffff8000000000000"),
                    CipherText = new BitString("0007e20b8298ec354f0f5fe7470f36bd")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffc000000000000"),
                    CipherText = new BitString("b95ba05b332da61ef63a2b31fcad9879")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffe000000000000"),
                    CipherText = new BitString("4620a49bd967491561669ab25dce45f4")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffff000000000000"),
                    CipherText = new BitString("12e71214ae8e04f0bb63d7425c6f14d5")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffff800000000000"),
                    CipherText = new BitString("4cc42fc1407b008fe350907c092e80ac")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffc00000000000"),
                    CipherText = new BitString("08b244ce7cbc8ee97fbba808cb146fda")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffe00000000000"),
                    CipherText = new BitString("39b333e8694f21546ad1edd9d87ed95b")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffff00000000000"),
                    CipherText = new BitString("3b271f8ab2e6e4a20ba8090f43ba78f3")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffff80000000000"),
                    CipherText = new BitString("9ad983f3bf651cd0393f0a73cccdea50")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffc0000000000"),
                    CipherText = new BitString("8f476cbff75c1f725ce18e4bbcd19b32")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffe0000000000"),
                    CipherText = new BitString("905b6267f1d6ab5320835a133f096f2a")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffff0000000000"),
                    CipherText = new BitString("145b60d6d0193c23f4221848a892d61a")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffff8000000000"),
                    CipherText = new BitString("55cfb3fb6d75cad0445bbc8dafa25b0f")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffc000000000"),
                    CipherText = new BitString("7b8e7098e357ef71237d46d8b075b0f5")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffe000000000"),
                    CipherText = new BitString("2bf27229901eb40f2df9d8398d1505ae")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffff000000000"),
                    CipherText = new BitString("83a63402a77f9ad5c1e931a931ecd706")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffff800000000"),
                    CipherText = new BitString("6f8ba6521152d31f2bada1843e26b973")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffc00000000"),
                    CipherText = new BitString("e5c3b8e30fd2d8e6239b17b44bd23bbd")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffe00000000"),
                    CipherText = new BitString("1ac1f7102c59933e8b2ddc3f14e94baa")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffff00000000"),
                    CipherText = new BitString("21d9ba49f276b45f11af8fc71a088e3d")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffff80000000"),
                    CipherText = new BitString("649f1cddc3792b4638635a392bc9bade")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffc0000000"),
                    CipherText = new BitString("e2775e4b59c1bc2e31a2078c11b5a08c")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffe0000000"),
                    CipherText = new BitString("2be1fae5048a25582a679ca10905eb80")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffff0000000"),
                    CipherText = new BitString("da86f292c6f41ea34fb2068df75ecc29")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffff8000000"),
                    CipherText = new BitString("220df19f85d69b1b562fa69a3c5beca5")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffc000000"),
                    CipherText = new BitString("1f11d5d0355e0b556ccdb6c7f5083b4d")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffe000000"),
                    CipherText = new BitString("62526b78be79cb384633c91f83b4151b")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffff000000"),
                    CipherText = new BitString("90ddbcb950843592dd47bbef00fdc876")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffff800000"),
                    CipherText = new BitString("2fd0e41c5b8402277354a7391d2618e2")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffc00000"),
                    CipherText = new BitString("3cdf13e72dee4c581bafec70b85f9660")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffe00000"),
                    CipherText = new BitString("afa2ffc137577092e2b654fa199d2c43")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffff00000"),
                    CipherText = new BitString("8d683ee63e60d208e343ce48dbc44cac")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffff80000"),
                    CipherText = new BitString("705a4ef8ba2133729c20185c3d3a4763")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffc0000"),
                    CipherText = new BitString("0861a861c3db4e94194211b77ed761b9")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffe0000"),
                    CipherText = new BitString("4b00c27e8b26da7eab9d3a88dec8b031")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffff0000"),
                    CipherText = new BitString("5f397bf03084820cc8810d52e5b666e9")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffff8000"),
                    CipherText = new BitString("63fafabb72c07bfbd3ddc9b1203104b8")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffffc000"),
                    CipherText = new BitString("683e2140585b18452dd4ffbb93c95df9")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffffe000"),
                    CipherText = new BitString("286894e48e537f8763b56707d7d155c8")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffff000"),
                    CipherText = new BitString("a423deabc173dcf7e2c4c53e77d37cd1")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffff800"),
                    CipherText = new BitString("eb8168313e1cfdfdb5e986d5429cf172")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffffc00"),
                    CipherText = new BitString("27127daafc9accd2fb334ec3eba52323")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffffe00"),
                    CipherText = new BitString("ee0715b96f72e3f7a22a5064fc592f4c")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffffff00"),
                    CipherText = new BitString("29ee526770f2a11dcfa989d1ce88830f")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffffff80"),
                    CipherText = new BitString("0493370e054b09871130fe49af730a5a")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffffffc0"),
                    CipherText = new BitString("9b7b940f6c509f9e44a4ee140448ee46")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffffffe0"),
                    CipherText = new BitString("2915be4a1ecfdcbe3e023811a12bb6c7")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffffff0"),
                    CipherText = new BitString("7240e524bc51d8c4d440b1be55d1062c")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffffff8"),
                    CipherText = new BitString("da63039d38cb4612b2dc36ba26684b93")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffffffc"),
                    CipherText = new BitString("0f59cb5a4b522e2ac56c1a64f558ad9a")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("fffffffffffffffffffffffffffffffe"),
                    CipherText = new BitString("7bfe9d876c6d63c1d035da8fe21c409d")
                },
                new AlgoArrayResponse()
                {

                    Key = key,
                    PlainText = new BitString("ffffffffffffffffffffffffffffffff"),
                    CipherText = new BitString("acdace8078a32b1a182bfa4987ca1347")
                },
            };
        }
        #endregion VarTxt

        #region VarKey
        public static List<AlgoArrayResponse> GetVarKey128BitKey()
        {
            int count = 0;
            BitString plainText = new BitString("00000000000000000000000000000000");

            return new List<AlgoArrayResponse>()
            {
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("80000000000000000000000000000000"),
                    CipherText = new BitString("0edd33d3c621e546455bd8ba1418bec8")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("c0000000000000000000000000000000"),
                    CipherText = new BitString("4bc3f883450c113c64ca42e1112a9e87")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("e0000000000000000000000000000000"),
                    CipherText = new BitString("72a1da770f5d7ac4c9ef94d822affd97")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("f0000000000000000000000000000000"),
                    CipherText = new BitString("970014d634e2b7650777e8e84d03ccd8")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("f8000000000000000000000000000000"),
                    CipherText = new BitString("f17e79aed0db7e279e955b5f493875a7")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fc000000000000000000000000000000"),
                    CipherText = new BitString("9ed5a75136a940d0963da379db4af26a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fe000000000000000000000000000000"),
                    CipherText = new BitString("c4295f83465c7755e8fa364bac6a7ea5")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ff000000000000000000000000000000"),
                    CipherText = new BitString("b1d758256b28fd850ad4944208cf1155")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ff800000000000000000000000000000"),
                    CipherText = new BitString("42ffb34c743de4d88ca38011c990890b")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffc00000000000000000000000000000"),
                    CipherText = new BitString("9958f0ecea8b2172c0c1995f9182c0f3")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffe00000000000000000000000000000"),
                    CipherText = new BitString("956d7798fac20f82a8823f984d06f7f5")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fff00000000000000000000000000000"),
                    CipherText = new BitString("a01bf44f2d16be928ca44aaf7b9b106b")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fff80000000000000000000000000000"),
                    CipherText = new BitString("b5f1a33e50d40d103764c76bd4c6b6f8")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffc0000000000000000000000000000"),
                    CipherText = new BitString("2637050c9fc0d4817e2d69de878aee8d")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffe0000000000000000000000000000"),
                    CipherText = new BitString("113ecbe4a453269a0dd26069467fb5b5")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffff0000000000000000000000000000"),
                    CipherText = new BitString("97d0754fe68f11b9e375d070a608c884")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffff8000000000000000000000000000"),
                    CipherText = new BitString("c6a0b3e998d05068a5399778405200b4")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffc000000000000000000000000000"),
                    CipherText = new BitString("df556a33438db87bc41b1752c55e5e49")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffe000000000000000000000000000"),
                    CipherText = new BitString("90fb128d3a1af6e548521bb962bf1f05")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffff000000000000000000000000000"),
                    CipherText = new BitString("26298e9c1db517c215fadfb7d2a8d691")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffff800000000000000000000000000"),
                    CipherText = new BitString("a6cb761d61f8292d0df393a279ad0380")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffc00000000000000000000000000"),
                    CipherText = new BitString("12acd89b13cd5f8726e34d44fd486108")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffe00000000000000000000000000"),
                    CipherText = new BitString("95b1703fc57ba09fe0c3580febdd7ed4")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffff00000000000000000000000000"),
                    CipherText = new BitString("de11722d893e9f9121c381becc1da59a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffff80000000000000000000000000"),
                    CipherText = new BitString("6d114ccb27bf391012e8974c546d9bf2")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffc0000000000000000000000000"),
                    CipherText = new BitString("5ce37e17eb4646ecfac29b9cc38d9340")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffe0000000000000000000000000"),
                    CipherText = new BitString("18c1b6e2157122056d0243d8a165cddb")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffff0000000000000000000000000"),
                    CipherText = new BitString("99693e6a59d1366c74d823562d7e1431")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffff8000000000000000000000000"),
                    CipherText = new BitString("6c7c64dc84a8bba758ed17eb025a57e3")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffc000000000000000000000000"),
                    CipherText = new BitString("e17bc79f30eaab2fac2cbbe3458d687a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffe000000000000000000000000"),
                    CipherText = new BitString("1114bc2028009b923f0b01915ce5e7c4")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffff000000000000000000000000"),
                    CipherText = new BitString("9c28524a16a1e1c1452971caa8d13476")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffff800000000000000000000000"),
                    CipherText = new BitString("ed62e16363638360fdd6ad62112794f0")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffc00000000000000000000000"),
                    CipherText = new BitString("5a8688f0b2a2c16224c161658ffd4044")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffe00000000000000000000000"),
                    CipherText = new BitString("23f710842b9bb9c32f26648c786807ca")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffff00000000000000000000000"),
                    CipherText = new BitString("44a98bf11e163f632c47ec6a49683a89")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffff80000000000000000000000"),
                    CipherText = new BitString("0f18aff94274696d9b61848bd50ac5e5")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffc0000000000000000000000"),
                    CipherText = new BitString("82408571c3e2424540207f833b6dda69")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffe0000000000000000000000"),
                    CipherText = new BitString("303ff996947f0c7d1f43c8f3027b9b75")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffff0000000000000000000000"),
                    CipherText = new BitString("7df4daf4ad29a3615a9b6ece5c99518a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffff8000000000000000000000"),
                    CipherText = new BitString("c72954a48d0774db0b4971c526260415")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffc000000000000000000000"),
                    CipherText = new BitString("1df9b76112dc6531e07d2cfda04411f0")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffe000000000000000000000"),
                    CipherText = new BitString("8e4d8e699119e1fc87545a647fb1d34f")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffff000000000000000000000"),
                    CipherText = new BitString("e6c4807ae11f36f091c57d9fb68548d1")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffff800000000000000000000"),
                    CipherText = new BitString("8ebf73aad49c82007f77a5c1ccec6ab4")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffc00000000000000000000"),
                    CipherText = new BitString("4fb288cc2040049001d2c7585ad123fc")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffe00000000000000000000"),
                    CipherText = new BitString("04497110efb9dceb13e2b13fb4465564")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffff00000000000000000000"),
                    CipherText = new BitString("75550e6cb5a88e49634c9ab69eda0430")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffff80000000000000000000"),
                    CipherText = new BitString("b6768473ce9843ea66a81405dd50b345")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffc0000000000000000000"),
                    CipherText = new BitString("cb2f430383f9084e03a653571e065de6")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffe0000000000000000000"),
                    CipherText = new BitString("ff4e66c07bae3e79fb7d210847a3b0ba")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffff0000000000000000000"),
                    CipherText = new BitString("7b90785125505fad59b13c186dd66ce3")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffff8000000000000000000"),
                    CipherText = new BitString("8b527a6aebdaec9eaef8eda2cb7783e5")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffc000000000000000000"),
                    CipherText = new BitString("43fdaf53ebbc9880c228617d6a9b548b")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffe000000000000000000"),
                    CipherText = new BitString("53786104b9744b98f052c46f1c850d0b")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffff000000000000000000"),
                    CipherText = new BitString("b5ab3013dd1e61df06cbaf34ca2aee78")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffff800000000000000000"),
                    CipherText = new BitString("7470469be9723030fdcc73a8cd4fbb10")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffc00000000000000000"),
                    CipherText = new BitString("a35a63f5343ebe9ef8167bcb48ad122e")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffe00000000000000000"),
                    CipherText = new BitString("fd8687f0757a210e9fdf181204c30863")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffff00000000000000000"),
                    CipherText = new BitString("7a181e84bd5457d26a88fbae96018fb0")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffff80000000000000000"),
                    CipherText = new BitString("653317b9362b6f9b9e1a580e68d494b5")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffc0000000000000000"),
                    CipherText = new BitString("995c9dc0b689f03c45867b5faa5c18d1")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffe0000000000000000"),
                    CipherText = new BitString("77a4d96d56dda398b9aabecfc75729fd")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffff0000000000000000"),
                    CipherText = new BitString("84be19e053635f09f2665e7bae85b42d")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffff8000000000000000"),
                    CipherText = new BitString("32cd652842926aea4aa6137bb2be2b5e")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffc000000000000000"),
                    CipherText = new BitString("493d4a4f38ebb337d10aa84e9171a554")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffe000000000000000"),
                    CipherText = new BitString("d9bff7ff454b0ec5a4a2a69566e2cb84")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffff000000000000000"),
                    CipherText = new BitString("3535d565ace3f31eb249ba2cc6765d7a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffff800000000000000"),
                    CipherText = new BitString("f60e91fc3269eecf3231c6e9945697c6")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffc00000000000000"),
                    CipherText = new BitString("ab69cfadf51f8e604d9cc37182f6635a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffe00000000000000"),
                    CipherText = new BitString("7866373f24a0b6ed56e0d96fcdafb877")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffff00000000000000"),
                    CipherText = new BitString("1ea448c2aac954f5d812e9d78494446a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffff80000000000000"),
                    CipherText = new BitString("acc5599dd8ac02239a0fef4a36dd1668")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffc0000000000000"),
                    CipherText = new BitString("d8764468bb103828cf7e1473ce895073")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffe0000000000000"),
                    CipherText = new BitString("1b0d02893683b9f180458e4aa6b73982")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffff0000000000000"),
                    CipherText = new BitString("96d9b017d302df410a937dcdb8bb6e43")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffff8000000000000"),
                    CipherText = new BitString("ef1623cc44313cff440b1594a7e21cc6")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffc000000000000"),
                    CipherText = new BitString("284ca2fa35807b8b0ae4d19e11d7dbd7")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffe000000000000"),
                    CipherText = new BitString("f2e976875755f9401d54f36e2a23a594")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffff000000000000"),
                    CipherText = new BitString("ec198a18e10e532403b7e20887c8dd80")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffff800000000000"),
                    CipherText = new BitString("545d50ebd919e4a6949d96ad47e46a80")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffc00000000000"),
                    CipherText = new BitString("dbdfb527060e0a71009c7bb0c68f1d44")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffe00000000000"),
                    CipherText = new BitString("9cfa1322ea33da2173a024f2ff0d896d")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffff00000000000"),
                    CipherText = new BitString("8785b1a75b0f3bd958dcd0e29318c521")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffff80000000000"),
                    CipherText = new BitString("38f67b9e98e4a97b6df030a9fcdd0104")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffc0000000000"),
                    CipherText = new BitString("192afffb2c880e82b05926d0fc6c448b")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffe0000000000"),
                    CipherText = new BitString("6a7980ce7b105cf530952d74daaf798c")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffff0000000000"),
                    CipherText = new BitString("ea3695e1351b9d6858bd958cf513ef6c")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffff8000000000"),
                    CipherText = new BitString("6da0490ba0ba0343b935681d2cce5ba1")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffc000000000"),
                    CipherText = new BitString("f0ea23af08534011c60009ab29ada2f1")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffe000000000"),
                    CipherText = new BitString("ff13806cf19cc38721554d7c0fcdcd4b")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffff000000000"),
                    CipherText = new BitString("6838af1f4f69bae9d85dd188dcdf0688")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffff800000000"),
                    CipherText = new BitString("36cf44c92d550bfb1ed28ef583ddf5d7")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffc00000000"),
                    CipherText = new BitString("d06e3195b5376f109d5c4ec6c5d62ced")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffe00000000"),
                    CipherText = new BitString("c440de014d3d610707279b13242a5c36")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffff00000000"),
                    CipherText = new BitString("f0c5c6ffa5e0bd3a94c88f6b6f7c16b9")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffff80000000"),
                    CipherText = new BitString("3e40c3901cd7effc22bffc35dee0b4d9")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffc0000000"),
                    CipherText = new BitString("b63305c72bedfab97382c406d0c49bc6")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffe0000000"),
                    CipherText = new BitString("36bbaab22a6bd4925a99a2b408d2dbae")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffff0000000"),
                    CipherText = new BitString("307c5b8fcd0533ab98bc51e27a6ce461")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffff8000000"),
                    CipherText = new BitString("829c04ff4c07513c0b3ef05c03e337b5")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffc000000"),
                    CipherText = new BitString("f17af0e895dda5eb98efc68066e84c54")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffe000000"),
                    CipherText = new BitString("277167f3812afff1ffacb4a934379fc3")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffff000000"),
                    CipherText = new BitString("2cb1dc3a9c72972e425ae2ef3eb597cd")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffff800000"),
                    CipherText = new BitString("36aeaa3a213e968d4b5b679d3a2c97fe")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffc00000"),
                    CipherText = new BitString("9241daca4fdd034a82372db50e1a0f3f")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffe00000"),
                    CipherText = new BitString("c14574d9cd00cf2b5a7f77e53cd57885")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffff00000"),
                    CipherText = new BitString("793de39236570aba83ab9b737cb521c9")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffff80000"),
                    CipherText = new BitString("16591c0f27d60e29b85a96c33861a7ef")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffc0000"),
                    CipherText = new BitString("44fb5c4d4f5cb79be5c174a3b1c97348")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffe0000"),
                    CipherText = new BitString("674d2b61633d162be59dde04222f4740")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffff0000"),
                    CipherText = new BitString("b4750ff263a65e1f9e924ccfd98f3e37")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffff8000"),
                    CipherText = new BitString("62d0662d6eaeddedebae7f7ea3a4f6b6")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffc000"),
                    CipherText = new BitString("70c46bb30692be657f7eaa93ebad9897")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffe000"),
                    CipherText = new BitString("323994cfb9da285a5d9642e1759b224a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffff000"),
                    CipherText = new BitString("1dbf57877b7b17385c85d0b54851e371")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffff800"),
                    CipherText = new BitString("dfa5c097cdc1532ac071d57b1d28d1bd")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffc00"),
                    CipherText = new BitString("3a0c53fa37311fc10bd2a9981f513174")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffe00"),
                    CipherText = new BitString("ba4f970c0a25c41814bdae2e506be3b4")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffff00"),
                    CipherText = new BitString("2dce3acb727cd13ccd76d425ea56e4f6")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffff80"),
                    CipherText = new BitString("5160474d504b9b3eefb68d35f245f4b3")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffc0"),
                    CipherText = new BitString("41a8a947766635dec37553d9a6c0cbb7")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffe0"),
                    CipherText = new BitString("25d6cfe6881f2bf497dd14cd4ddf445b")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffff0"),
                    CipherText = new BitString("41c78c135ed9e98c096640647265da1e")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffff8"),
                    CipherText = new BitString("5a4d404d8917e353e92a21072c3b2305")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffc"),
                    CipherText = new BitString("02bc96846b3fdc71643f384cd3cc3eaf")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffe"),
                    CipherText = new BitString("9ba4a9143f4e5d4048521c4f8877d88e")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffff"),
                    CipherText = new BitString("a1f6258c877d5fcd8964484538bfc92c")
                },
            };
        }

        public static List<AlgoArrayResponse> GetVarKey192BitKey()
        {
            int count = 0;
            BitString plainText = new BitString("00000000000000000000000000000000");

            return new List<AlgoArrayResponse>()
            {
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("800000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("de885dc87f5a92594082d02cc1e1b42c")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("c00000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("132b074e80f2a597bf5febd8ea5da55e")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("e00000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("6eccedf8de592c22fb81347b79f2db1f")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("f00000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("180b09f267c45145db2f826c2582d35c")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("f80000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("edd807ef7652d7eb0e13c8b5e15b3bc0")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fc0000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("9978bcf8dd8fd72241223ad24b31b8a4")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fe0000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("5310f654343e8f27e12c83a48d24ff81")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ff0000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("833f71258d53036b02952c76c744f5a1")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ff8000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("eba83ff200cff9318a92f8691a06b09f")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffc000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("ff620ccbe9f3292abdf2176b09f04eba")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffe000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("7ababc4b3f516c9aafb35f4140b548f9")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fff000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("aa187824d9c4582b0916493ecbde8c57")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fff800000000000000000000000000000000000000000000"),
                    CipherText = new BitString("1c0ad553177fd5ea1092c9d626a29dc4")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffc00000000000000000000000000000000000000000000"),
                    CipherText = new BitString("a5dc46c37261194124ecaebd680408ec")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffe00000000000000000000000000000000000000000000"),
                    CipherText = new BitString("e4f2f2ae23e9b10bacfa58601531ba54")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffff00000000000000000000000000000000000000000000"),
                    CipherText = new BitString("b7d67cf1a1e91e8ff3a57a172c7bf412")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffff80000000000000000000000000000000000000000000"),
                    CipherText = new BitString("26706be06967884e847d137128ce47b3")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffc0000000000000000000000000000000000000000000"),
                    CipherText = new BitString("b2f8b409b0585909aad3a7b5a219072a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffe0000000000000000000000000000000000000000000"),
                    CipherText = new BitString("5e4b7bff0290c78344c54a23b722cd20")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffff0000000000000000000000000000000000000000000"),
                    CipherText = new BitString("07093657552d4414227ce161e9ebf7dd")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffff8000000000000000000000000000000000000000000"),
                    CipherText = new BitString("e1af1e7d8bc225ed4dffb771ecbb9e67")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffc000000000000000000000000000000000000000000"),
                    CipherText = new BitString("ef6555253635d8432156cfd9c11b145a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffe000000000000000000000000000000000000000000"),
                    CipherText = new BitString("fb4035074a5d4260c90cbd6da6c3fceb")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffff000000000000000000000000000000000000000000"),
                    CipherText = new BitString("446ee416f9ad1c103eb0cc96751c88e1")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffff800000000000000000000000000000000000000000"),
                    CipherText = new BitString("198ae2a4637ac0a7890a8fd1485445c9")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffc00000000000000000000000000000000000000000"),
                    CipherText = new BitString("562012ec8faded0825fb2fa70ab30cbd")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffe00000000000000000000000000000000000000000"),
                    CipherText = new BitString("cc8a64b46b5d88bf7f247d4dbaf38f05")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffff00000000000000000000000000000000000000000"),
                    CipherText = new BitString("a168253762e2cc81b42d1e5001762699")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffff80000000000000000000000000000000000000000"),
                    CipherText = new BitString("1b41f83b38ce5032c6cd7af98cf62061")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffc0000000000000000000000000000000000000000"),
                    CipherText = new BitString("61a89990cd1411750d5fb0dc988447d4")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffe0000000000000000000000000000000000000000"),
                    CipherText = new BitString("b5accc8ed629edf8c68a539183b1ea82")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffff0000000000000000000000000000000000000000"),
                    CipherText = new BitString("b16fa71f846b81a13f361c43a851f290")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffff8000000000000000000000000000000000000000"),
                    CipherText = new BitString("4fad6efdff5975aee7692234bcd54488")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffc000000000000000000000000000000000000000"),
                    CipherText = new BitString("ebfdb05a783d03082dfe5fdd80a00b17")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffe000000000000000000000000000000000000000"),
                    CipherText = new BitString("eb81b584766997af6ba5529d3bdd8609")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffff000000000000000000000000000000000000000"),
                    CipherText = new BitString("0cf4ff4f49c8a0ca060c443499e29313")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffff800000000000000000000000000000000000000"),
                    CipherText = new BitString("cc4ba8a8e029f8b26d8afff9df133bb6")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffc00000000000000000000000000000000000000"),
                    CipherText = new BitString("fefebf64360f38e4e63558f0ffc550c3")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffe00000000000000000000000000000000000000"),
                    CipherText = new BitString("12ad98cbf725137d6a8108c2bed99322")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffff00000000000000000000000000000000000000"),
                    CipherText = new BitString("6afaa996226198b3e2610413ce1b3f78")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffff80000000000000000000000000000000000000"),
                    CipherText = new BitString("2a8ce6747a7e39367828e290848502d9")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffc0000000000000000000000000000000000000"),
                    CipherText = new BitString("223736e8b8f89ca1e37b6deab40facf1")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffe0000000000000000000000000000000000000"),
                    CipherText = new BitString("c0f797e50418b95fa6013333917a9480")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffff0000000000000000000000000000000000000"),
                    CipherText = new BitString("a758de37c2ece2a02c73c01fedc9a132")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffff8000000000000000000000000000000000000"),
                    CipherText = new BitString("3a9b87ae77bae706803966c66c73adbd")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffc000000000000000000000000000000000000"),
                    CipherText = new BitString("d365ab8df8ffd782e358121a4a4fc541")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffe000000000000000000000000000000000000"),
                    CipherText = new BitString("c8dcd9e6f75e6c36c8daee0466f0ed74")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffff000000000000000000000000000000000000"),
                    CipherText = new BitString("c79a637beb1c0304f14014c037e736dd")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffff800000000000000000000000000000000000"),
                    CipherText = new BitString("105f0a25e84ac930d996281a5f954dd9")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffc00000000000000000000000000000000000"),
                    CipherText = new BitString("42e4074b2927973e8d17ffa92f7fe615")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffe00000000000000000000000000000000000"),
                    CipherText = new BitString("4fe2a9d2c1824449c69e3e0398f12963")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffff00000000000000000000000000000000000"),
                    CipherText = new BitString("b7f29c1e1f62847a15253b28a1e9d712")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffff80000000000000000000000000000000000"),
                    CipherText = new BitString("36ed5d29b903f31e8983ef8b0a2bf990")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffc0000000000000000000000000000000000"),
                    CipherText = new BitString("27b8070270810f9d023f9dd7ff3b4aa2")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffe0000000000000000000000000000000000"),
                    CipherText = new BitString("94d46e155c1228f61d1a0db4815ecc4b")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffff0000000000000000000000000000000000"),
                    CipherText = new BitString("ca6108d1d98071428eeceef1714b96dd")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffff8000000000000000000000000000000000"),
                    CipherText = new BitString("dc5b25b71b6296cf73dd2cdcac2f70b1")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffc000000000000000000000000000000000"),
                    CipherText = new BitString("44aba95e8a06a2d9d3530d2677878c80")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffe000000000000000000000000000000000"),
                    CipherText = new BitString("a570d20e89b467e8f5176061b81dd396")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffff000000000000000000000000000000000"),
                    CipherText = new BitString("758f4467a5d8f1e7307dc30b34e404f4")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffff800000000000000000000000000000000"),
                    CipherText = new BitString("bcea28e9071b5a2302970ff352451bc5")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffc00000000000000000000000000000000"),
                    CipherText = new BitString("7523c00bc177d331ad312e09c9015c1c")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffe00000000000000000000000000000000"),
                    CipherText = new BitString("ccac61e3183747b3f5836da21a1bc4f4")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffff00000000000000000000000000000000"),
                    CipherText = new BitString("707b075791878880b44189d3522b8c30")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffff80000000000000000000000000000000"),
                    CipherText = new BitString("7132d0c0e4a07593cf12ebb12be7688c")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffc0000000000000000000000000000000"),
                    CipherText = new BitString("effbac1644deb0c784275fe56e19ead3")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffe0000000000000000000000000000000"),
                    CipherText = new BitString("a005063f30f4228b374e2459738f26bb")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffff0000000000000000000000000000000"),
                    CipherText = new BitString("29975b5f48bb68fcbbc7cea93b452ed7")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffff8000000000000000000000000000000"),
                    CipherText = new BitString("cf3f2576e2afedc74bb1ca7eeec1c0e7")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffc000000000000000000000000000000"),
                    CipherText = new BitString("07c403f5f966e0e3d9f296d6226dca28")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffe000000000000000000000000000000"),
                    CipherText = new BitString("c8c20908249ab4a34d6dd0a31327ff1a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffff000000000000000000000000000000"),
                    CipherText = new BitString("c0541329ecb6159ab23b7fc5e6a21bca")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffff800000000000000000000000000000"),
                    CipherText = new BitString("7aa1acf1a2ed9ba72bc6deb31d88b863")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffc00000000000000000000000000000"),
                    CipherText = new BitString("808bd8eddabb6f3bf0d5a8a27be1fe8a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffe00000000000000000000000000000"),
                    CipherText = new BitString("273c7d7685e14ec66bbb96b8f05b6ddd")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffff00000000000000000000000000000"),
                    CipherText = new BitString("32752eefc8c2a93f91b6e73eb07cca6e")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffff80000000000000000000000000000"),
                    CipherText = new BitString("d893e7d62f6ce502c64f75e281f9c000")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffc0000000000000000000000000000"),
                    CipherText = new BitString("8dfd999be5d0cfa35732c0ddc88ff5a5")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffe0000000000000000000000000000"),
                    CipherText = new BitString("02647c76a300c3173b841487eb2bae9f")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffff0000000000000000000000000000"),
                    CipherText = new BitString("172df8b02f04b53adab028b4e01acd87")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffff8000000000000000000000000000"),
                    CipherText = new BitString("054b3bf4998aeb05afd87ec536533a36")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffc000000000000000000000000000"),
                    CipherText = new BitString("3783f7bf44c97f065258a666cae03020")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffe000000000000000000000000000"),
                    CipherText = new BitString("aad4c8a63f80954104de7b92cede1be1")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffff000000000000000000000000000"),
                    CipherText = new BitString("cbfe61810fd5467ccdacb75800f3ac07")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffff800000000000000000000000000"),
                    CipherText = new BitString("830d8a2590f7d8e1b55a737f4af45f34")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffc00000000000000000000000000"),
                    CipherText = new BitString("fffcd4683f858058e74314671d43fa2c")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffe00000000000000000000000000"),
                    CipherText = new BitString("523d0babbb82f46ebc9e70b1cd41ddd0")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffff00000000000000000000000000"),
                    CipherText = new BitString("344aab37080d7486f7d542a309e53eed")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffff80000000000000000000000000"),
                    CipherText = new BitString("56c5609d0906b23ab9caca816f5dbebd")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffc0000000000000000000000000"),
                    CipherText = new BitString("7026026eedd91adc6d831cdf9894bdc6")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffe0000000000000000000000000"),
                    CipherText = new BitString("88330baa4f2b618fc9d9b021bf503d5a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffff0000000000000000000000000"),
                    CipherText = new BitString("fc9e0ea22480b0bac935c8a8ebefcdcf")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffff8000000000000000000000000"),
                    CipherText = new BitString("29ca779f398fb04f867da7e8a44756cb")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffc000000000000000000000000"),
                    CipherText = new BitString("51f89c42985786bfc43c6df8ada36832")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffe000000000000000000000000"),
                    CipherText = new BitString("6ac1de5fb8f21d874e91c53b560c50e3")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffff000000000000000000000000"),
                    CipherText = new BitString("03aa9058490eda306001a8a9f48d0ca7")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffff800000000000000000000000"),
                    CipherText = new BitString("e34ec71d6128d4871865d617c30b37e3")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffc00000000000000000000000"),
                    CipherText = new BitString("14be1c535b17cabd0c4d93529d69bf47")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffe00000000000000000000000"),
                    CipherText = new BitString("c9ef67756507beec9dd3862883478044")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffff00000000000000000000000"),
                    CipherText = new BitString("40e231fa5a5948ce2134e92fc0664d4b")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffff80000000000000000000000"),
                    CipherText = new BitString("03194b8e5dda5530d0c678c0b48f5d92")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffc0000000000000000000000"),
                    CipherText = new BitString("90bd086f237cc4fd99f4d76bde6b4826")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffe0000000000000000000000"),
                    CipherText = new BitString("19259761ca17130d6ed86d57cd7951ee")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffff0000000000000000000000"),
                    CipherText = new BitString("d7cbb3f34b9b450f24b0e8518e54da6d")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffff8000000000000000000000"),
                    CipherText = new BitString("725b9caebe9f7f417f4068d0d2ee20b3")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffc000000000000000000000"),
                    CipherText = new BitString("9d924b934a90ce1fd39b8a9794f82672")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffe000000000000000000000"),
                    CipherText = new BitString("c50562bf094526a91c5bc63c0c224995")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffff000000000000000000000"),
                    CipherText = new BitString("d2f11805046743bd74f57188d9188df7")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffff800000000000000000000"),
                    CipherText = new BitString("8dd274bd0f1b58ae345d9e7233f9b8f3")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffc00000000000000000000"),
                    CipherText = new BitString("9d6bdc8f4ce5feb0f3bed2e4b9a9bb0b")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffe00000000000000000000"),
                    CipherText = new BitString("fd5548bcf3f42565f7efa94562528d46")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffff00000000000000000000"),
                    CipherText = new BitString("d2ccaebd3a4c3e80b063748131ba4a71")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffff80000000000000000000"),
                    CipherText = new BitString("e03cb23d9e11c9d93f117e9c0a91b576")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffc0000000000000000000"),
                    CipherText = new BitString("78f933a2081ac1db84f69d10f4523fe0")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffe0000000000000000000"),
                    CipherText = new BitString("4061f7412ed320de0edc8851c2e2436f")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffff0000000000000000000"),
                    CipherText = new BitString("9064ba1cd04ce6bab98474330814b4d4")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffff8000000000000000000"),
                    CipherText = new BitString("48391bffb9cfff80ac238c886ef0a461")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffc000000000000000000"),
                    CipherText = new BitString("b8d2a67df5a999fdbf93edd0343296c9")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffe000000000000000000"),
                    CipherText = new BitString("aaca7367396b69a221bd632bea386eec")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffff000000000000000000"),
                    CipherText = new BitString("a80fd5020dfe65f5f16293ec92c6fd89")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffff800000000000000000"),
                    CipherText = new BitString("2162995b8217a67f1abc342e146406f8")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffc00000000000000000"),
                    CipherText = new BitString("c6a6164b7a60bae4e986ffac28dfadd9")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffe00000000000000000"),
                    CipherText = new BitString("64e0d7f900e3d9c83e4b8f96717b2146")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffff00000000000000000"),
                    CipherText = new BitString("1ad2561de8c1232f5d8dbab4739b6cbb")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffff80000000000000000"),
                    CipherText = new BitString("279689e9a557f58b1c3bf40c97a90964")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffc0000000000000000"),
                    CipherText = new BitString("c4637e4a5e6377f9cc5a8638045de029")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffe0000000000000000"),
                    CipherText = new BitString("492e607e5aea4688594b45f3aee3df90")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffff0000000000000000"),
                    CipherText = new BitString("e8c4e4381feec74054954c05b777a00a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffff8000000000000000"),
                    CipherText = new BitString("91549514605f38246c9b724ad839f01d")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffc000000000000000"),
                    CipherText = new BitString("74b24e3b6fefe40a4f9ef7ac6e44d76a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffe000000000000000"),
                    CipherText = new BitString("2437a683dc5d4b52abb4a123a8df86c6")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffff000000000000000"),
                    CipherText = new BitString("bb2852c891c5947d2ed44032c421b85f")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffff800000000000000"),
                    CipherText = new BitString("1b9f5fbd5e8a4264c0a85b80409afa5e")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffc00000000000000"),
                    CipherText = new BitString("30dab809f85a917fe924733f424ac589")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffe00000000000000"),
                    CipherText = new BitString("eaef5c1f8d605192646695ceadc65f32")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffff00000000000000"),
                    CipherText = new BitString("b8aa90040b4c15a12316b78e0f9586fc")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffff80000000000000"),
                    CipherText = new BitString("97fac8297ceaabc87d454350601e0673")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffc0000000000000"),
                    CipherText = new BitString("9b47ef567ac28dfe488492f157e2b2e0")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffe0000000000000"),
                    CipherText = new BitString("1b8426027ddb962b5c5ba7eb8bc9ab63")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffff0000000000000"),
                    CipherText = new BitString("e917fc77e71992a12dbe4c18068bec82")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffff8000000000000"),
                    CipherText = new BitString("dceebbc98840f8ae6daf76573b7e56f4")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffc000000000000"),
                    CipherText = new BitString("4e11a9f74205125b61e0aee047eca20d")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffe000000000000"),
                    CipherText = new BitString("f60467f55a1f17eab88e800120cbc284")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffff000000000000"),
                    CipherText = new BitString("d436649f600b449ee276530f0cd83c11")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffff800000000000"),
                    CipherText = new BitString("3bc0e3656a9e3ac7cd378a737f53b637")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffc00000000000"),
                    CipherText = new BitString("6bacae63d33b928aa8380f8d54d88c17")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffe00000000000"),
                    CipherText = new BitString("8935ffbc75ae6251bf8e859f085adcb9")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffff00000000000"),
                    CipherText = new BitString("93dc4970fe35f67747cb0562c06d875a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffff80000000000"),
                    CipherText = new BitString("14f9df858975851797ba604fb0d16cc7")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffc0000000000"),
                    CipherText = new BitString("02ea0c98dca10b38c21b3b14e8d1b71f")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffe0000000000"),
                    CipherText = new BitString("8f091b1b5b0749b2adc803e63dda9b72")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffff0000000000"),
                    CipherText = new BitString("05b389e3322c6da08384345a4137fd08")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffff8000000000"),
                    CipherText = new BitString("381308c438f35b399f10ad71b05027d8")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffc000000000"),
                    CipherText = new BitString("68c230fcfa9279c3409fc423e2acbe04")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffe000000000"),
                    CipherText = new BitString("1c84a475acb011f3f59f4f46b76274c0")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffff000000000"),
                    CipherText = new BitString("45119b68cb3f8399ee60066b5611a4d7")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffff800000000"),
                    CipherText = new BitString("9423762f527a4060ffca312dcca22a16")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffc00000000"),
                    CipherText = new BitString("f361a2745a33f056a5ac6ace2f08e344")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffe00000000"),
                    CipherText = new BitString("5ef145766eca849f5d011536a6557fdb")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffff00000000"),
                    CipherText = new BitString("c9af27b2c89c9b4cf4a0c4106ac80318")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffff80000000"),
                    CipherText = new BitString("fb9c4f16c621f4eab7e9ac1d7551dd57")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffc0000000"),
                    CipherText = new BitString("138e06fba466fa70854d8c2e524cffb2")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffe0000000"),
                    CipherText = new BitString("fb4bc78b225070773f04c40466d4e90c")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffff0000000"),
                    CipherText = new BitString("8b2cbff1ed0150feda8a4799be94551f")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffff8000000"),
                    CipherText = new BitString("08b30d7b3f27962709a36bcadfb974bd")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffc000000"),
                    CipherText = new BitString("fdf6d32e044d77adcf37fb97ac213326")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffe000000"),
                    CipherText = new BitString("93cb284ecdcfd781a8afe32077949e88")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffff000000"),
                    CipherText = new BitString("7b017bb02ec87b2b94c96e40a26fc71a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffff800000"),
                    CipherText = new BitString("c5c038b6990664ab08a3aaa5df9f3266")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffc00000"),
                    CipherText = new BitString("4b7020be37fab6259b2a27f4ec551576")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffe00000"),
                    CipherText = new BitString("60136703374f64e860b48ce31f930716")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffff00000"),
                    CipherText = new BitString("8d63a269b14d506ccc401ab8a9f1b591")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffff80000"),
                    CipherText = new BitString("d317f81dc6aa454aee4bd4a5a5cff4bd")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffc0000"),
                    CipherText = new BitString("dddececd5354f04d530d76ed884246eb")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffe0000"),
                    CipherText = new BitString("41c5205cc8fd8eda9a3cffd2518f365a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffff0000"),
                    CipherText = new BitString("cf42fb474293d96eca9db1b37b1ba676")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffff8000"),
                    CipherText = new BitString("a231692607169b4ecdead5cd3b10db3e")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffc000"),
                    CipherText = new BitString("ace4b91c9c669e77e7acacd19859ed49")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffe000"),
                    CipherText = new BitString("75db7cfd4a7b2b62ab78a48f3ddaf4af")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffff000"),
                    CipherText = new BitString("c1faba2d46e259cf480d7c38e4572a58")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffff800"),
                    CipherText = new BitString("241c45bc6ae16dee6eb7bea128701582")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffc00"),
                    CipherText = new BitString("8fd03057cf1364420c2b78069a3e2502")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffe00"),
                    CipherText = new BitString("ddb505e6cc1384cbaec1df90b80beb20")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffff00"),
                    CipherText = new BitString("5674a3bed27bf4bd3622f9f5fe208306")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffff80"),
                    CipherText = new BitString("b687f26a89cfbfbb8e5eeac54055315e")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffc0"),
                    CipherText = new BitString("0547dd32d3b29ab6a4caeb606c5b6f78")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffe0"),
                    CipherText = new BitString("186861f8bc5386d31fb77f720c3226e6")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffff0"),
                    CipherText = new BitString("eacf1e6c4224efb38900b185ab1dfd42")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffff8"),
                    CipherText = new BitString("d241aab05a42d319de81d874f5c7b90d")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffc"),
                    CipherText = new BitString("5eb9bc759e2ad8d2140a6c762ae9e1ab")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffe"),
                    CipherText = new BitString("018596e15e78e2c064159defce5f3085")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffff"),
                    CipherText = new BitString("dd8a493514231cbf56eccee4c40889fb")
                },
            };
        }

        public static List<AlgoArrayResponse> GetVarKey256BitKey()
        {
            int count = 0;
            BitString plainText = new BitString("00000000000000000000000000000000");

            return new List<AlgoArrayResponse>()
            {
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("8000000000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("e35a6dcb19b201a01ebcfa8aa22b5759")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("c000000000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("b29169cdcf2d83e838125a12ee6aa400")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("e000000000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("d8f3a72fc3cdf74dfaf6c3e6b97b2fa6")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("f000000000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("1c777679d50037c79491a94da76a9a35")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("f800000000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("9cf4893ecafa0a0247a898e040691559")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fc00000000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("8fbb413703735326310a269bd3aa94b2")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fe00000000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("60e32246bed2b0e859e55c1cc6b26502")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ff00000000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("ec52a212f80a09df6317021bc2a9819e")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ff80000000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("f23e5b600eb70dbccf6c0b1d9a68182c")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffc0000000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("a3f599d63a82a968c33fe26590745970")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffe0000000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("d1ccb9b1337002cbac42c520b5d67722")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fff0000000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("cc111f6c37cf40a1159d00fb59fb0488")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fff8000000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("dc43b51ab609052372989a26e9cdd714")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffc000000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4dcede8da9e2578f39703d4433dc6459")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffe000000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("1a4c1c263bbccfafc11782894685e3a8")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffff000000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("937ad84880db50613423d6d527a2823d")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffff800000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("610b71dfc688e150d8152c5b35ebc14d")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffc00000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("27ef2495dabf323885aab39c80f18d8b")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffe00000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("633cafea395bc03adae3a1e2068e4b4e")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffff00000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("6e1b482b53761cf631819b749a6f3724")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffff80000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("976e6f851ab52c771998dbb2d71c75a9")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffc0000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("85f2ba84f8c307cf525e124c3e22e6cc")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffe0000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("6bcca98bf6a835fa64955f72de4115fe")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffff0000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2c75e2d36eebd65411f14fd0eb1d2a06")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffff8000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("bd49295006250ffca5100b6007a0eade")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffc000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("a190527d0ef7c70f459cd3940df316ec")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffe000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("bbd1097a62433f79449fa97d4ee80dbf")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffff000000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("07058e408f5b99b0e0f061a1761b5b3b")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffff800000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("5fd1f13fa0f31e37fabde328f894eac2")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffc00000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("fc4af7c948df26e2ef3e01c1ee5b8f6f")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffe00000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("829fd7208fb92d44a074a677ee9861ac")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffff00000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("ad9fc613a703251b54c64a0e76431711")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffff80000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("33ac9eccc4cc75e2711618f80b1548e8")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffc0000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2025c74b8ad8f4cda17ee2049c4c902d")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffe0000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("f85ca05fe528f1ce9b790166e8d551e7")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffff0000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("6f6238d8966048d4967154e0dad5a6c9")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffff8000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("f2b21b4e7640a9b3346de8b82fb41e49")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffc000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("f836f251ad1d11d49dc344628b1884e1")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffe000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("077e9470ae7abea5a9769d49182628c3")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffff000000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("e0dcc2d27fc9865633f85223cf0d611f")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffff800000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("be66cfea2fecd6bf0ec7b4352c99bcaa")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffc00000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("df31144f87a2ef523facdcf21a427804")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffe00000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("b5bb0f5629fb6aae5e1839a3c3625d63")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffff00000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("3c9db3335306fe1ec612bdbfae6b6028")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffff80000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("3dd5c34634a79d3cfcc8339760e6f5f4")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffc0000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("82bda118a3ed7af314fa2ccc5c07b761")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffe0000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2937a64f7d4f46fe6fea3b349ec78e38")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffff0000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("225f068c28476605735ad671bb8f39f3")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffff8000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("ae682c5ecd71898e08942ac9aa89875c")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffc000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("5e031cb9d676c3022d7f26227e85c38f")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffe000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("a78463fb064db5d52bb64bfef64f2dda")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffff000000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("8aa9b75e784593876c53a00eae5af52b")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffff800000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("3f84566df23da48af692722fe980573a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffc00000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("31690b5ed41c7eb42a1e83270a7ff0e6")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffe00000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("77dd7702646d55f08365e477d3590eda")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffff00000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4c022ac62b3cb78d739cc67b3e20bb7e")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffff80000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("092fa137ce18b5dfe7906f550bb13370")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffc0000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("3e0cdadf2e68353c0027672c97144dd3")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffe0000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("d8c4b200b383fc1f2b2ea677618a1d27")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffff0000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("11825f99b0e9bb3477c1c0713b015aac")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffff8000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("f8b9fffb5c187f7ddc7ab10f4fb77576")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffc000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("ffb4e87a32b37d6f2c8328d3b5377802")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffe000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("d276c13a5d220f4da9224e74896391ce")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffff000000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("94efe7a0e2e031e2536da01df799c927")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffff800000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("8f8fd822680a85974e53a5a8eb9d38de")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffc00000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("e0f0a91b2e45f8cc37b7805a3042588d")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffe00000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("597a6252255e46d6364dbeeda31e279c")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffff00000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("f51a0f694442b8f05571797fec7ee8bf")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffff80000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("9ff071b165b5198a93dddeebc54d09b5")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffc0000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("c20a19fd5758b0c4bc1a5df89cf73877")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffe0000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("97120166307119ca2280e9315668e96f")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffff0000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4b3b9f1e099c2a09dc091e90e4f18f0a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffff8000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("eb040b891d4b37f6851f7ec219cd3f6d")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffc000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("9f0fdec08b7fd79aa39535bea42db92a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffe000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2e70f168fc74bf911df240bcd2cef236")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffff000000000000000000000000000000000000000000000"),
                    CipherText = new BitString("462ccd7f5fd1108dbc152f3cacad328b")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffff800000000000000000000000000000000000000000000"),
                    CipherText = new BitString("a4af534a7d0b643a01868785d86dfb95")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffc00000000000000000000000000000000000000000000"),
                    CipherText = new BitString("ab980296197e1a5022326c31da4bf6f3")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffe00000000000000000000000000000000000000000000"),
                    CipherText = new BitString("f97d57b3333b6281b07d486db2d4e20c")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffff00000000000000000000000000000000000000000000"),
                    CipherText = new BitString("f33fa36720231afe4c759ade6bd62eb6")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffff80000000000000000000000000000000000000000000"),
                    CipherText = new BitString("fdcfac0c02ca538343c68117e0a15938")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffc0000000000000000000000000000000000000000000"),
                    CipherText = new BitString("ad4916f5ee5772be764fc027b8a6e539")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffe0000000000000000000000000000000000000000000"),
                    CipherText = new BitString("2e16873e1678610d7e14c02d002ea845")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffff0000000000000000000000000000000000000000000"),
                    CipherText = new BitString("4e6e627c1acc51340053a8236d579576")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffff8000000000000000000000000000000000000000000"),
                    CipherText = new BitString("ab0c8410aeeead92feec1eb430d652cb")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffc000000000000000000000000000000000000000000"),
                    CipherText = new BitString("e86f7e23e835e114977f60e1a592202e")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffe000000000000000000000000000000000000000000"),
                    CipherText = new BitString("e68ad5055a367041fade09d9a70a794b")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffff000000000000000000000000000000000000000000"),
                    CipherText = new BitString("0791823a3c666bb6162825e78606a7fe")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffff800000000000000000000000000000000000000000"),
                    CipherText = new BitString("dcca366a9bf47b7b868b77e25c18a364")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffc00000000000000000000000000000000000000000"),
                    CipherText = new BitString("684c9efc237e4a442965f84bce20247a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffe00000000000000000000000000000000000000000"),
                    CipherText = new BitString("a858411ffbe63fdb9c8aa1bfaed67b52")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffff00000000000000000000000000000000000000000"),
                    CipherText = new BitString("04bc3da2179c3015498b0e03910db5b8")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffff80000000000000000000000000000000000000000"),
                    CipherText = new BitString("40071eeab3f935dbc25d00841460260f")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffc0000000000000000000000000000000000000000"),
                    CipherText = new BitString("0ebd7c30ed2016e08ba806ddb008bcc8")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffe0000000000000000000000000000000000000000"),
                    CipherText = new BitString("15c6becf0f4cec7129cbd22d1a79b1b8")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffff0000000000000000000000000000000000000000"),
                    CipherText = new BitString("0aeede5b91f721700e9e62edbf60b781")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffff8000000000000000000000000000000000000000"),
                    CipherText = new BitString("266581af0dcfbed1585e0a242c64b8df")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffc000000000000000000000000000000000000000"),
                    CipherText = new BitString("6693dc911662ae473216ba22189a511a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffe000000000000000000000000000000000000000"),
                    CipherText = new BitString("7606fa36d86473e6fb3a1bb0e2c0adf5")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffff000000000000000000000000000000000000000"),
                    CipherText = new BitString("112078e9e11fbb78e26ffb8899e96b9a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffff800000000000000000000000000000000000000"),
                    CipherText = new BitString("40b264e921e9e4a82694589ef3798262")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffc00000000000000000000000000000000000000"),
                    CipherText = new BitString("8d4595cb4fa7026715f55bd68e2882f9")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffe00000000000000000000000000000000000000"),
                    CipherText = new BitString("b588a302bdbc09197df1edae68926ed9")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffff00000000000000000000000000000000000000"),
                    CipherText = new BitString("33f7502390b8a4a221cfecd0666624ba")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffff80000000000000000000000000000000000000"),
                    CipherText = new BitString("3d20253adbce3be2373767c4d822c566")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffc0000000000000000000000000000000000000"),
                    CipherText = new BitString("a42734a3929bf84cf0116c9856a3c18c")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffe0000000000000000000000000000000000000"),
                    CipherText = new BitString("e3abc4939457422bb957da3c56938c6d")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffff0000000000000000000000000000000000000"),
                    CipherText = new BitString("972bdd2e7c525130fadc8f76fc6f4b3f")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffff8000000000000000000000000000000000000"),
                    CipherText = new BitString("84a83d7b94c699cbcb8a7d9b61f64093")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffc000000000000000000000000000000000000"),
                    CipherText = new BitString("ce61d63514aded03d43e6ebfc3a9001f")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffe000000000000000000000000000000000000"),
                    CipherText = new BitString("6c839dd58eeae6b8a36af48ed63d2dc9")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffff000000000000000000000000000000000000"),
                    CipherText = new BitString("cd5ece55b8da3bf622c4100df5de46f9")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffff800000000000000000000000000000000000"),
                    CipherText = new BitString("3b6f46f40e0ac5fc0a9c1105f800f48d")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffc00000000000000000000000000000000000"),
                    CipherText = new BitString("ba26d47da3aeb028de4fb5b3a854a24b")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffe00000000000000000000000000000000000"),
                    CipherText = new BitString("87f53bf620d3677268445212904389d5")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffff00000000000000000000000000000000000"),
                    CipherText = new BitString("10617d28b5e0f4605492b182a5d7f9f6")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffff80000000000000000000000000000000000"),
                    CipherText = new BitString("9aaec4fabbf6fae2a71feff02e372b39")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffc0000000000000000000000000000000000"),
                    CipherText = new BitString("3a90c62d88b5c42809abf782488ed130")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffe0000000000000000000000000000000000"),
                    CipherText = new BitString("f1f1c5a40899e15772857ccb65c7a09a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffff0000000000000000000000000000000000"),
                    CipherText = new BitString("190843d29b25a3897c692ce1dd81ee52")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffff8000000000000000000000000000000000"),
                    CipherText = new BitString("a866bc65b6941d86e8420a7ffb0964db")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffc000000000000000000000000000000000"),
                    CipherText = new BitString("8193c6ff85225ced4255e92f6e078a14")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffe000000000000000000000000000000000"),
                    CipherText = new BitString("9661cb2424d7d4a380d547f9e7ec1cb9")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffff000000000000000000000000000000000"),
                    CipherText = new BitString("86f93d9ec08453a071e2e2877877a9c8")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffff800000000000000000000000000000000"),
                    CipherText = new BitString("27eefa80ce6a4a9d598e3fec365434d2")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffc00000000000000000000000000000000"),
                    CipherText = new BitString("d62068444578e3ab39ce7ec95dd045dc")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffe00000000000000000000000000000000"),
                    CipherText = new BitString("b5f71d4dd9a71fe5d8bc8ba7e6ea3048")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffff00000000000000000000000000000000"),
                    CipherText = new BitString("6825a347ac479d4f9d95c5cb8d3fd7e9")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffff80000000000000000000000000000000"),
                    CipherText = new BitString("e3714e94a5778955cc0346358e94783a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffc0000000000000000000000000000000"),
                    CipherText = new BitString("d836b44bb29e0c7d89fa4b2d4b677d2a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffe0000000000000000000000000000000"),
                    CipherText = new BitString("5d454b75021d76d4b84f873a8f877b92")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffff0000000000000000000000000000000"),
                    CipherText = new BitString("c3498f7eced2095314fc28115885b33f")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffff8000000000000000000000000000000"),
                    CipherText = new BitString("6e668856539ad8e405bd123fe6c88530")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffc000000000000000000000000000000"),
                    CipherText = new BitString("8680db7f3a87b8605543cfdbe6754076")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffe000000000000000000000000000000"),
                    CipherText = new BitString("6c5d03b13069c3658b3179be91b0800c")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffff000000000000000000000000000000"),
                    CipherText = new BitString("ef1b384ac4d93eda00c92add0995ea5f")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffff800000000000000000000000000000"),
                    CipherText = new BitString("bf8115805471741bd5ad20a03944790f")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffc00000000000000000000000000000"),
                    CipherText = new BitString("c64c24b6894b038b3c0d09b1df068b0b")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffe00000000000000000000000000000"),
                    CipherText = new BitString("3967a10cffe27d0178545fbf6a40544b")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffff00000000000000000000000000000"),
                    CipherText = new BitString("7c85e9c95de1a9ec5a5363a8a053472d")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffff80000000000000000000000000000"),
                    CipherText = new BitString("a9eec03c8abec7ba68315c2c8c2316e0")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffc0000000000000000000000000000"),
                    CipherText = new BitString("cac8e414c2f388227ae14986fc983524")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffe0000000000000000000000000000"),
                    CipherText = new BitString("5d942b7f4622ce056c3ce3ce5f1dd9d6")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffff0000000000000000000000000000"),
                    CipherText = new BitString("d240d648ce21a3020282c3f1b528a0b6")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffff8000000000000000000000000000"),
                    CipherText = new BitString("45d089c36d5c5a4efc689e3b0de10dd5")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffc000000000000000000000000000"),
                    CipherText = new BitString("b4da5df4becb5462e03a0ed00d295629")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffe000000000000000000000000000"),
                    CipherText = new BitString("dcf4e129136c1a4b7a0f38935cc34b2b")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffff000000000000000000000000000"),
                    CipherText = new BitString("d9a4c7618b0ce48a3d5aee1a1c0114c4")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffff800000000000000000000000000"),
                    CipherText = new BitString("ca352df025c65c7b0bf306fbee0f36ba")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffc00000000000000000000000000"),
                    CipherText = new BitString("238aca23fd3409f38af63378ed2f5473")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffe00000000000000000000000000"),
                    CipherText = new BitString("59836a0e06a79691b36667d5380d8188")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffff00000000000000000000000000"),
                    CipherText = new BitString("33905080f7acf1cdae0a91fc3e85aee4")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffff80000000000000000000000000"),
                    CipherText = new BitString("72c9e4646dbc3d6320fc6689d93e8833")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffc0000000000000000000000000"),
                    CipherText = new BitString("ba77413dea5925b7f5417ea47ff19f59")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffe0000000000000000000000000"),
                    CipherText = new BitString("6cae8129f843d86dc786a0fb1a184970")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffff0000000000000000000000000"),
                    CipherText = new BitString("fcfefb534100796eebbd990206754e19")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffff8000000000000000000000000"),
                    CipherText = new BitString("8c791d5fdddf470da04f3e6dc4a5b5b5")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffc000000000000000000000000"),
                    CipherText = new BitString("c93bbdc07a4611ae4bb266ea5034a387")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffe000000000000000000000000"),
                    CipherText = new BitString("c102e38e489aa74762f3efc5bb23205a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffff000000000000000000000000"),
                    CipherText = new BitString("93201481665cbafc1fcc220bc545fb3d")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffff800000000000000000000000"),
                    CipherText = new BitString("4960757ec6ce68cf195e454cfd0f32ca")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffc00000000000000000000000"),
                    CipherText = new BitString("feec7ce6a6cbd07c043416737f1bbb33")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffe00000000000000000000000"),
                    CipherText = new BitString("11c5413904487a805d70a8edd9c35527")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffff00000000000000000000000"),
                    CipherText = new BitString("347846b2b2e36f1f0324c86f7f1b98e2")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffff80000000000000000000000"),
                    CipherText = new BitString("332eee1a0cbd19ca2d69b426894044f0")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffc0000000000000000000000"),
                    CipherText = new BitString("866b5b3977ba6efa5128efbda9ff03cd")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffe0000000000000000000000"),
                    CipherText = new BitString("cc1445ee94c0f08cdee5c344ecd1e233")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffff0000000000000000000000"),
                    CipherText = new BitString("be288319029363c2622feba4b05dfdfe")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffff8000000000000000000000"),
                    CipherText = new BitString("cfd1875523f3cd21c395651e6ee15e56")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffc000000000000000000000"),
                    CipherText = new BitString("cb5a408657837c53bf16f9d8465dce19")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffe000000000000000000000"),
                    CipherText = new BitString("ca0bf42cb107f55ccff2fc09ee08ca15")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffff000000000000000000000"),
                    CipherText = new BitString("fdd9bbb4a7dc2e4a23536a5880a2db67")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffff800000000000000000000"),
                    CipherText = new BitString("ede447b362c484993dec9442a3b46aef")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffc00000000000000000000"),
                    CipherText = new BitString("10dffb05904bff7c4781df780ad26837")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffe00000000000000000000"),
                    CipherText = new BitString("c33bc13e8de88ac25232aa7496398783")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffff00000000000000000000"),
                    CipherText = new BitString("ca359c70803a3b2a3d542e8781dea975")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffff80000000000000000000"),
                    CipherText = new BitString("bcc65b526f88d05b89ce8a52021fdb06")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffc0000000000000000000"),
                    CipherText = new BitString("db91a38855c8c4643851fbfb358b0109")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffe0000000000000000000"),
                    CipherText = new BitString("ca6e8893a114ae8e27d5ab03a5499610")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffff0000000000000000000"),
                    CipherText = new BitString("6629d2b8df97da728cdd8b1e7f945077")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffff8000000000000000000"),
                    CipherText = new BitString("4570a5a18cfc0dd582f1d88d5c9a1720")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffc000000000000000000"),
                    CipherText = new BitString("72bc65aa8e89562e3f274d45af1cd10b")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffe000000000000000000"),
                    CipherText = new BitString("98551da1a6503276ae1c77625f9ea615")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffff000000000000000000"),
                    CipherText = new BitString("0ddfe51ced7e3f4ae927daa3fe452cee")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffff800000000000000000"),
                    CipherText = new BitString("db826251e4ce384b80218b0e1da1dd4c")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffc00000000000000000"),
                    CipherText = new BitString("2cacf728b88abbad7011ed0e64a1680c")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffe00000000000000000"),
                    CipherText = new BitString("330d8ee7c5677e099ac74c9994ee4cfb")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffff00000000000000000"),
                    CipherText = new BitString("edf61ae362e882ddc0167474a7a77f3a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffff80000000000000000"),
                    CipherText = new BitString("6168b00ba7859e0970ecfd757efecf7c")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffc0000000000000000"),
                    CipherText = new BitString("d1415447866230d28bb1ea18a4cdfd02")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffe0000000000000000"),
                    CipherText = new BitString("516183392f7a8763afec68a060264141")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffff0000000000000000"),
                    CipherText = new BitString("77565c8d73cfd4130b4aa14d8911710f")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffff8000000000000000"),
                    CipherText = new BitString("37232a4ed21ccc27c19c9610078cabac")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffc000000000000000"),
                    CipherText = new BitString("804f32ea71828c7d329077e712231666")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffe000000000000000"),
                    CipherText = new BitString("d64424f23cb97215e9c2c6f28d29eab7")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffff000000000000000"),
                    CipherText = new BitString("023e82b533f68c75c238cebdb2ee89a2")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffff800000000000000"),
                    CipherText = new BitString("193a3d24157a51f1ee0893f6777417e7")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffc00000000000000"),
                    CipherText = new BitString("84ecacfcd400084d078612b1945f2ef5")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffe00000000000000"),
                    CipherText = new BitString("1dcd8bb173259eb33a5242b0de31a455")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffff00000000000000"),
                    CipherText = new BitString("35e9eddbc375e792c19992c19165012b")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffff80000000000000"),
                    CipherText = new BitString("8a772231c01dfdd7c98e4cfddcc0807a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffffc0000000000000"),
                    CipherText = new BitString("6eda7ff6b8319180ff0d6e65629d01c3")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffffe0000000000000"),
                    CipherText = new BitString("c267ef0e2d01a993944dd397101413cb")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffff0000000000000"),
                    CipherText = new BitString("e9f80e9d845bcc0f62926af72eabca39")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffff8000000000000"),
                    CipherText = new BitString("6702990727aa0878637b45dcd3a3b074")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffffc000000000000"),
                    CipherText = new BitString("2e2e647d5360e09230a5d738ca33471e")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffffe000000000000"),
                    CipherText = new BitString("1f56413c7add6f43d1d56e4f02190330")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffffff000000000000"),
                    CipherText = new BitString("69cd0606e15af729d6bca143016d9842")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffffff800000000000"),
                    CipherText = new BitString("a085d7c1a500873a20099c4caa3c3f5b")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffffffc00000000000"),
                    CipherText = new BitString("4fc0d230f8891415b87b83f95f2e09d1")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffffffe00000000000"),
                    CipherText = new BitString("4327d08c523d8eba697a4336507d1f42")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffffff00000000000"),
                    CipherText = new BitString("7a15aab82701efa5ae36ab1d6b76290f")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffffff80000000000"),
                    CipherText = new BitString("5bf0051893a18bb30e139a58fed0fa54")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffffffc0000000000"),
                    CipherText = new BitString("97e8adf65638fd9cdf3bc22c17fe4dbd")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffffffe0000000000"),
                    CipherText = new BitString("1ee6ee326583a0586491c96418d1a35d")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffffffff0000000000"),
                    CipherText = new BitString("26b549c2ec756f82ecc48008e529956b")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffffffff8000000000"),
                    CipherText = new BitString("70377b6da669b072129e057cc28e9ca5")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffffffffc000000000"),
                    CipherText = new BitString("9c94b8b0cb8bcc919072262b3fa05ad9")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffffffffe000000000"),
                    CipherText = new BitString("2fbb83dfd0d7abcb05cd28cad2dfb523")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffffffff000000000"),
                    CipherText = new BitString("96877803de77744bb970d0a91f4debae")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffffffff800000000"),
                    CipherText = new BitString("7379f3370cf6e5ce12ae5969c8eea312")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffffffffc00000000"),
                    CipherText = new BitString("02dc99fa3d4f98ce80985e7233889313")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffffffffe00000000"),
                    CipherText = new BitString("1e38e759075ba5cab6457da51844295a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffffffffff00000000"),
                    CipherText = new BitString("70bed8dbf615868a1f9d9b05d3e7a267")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffffffffff80000000"),
                    CipherText = new BitString("234b148b8cb1d8c32b287e896903d150")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffffffffffc0000000"),
                    CipherText = new BitString("294b033df4da853f4be3e243f7e513f4")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffffffffffe0000000"),
                    CipherText = new BitString("3f58c950f0367160adec45f2441e7411")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffffffffff0000000"),
                    CipherText = new BitString("37f655536a704e5ace182d742a820cf4")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffffffffff8000000"),
                    CipherText = new BitString("ea7bd6bb63418731aeac790fe42d61e8")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffffffffffc000000"),
                    CipherText = new BitString("e74a4c999b4c064e48bb1e413f51e5ea")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffffffffffe000000"),
                    CipherText = new BitString("ba9ebefdb4ccf30f296cecb3bc1943e8")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffffffffffff000000"),
                    CipherText = new BitString("3194367a4898c502c13bb7478640a72d")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffffffffffff800000"),
                    CipherText = new BitString("da797713263d6f33a5478a65ef60d412")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffc00000"),
                    CipherText = new BitString("d1ac39bb1ef86b9c1344f214679aa376")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffe00000"),
                    CipherText = new BitString("2fdea9e650532be5bc0e7325337fd363")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffffffffffff00000"),
                    CipherText = new BitString("d3a204dbd9c2af158b6ca67a5156ce4a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffffffffffff80000"),
                    CipherText = new BitString("3a0a0e75a8da36735aee6684d965a778")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffc0000"),
                    CipherText = new BitString("52fc3e620492ea99641ea168da5b6d52")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffe0000"),
                    CipherText = new BitString("d2e0c7f15b4772467d2cfc873000b2ca")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff0000"),
                    CipherText = new BitString("563531135e0c4d70a38f8bdb190ba04e")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff8000"),
                    CipherText = new BitString("a8a39a0f5663f4c0fe5f2d3cafff421a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffc000"),
                    CipherText = new BitString("d94b5e90db354c1e42f61fabe167b2c0")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffe000"),
                    CipherText = new BitString("50e6d3c9b6698a7cd276f96b1473f35a")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff000"),
                    CipherText = new BitString("9338f08e0ebee96905d8f2e825208f43")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff800"),
                    CipherText = new BitString("8b378c86672aa54a3a266ba19d2580ca")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffc00"),
                    CipherText = new BitString("cca7c3086f5f9511b31233da7cab9160")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffe00"),
                    CipherText = new BitString("5b40ff4ec9be536ba23035fa4f06064c")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff00"),
                    CipherText = new BitString("60eb5af8416b257149372194e8b88749")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff80"),
                    CipherText = new BitString("2f005a8aed8a361c92e440c15520cbd1")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffc0"),
                    CipherText = new BitString("7b03627611678a997717578807a800e2")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffe0"),
                    CipherText = new BitString("cf78618f74f6f3696e0a4779b90b5a77")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff0"),
                    CipherText = new BitString("03720371a04962eaea0a852e69972858")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff8"),
                    CipherText = new BitString("1f8a8133aa8ccf70e2bd3285831ca6b7")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffc"),
                    CipherText = new BitString("27936bd27fb1468fc8b48bc483321725")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffe"),
                    CipherText = new BitString("b07d4f3e2cd2ef2eb545980754dfea0f")
                },
                new AlgoArrayResponse()
                {

                    PlainText = plainText,
                    Key = new BitString("ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff"),
                    CipherText = new BitString("4bf85f1b5d54adbc307b0a048389adcb")
                },
            };
        }
        #endregion VarKey
    }
}
