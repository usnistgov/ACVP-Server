using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DRBG.Enums;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.DRBG.Tests
{
    [TestFixture, FastCryptoTest]
    public class DrbgHashTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ShouldSha512224NoReseedNoPrNoPersoNoAddInput()
        {
            var entropyProvider = new TestableEntropyProvider();
            var factory = new DrbgFactory();
            var parameters = new DrbgParameters
            {
                PredResistanceEnabled = false,
                EntropyInputLen = 192,
                NonceLen = 96,
                PersoStringLen = 0,
                AdditionalInputLen = 0,
                ReturnedBitsLen = 896,
                DerFuncEnabled = false,
                Mechanism = DrbgMechanism.Hash,
                Mode = DrbgMode.SHA512t224,
                ReseedImplemented = false
            };

            var subject = factory.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(new BitString("65568162700f22ac868a504110fa466c70cfe0e7c32f1451")); // Entropy Input
            entropyProvider.AddEntropy(new BitString("69545e871b89c7b3f95885eb")); // Nonce
            subject.Instantiate(160, new BitString(0)); // Perso string
            entropyProvider.AddEntropy(new BitString(0)); // Entropy Input Reseed

            //subject.Reseed(false, new BitString(0));
            subject.Generate(parameters.ReturnedBitsLen, new BitString(0)); // Additional Input
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString(0)); // Additional Input

            var expectation = new BitString("588572c2e4f0d6c7077d2b9eb593687ca92c86e5a9729505fcff52adfcf8a5eb850b910b985df10299bfe7434f3b6b7af92a3edaed732751cdb421c38431e2763afc6799eb61e176f9f20945870680ff8b62484378d3a7fd7d29202e5d371785d68fe399d5f600f34517fcccadf58937");

            Assert.AreEqual(expectation, result.Bits);
        }

        [Test]
        public void ShouldSha1NoReseedNoPrNoPersoNoAddInput()
        {
            var entropyProvider = new TestableEntropyProvider();
            var factory = new DrbgFactory();
            var parameters = new DrbgParameters
            {
                PredResistanceEnabled = false,
                EntropyInputLen = 128,
                NonceLen = 64,
                PersoStringLen = 0,
                AdditionalInputLen = 0,
                ReturnedBitsLen = 640,
                DerFuncEnabled = false,
                Mechanism = DrbgMechanism.Hash,
                Mode = DrbgMode.SHA1,
                ReseedImplemented = false
            };

            var subject = factory.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(new BitString("33764f3e4edf7dda77cf42d8f05db5ec")); // Entropy Input
            entropyProvider.AddEntropy(new BitString("327e8be96dc19e25")); // Nonce
            subject.Instantiate(160, new BitString(0)); // Perso string
            entropyProvider.AddEntropy(new BitString(0)); // Entropy Input Reseed

            //subject.Reseed(false, new BitString(0));
            subject.Generate(parameters.ReturnedBitsLen, new BitString(0)); // Additional Input
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString(0)); // Additional Input

            var expectation = new BitString("04db51649d660a0712b7218a6c5fbf9acb8af0272a6c732af0f2e66292e6c4517e102877c24277c3522984c5320c52b6e1d50a6733d29f31549067b4ffb64bb5d15b261342db73617b415e503652bc7d");

            Assert.AreEqual(expectation, result.Bits);
        }

        [Test]
        public void ShouldSha224NoReseedNoPersoNoAddInput()
        {
            var entropyProvider = new TestableEntropyProvider();
            var factory = new DrbgFactory();
            var parameters = new DrbgParameters
            {
                PredResistanceEnabled = true,
                EntropyInputLen = 192,
                NonceLen = 96,
                PersoStringLen = 0,
                AdditionalInputLen = 0,
                ReturnedBitsLen = 896,
                DerFuncEnabled = false,
                Mechanism = DrbgMechanism.Hash,
                Mode = DrbgMode.SHA224,
                ReseedImplemented = false
            };

            var subject = factory.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(new BitString("cc569f9c71e2ef011991085377ba2ec4ce4f20372cc9325b")); // Entropy Input
            entropyProvider.AddEntropy(new BitString("6f02ba8a24b96a087d3b516e")); // Nonce
            subject.Instantiate(224, new BitString(0)); // Perso string
            entropyProvider.AddEntropy(new BitString("efaf7059a0a18ecac458e12864c9840a8680cad8b6402090")); // Entropy Input Reseed
            entropyProvider.AddEntropy(new BitString("ae626fa52cf75ba0f830c4936f060d4ccfe8ab5aeda42cbf")); // Entropy Input Reseed

            //subject.Reseed(false, new BitString(0));
            subject.Generate(parameters.ReturnedBitsLen, new BitString(0)); // Additional Input
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString(0)); // Additional Input

            var expectation = new BitString("3c65020a0c8d176d6ef15b5a3db00ba43ea348842c0fc0f03bc757694dc6145c1826049ac5c39b4bbf14bf02332110cd99ae8d346c435308a92a28a600aba3f20f1b145b5a07ba3da7cebb69bc1a6c8bdf74607947bae4faa5c31724ec1e8029a8006e247d3d418bcc6aa8186df53e86");

            Assert.AreEqual(expectation, result.Bits);
        }

        [Test]
        public void ShouldSha256NoReseedNoPerso()
        {
            var entropyProvider = new TestableEntropyProvider();
            var factory = new DrbgFactory();
            var parameters = new DrbgParameters
            {
                PredResistanceEnabled = true,
                EntropyInputLen = 256,
                NonceLen = 128,
                PersoStringLen = 0,
                AdditionalInputLen = 256,
                ReturnedBitsLen = 1024,
                DerFuncEnabled = false,
                Mechanism = DrbgMechanism.Hash,
                Mode = DrbgMode.SHA256,
                ReseedImplemented = false
            };

            var subject = factory.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(new BitString("587db71d1772d78a43ac5db76159cc1d938a279211cafa75c7268f603f2d8210")); // Entropy Input
            entropyProvider.AddEntropy(new BitString("6cd151fc87ad8a96e4cc2dfa5cbaa923")); // Nonce
            subject.Instantiate(256, new BitString(0)); // Perso string
            entropyProvider.AddEntropy(new BitString("f9b75d5aff19b49470fbfc72463ffdca845b62c26a7da35c2fc0be307ab0e639")); // Entropy Input Reseed
            entropyProvider.AddEntropy(new BitString("73add18476397e22be249cea23d4b43b761bdbed01e8218e53d214646b9bf35d")); // Entropy Input Reseed

            //subject.Reseed(false, new BitString(0));
            subject.Generate(parameters.ReturnedBitsLen, new BitString("7e82047a4e258cc9b3b84425d7cbc803d8be2f5624ff69b6aea40a610cbef477")); // Additional Input
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString("c1f98d3aa3c3a1db35cebabe3bdbdaee5708555528fd2f25d4b57b410ba0e06b")); // Additional Input

            var expectation = new BitString("549486b786fd7653d128ae45c1301d8ef072310f53ccf7741cd054b59d2258ccd40fffdf47d04f75697d10010d9d634ad78926e408ef4edebb81570815b8dc4e1405af5213d98df443adab024a99d4d5fc88c12c1b0ab4c91c55d6e07e3a764d4649dd4f092b920add34f24c8b6e365761eb9b45fc21c94a1853b8f6930aac12");

            Assert.AreEqual(expectation, result.Bits);
        }

        [Test]
        public void ShouldSha384NoReseedNoAddInput()
        {
            var entropyProvider = new TestableEntropyProvider();
            var factory = new DrbgFactory();
            var parameters = new DrbgParameters
            {
                PredResistanceEnabled = true,
                EntropyInputLen = 256,
                NonceLen = 128,
                PersoStringLen = 256,
                AdditionalInputLen = 0,
                ReturnedBitsLen = 1536,
                DerFuncEnabled = false,
                Mechanism = DrbgMechanism.Hash,
                Mode = DrbgMode.SHA384,
                ReseedImplemented = false
            };

            var subject = factory.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(new BitString("b16be65ecff9f739fd6ca4523224a07e7bde4f648c89f18c1f62b491ee1bde46")); // Entropy Input
            entropyProvider.AddEntropy(new BitString("0c69654d2d6333dffc082391c1625a02")); // Nonce
            subject.Instantiate(384, new BitString("f76c1246129b5a15e9f31fda4d29eeb8902d12b3ff6b2adbc3a4e9c3f9891b41")); // Perso string
            entropyProvider.AddEntropy(new BitString("512e26f4a76517dd08f8c61949f936aec036bfba734ba924d6816be8c7629964")); // Entropy Input Reseed
            entropyProvider.AddEntropy(new BitString("433563dda3519d3a0ffb2f0fa489dd38d3c4da3838f479ccb50cc50fec8e7cc9")); // Entropy Input Reseed

            //subject.Reseed(false, new BitString(0));
            subject.Generate(parameters.ReturnedBitsLen, new BitString(0)); // Additional Input
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString(0)); // Additional Input

            var expectation = new BitString("3a3b479e2e09d5e81a4126f6bb6177aa1799c84cdb828c9e75ea20728a68492ab09c989c514a82ce195cd5f94192b3f532f5719db84f84115427335a41daff1d9db65a8905c12682b24b915ab16fa6ebaa016af8c05062f6112f8703cd3827dcf50ff23e7b79fd11d29b976e9f46080f878c35a14e2f3e8d7f72f8364441ed2cad648be07c1efebb88240256e36c4b8648d229c428081cdbb14e183d5483c8b53cc67b520db48502895d57db17945c8911253e1d8da0b7c7b4d484f296fe353c");

            Assert.AreEqual(expectation, result.Bits);
        }

        [Test]
        public void ShouldSha512()
        {
            var entropyProvider = new TestableEntropyProvider();
            var factory = new DrbgFactory();
            var parameters = new DrbgParameters
            {
                PredResistanceEnabled = true,
                EntropyInputLen = 256,
                NonceLen = 128,
                PersoStringLen = 256,
                AdditionalInputLen = 256,
                ReturnedBitsLen = 2048,
                DerFuncEnabled = false,
                Mechanism = DrbgMechanism.Hash,
                Mode = DrbgMode.SHA512,
                ReseedImplemented = true
            };

            var subject = factory.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(new BitString("702c8d9beddaf7fb897a425d0fd17d1d5002264001ff28e53d05fd8bab63babb")); // Entropy Input
            entropyProvider.AddEntropy(new BitString("4b69c503a3ae83dcfad2b3613e0f17d8")); // Nonce
            subject.Instantiate(512, new BitString("63a3d7801dfb8696f5d4c82608832b719dab156c1b3b6047bfec5e6604fc2b8f")); // Perso string
            entropyProvider.AddEntropy(new BitString("98ee8bf9ee77af04c0da1e6b3d06d2b20cb97ad4e46d9257875f82aca809a21a")); // Entropy Input Reseed
            entropyProvider.AddEntropy(new BitString("f438d35a35b7ec6ea13897e429280615220daa7f91f2dbcbe83263c5a26bfe85")); // Entropy Input Reseed

            //subject.Reseed(false, new BitString(0));
            subject.Generate(parameters.ReturnedBitsLen, new BitString("a2ed6e9e3c2489e83226318c58e3795d449bad79f0bf0657502de9c97ce79983")); // Additional Input
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString("f45bb93dcafd5d8a12398e5b2c0b376f023718db6989444cafbc298b97c838e7")); // Additional Input

            var expectation = new BitString("c871ba97832f3f1febe0e844228e387323001652a53d6e8d540818413aea238c006d646eb174c5b974dc8e80087e28a8beb00f103cb999dc76728643bc2f9af043a780492c1055965e2d1183073f60ee8e5b07b8018554bb38c74acd4e07d1b6d3af93ebe06876aa11ec40b407793f4637ef57787b01368e00b54f763a437d419e09171eb551235128d0ec38ef3546fb0edc8328db76fcd296afe9ad87b765aead3b7fa873bd6bd2be4b513504d72db955591d566cc050f73c7bdfb9ffd57e4858f7ca25b2e72ea513a55988a45c294f713d13658b9b414f8cf3f3ae92cdc835a31cf1766e092ee93d05e7fd001a2765c4be43fdcb535d20588dcfbfe3fa5ea8");

            Assert.AreEqual(expectation, result.Bits);
        }

        [Test]
        public void ShouldSha512t224()
        {
            var entropyProvider = new TestableEntropyProvider();
            var factory = new DrbgFactory();
            var parameters = new DrbgParameters
            {
                PredResistanceEnabled = true,
                EntropyInputLen = 192,
                NonceLen = 96,
                PersoStringLen = 192,
                AdditionalInputLen = 0,
                ReturnedBitsLen = 896,
                DerFuncEnabled = false,
                Mechanism = DrbgMechanism.Hash,
                Mode = DrbgMode.SHA512t224,
                ReseedImplemented = true
            };

            var subject = factory.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(new BitString("23b649d085ac363211067b01581925722a56be00b38c8a18")); // Entropy Input
            entropyProvider.AddEntropy(new BitString("b13d4b96fc9d6a544e8fc8e1")); // Nonce
            subject.Instantiate(224, new BitString("a5c95fa1bc898e89950b7b34f03a63d194108a98de882d58")); // Perso string
            entropyProvider.AddEntropy(new BitString("0a0be76e8665d231d7cb0ad0248cba3985dcaa7f2d20b2e8")); // Entropy Input Reseed
            entropyProvider.AddEntropy(new BitString("7321c5a56d89551cf99dcbde7260820dd301300e09aab920")); // Entropy Input Reseed

            //subject.Reseed(false, new BitString(0));
            subject.Generate(parameters.ReturnedBitsLen, new BitString(0)); // Additional Input
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString(0)); // Additional Input

            var expectation = new BitString("19364b3d0a60572e6b3ac5af144c0e00d74ba64fecb2512460e11ac33f1f4f024352c787eca4b52b42291b8bcdd68a5c06472b374b3c7c6f7838a83595504ad8a1c4ea4d1993fec2566445774aa5f469ee11ae80a6decd13507d2b0a82253b192547054451fe07810f8edcdb1d269b6c");

            Assert.AreEqual(expectation, result.Bits);
        }

        [Test]
        public void ShouldSha512t256()
        {
            var entropyProvider = new TestableEntropyProvider();
            var factory = new DrbgFactory();
            var parameters = new DrbgParameters
            {
                PredResistanceEnabled = false,
                EntropyInputLen = 256,
                NonceLen = 128,
                PersoStringLen = 0,
                AdditionalInputLen = 256,
                ReturnedBitsLen = 1024,
                DerFuncEnabled = false,
                Mechanism = DrbgMechanism.Hash,
                Mode = DrbgMode.SHA512t256,
                ReseedImplemented = true
            };

            var subject = factory.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(new BitString("8391576a34d3986dcccd61e403cac3b3f219992128004870eec9e7925e51f92c")); // Entropy Input
            entropyProvider.AddEntropy(new BitString("e4e4d529853e19e2bc1165148b0070bc")); // Nonce
            subject.Instantiate(256, new BitString(0)); // Perso string
            entropyProvider.AddEntropy(new BitString("69f81788fdf6fee5b62dba259fd66135409c133d6c2e7a7e7a05eefea39b8999")); // Entropy Input Reseed

            subject.Reseed(new BitString("c3d25860b81a452314dc9a9dc24ad1e34b8182944ee17172da0be649c8df0f85"));
            subject.Generate(parameters.ReturnedBitsLen, new BitString("33a873e408d4ee7bf27bc576a0a0f36b1bcf9306a7645945ac6c66a74ce07165")); // Additional Input
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString("980837cb5a73249882f693813e2afdd51823a94462b3c0f747e4994af76ce719")); // Additional Input

            var expectation = new BitString("ec1c599246b585813a16e3cbb2e461cc62e6b38d96862b98e4bb502d60466d696576571dc0998110fd022ff0ed85f79cc7f6366e8f1c15b14a5cc1e79d68e2ba299d5421e6231bb47d22c998b79424de4113ab0b46e38f8c1057fd4a58201f4c40e491a57201e1a9f581cbca3f03f0e0f729cdaeadfbc86a1a0d708305a18e1a");

            Assert.AreEqual(expectation, result.Bits);
        }
    }
}
