using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Crypto.Common.DRBG.Enums;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.DRBG.Tests
{
    [TestFixture, FastCryptoTest]
    public class DrbgHmacTests
    {
        [SetUp]
        public void Setup()
        {
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
                Mechanism = DrbgMechanism.HMAC,
                Mode = DrbgMode.SHA1,
                ReseedImplemented = false
            };

            var subject = factory.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(new BitString("e91b63309e93d1d08e30e8d556906875")); // Entropy Input
            entropyProvider.AddEntropy(new BitString("f59747c468b0d0da")); // Nonce
            subject.Instantiate(0, new BitString(0)); // Perso string
            entropyProvider.AddEntropy(new BitString(0)); // Entropy Input Reseed

            //subject.Reseed(false, new BitString(0));
            subject.Generate(parameters.ReturnedBitsLen, new BitString(0)); // Additional Input
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString(0)); // Additional Input

            var expectation = new BitString("b7928f9503a417110788f9d0c2585f8aee6fb73b220a626b3ab9825b7a9facc79723d7e1ba9255e40e65c249b6082a7bc5e3f129d3d8f69b04ed1183419d6c4f2a13b304d2c5743f41c8b0ee73225347");

            Assert.AreEqual(expectation, result.Bits);
        }

        [Test]
        public void ShouldSha224NoReseedNoPersoNoAddInput()
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
                Mechanism = DrbgMechanism.HMAC,
                Mode = DrbgMode.SHA224,
                ReseedImplemented = false
            };

            var subject = factory.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(new BitString("a76e77a969ab92645181f0157802523746c34bf321867641")); // Entropy Input
            entropyProvider.AddEntropy(new BitString("051ed6ba39368033adc93d4e")); // Nonce
            subject.Instantiate(0, new BitString(0)); // Perso string
            entropyProvider.AddEntropy(new BitString(0)); // Entropy Input Reseed
            entropyProvider.AddEntropy(new BitString(0)); // Entropy Input Reseed

            //subject.Reseed(false, new BitString(0));
            subject.Generate(parameters.ReturnedBitsLen, new BitString(0)); // Additional Input
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString(0)); // Additional Input

            var expectation = new BitString("8925987db5566e60520f09bdddab488292bed92cd385e5b6fc223e1919640b4e34e34575033e56c0a8f608be21d3d221c67d39abec98d81312f3a2653d55ffbf44c337c82bed314c211be23ec394399ba351c4687dce649e7c2a1ba7b0b5dab125671b1bcf9008da65cad612d95ddc92");

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
                Mechanism = DrbgMechanism.HMAC,
                Mode = DrbgMode.SHA256,
                ReseedImplemented = false
            };

            var subject = factory.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(new BitString("2cad88b2b6a06e703de46185ccb2ddcf5e0ee030995ebdf95cc4fbc38441f17f")); // Entropy Input
            entropyProvider.AddEntropy(new BitString("32310770e04172c0cf91f6590cce44a4")); // Nonce
            subject.Instantiate(0, new BitString(0)); // Perso string
            entropyProvider.AddEntropy(new BitString("448bfbc5ce9e3b9da3e9642daecd994dfe373e75253e8eb585141224eca7ad7b")); // Entropy Input Reseed
            entropyProvider.AddEntropy(new BitString("afb57f69799c0b892b3015990e133698d543aa87829ace868e4a5e9525d62357")); // Entropy Input Reseed

            //subject.Reseed(false, new BitString(0));
            subject.Generate(parameters.ReturnedBitsLen, new BitString("ef6da5e6530e0d621749ab192e06327e995c3ac0c3963ab8c8cd2df2839ab5df")); // Additional Input
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString("44278b31ed853f0a510bd14650ac4b4971d8b426799a43511d016be68dedbb8d")); // Additional Input

            var expectation = new BitString("4c7dfbe509dc5a3ac26998723c6a44cad20b197fc86117c778d1568ab828923862885e97198f77a1cb45113f5d78726a0f120aec94afc45f57c8dcc1cb092b343480012858ef5bc559f57023442209326ec4a54d91ca3a77dfdf9e75f117cef50e6fd2dc9af6ddce8e6515b4a97357a97b6cd274f68a042fa41bbd7b7261b034");

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
                Mechanism = DrbgMechanism.HMAC,
                Mode = DrbgMode.SHA384,
                ReseedImplemented = false
            };

            var subject = factory.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(new BitString("37dc21c72dc7c82d5e13c51ecaf5a8ae06402500d92caf96c0555a95069f4f01")); // Entropy Input
            entropyProvider.AddEntropy(new BitString("44a961ead5d6d9bc317afc8206202bdd")); // Nonce
            subject.Instantiate(0, new BitString("41e3b89347bd035bde510ab8ff83d5fdcc9d5f2de648bdb468a714f2c1083c52")); // Perso string
            entropyProvider.AddEntropy(new BitString("d57fc02a2a500df1fb5c4d9d8837b52a5220fdf068fe2b8b4bcc63fbc9bfc94c")); // Entropy Input Reseed
            entropyProvider.AddEntropy(new BitString("8e21d987e8b6cb0f4cd37b144c668f18b7a36ed4e9758ee7b96029aa0ab2196a")); // Entropy Input Reseed

            //subject.Reseed(false, new BitString(0));
            subject.Generate(parameters.ReturnedBitsLen, new BitString(0)); // Additional Input
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString(0)); // Additional Input

            var expectation = new BitString("a929ee23c5832e5ab93ccaa40bf775593d7d04a1a8411dfa07b4c8a2da2dc91b1bcb9c27a0ba5a7152ce5ded5f76cf6b83c04c0f8a4f6b43383ae3e7d497280c0f944be91b0bca6a56df2d00641bfc1ec549b538898e559407b076164278c0eb7afb6d6f4495a50d4da178c04b259d21bb745692d3bd186edf5bb3da6f66b4418fc3d9b085b0a6c1a5e54696272c305c4b8887595b391dd6ed8da03dc9fdb2728d8c40a2defd8af05ef1c443a72323f2e0b0d268109fb7e7ee70192fa06bc6c2");

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
                Mechanism = DrbgMechanism.HMAC,
                Mode = DrbgMode.SHA512,
                ReseedImplemented = true
            };

            var subject = factory.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(new BitString("3aca6b55561521007c9ece085e9a6635e346fa804335d6ad42ebd6814c017fa8")); // Entropy Input
            entropyProvider.AddEntropy(new BitString("aa7fd3c3dd5d03d9b8efc7f70574581f")); // Nonce
            subject.Instantiate(0, new BitString("4bc9a485ec840d377ae4504aa1df41e444c4231687f3d7851c26c275bc687463")); // Perso string
            entropyProvider.AddEntropy(new BitString("4cc19fae5a456f8a53a656d23a0b665d6ddf7f43020a5febbb552714e447565d")); // Entropy Input Reseed
            entropyProvider.AddEntropy(new BitString("637386b3ab33f78fd9751c7b7e67e1e15f6e50ddc548a1eb5813f6d0d48381bf")); // Entropy Input Reseed

            //subject.Reseed(false, new BitString(0));
            subject.Generate(parameters.ReturnedBitsLen, new BitString("b39c43539fdc24343085cbb65b8d36c54732476d781104c355c391a951313a30")); // Additional Input
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString("b6850edd4622675ef5a507eab911e249d63fcf62f330cc8a16bb2ccc5858de5d")); // Additional Input

            var expectation = new BitString("546664042bef33064da28a5718f2c2e5f72d7725e3fbe87ad2ee90fbfe6c114ed36440fbbccf29698b4360bc4ad74650de13825838106adc53002bc389ee900691649b972f3187b84d05cecc8fd034497dd99c6c997d1914b4ef838d84abf23fae7f3ac9efdcdc04c003ac642c5126b00f9f24bf1431a4f19ef0b5f3d230aab3fdf091ba31b7ddcacdf2566f2cfab30f55b3123e733829b697b7c8b248420ab98ba6f11b017175256368e8d8361102c9e6d57386becbeabda092dd57aec65bc20ebee78eea7294571e168c454066d256b81bb8b7bb469207a18ebedbb4348fbe97a4d86d2bd095c41f6de59aa0800e131e98181886a2633cdcc550914d83b327");

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
                Mechanism = DrbgMechanism.HMAC,
                Mode = DrbgMode.SHA512t224,
                ReseedImplemented = true
            };

            var subject = factory.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(new BitString("1e48129b9eb4bd5fa6fd7775f0813d3928c7d21de4d06c34")); // Entropy Input
            entropyProvider.AddEntropy(new BitString("a18e9fda78f5134ba2a264d4")); // Nonce
            subject.Instantiate(0, new BitString("558ca5ae37effdb8cee4e5204b17bfea686c8773d97a78e3")); // Perso string
            entropyProvider.AddEntropy(new BitString("3ca0289962abb8a5f1d138d69ebdcbf68d73185d98f77745")); // Entropy Input Reseed
            entropyProvider.AddEntropy(new BitString("9f213f9b53769b1ebdba3841c6c05a83919ad950639e0b7e")); // Entropy Input Reseed

            //subject.Reseed(false, new BitString(0));
            subject.Generate(parameters.ReturnedBitsLen, new BitString(0)); // Additional Input
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString(0)); // Additional Input

            var expectation = new BitString("e33dcee09e13999b9ac710a8880a24eb4c745fbb1fab37b541039dbbd121e4c8049e3c4e31535fd3b9b1d97a7cfd586bbf2ecf973f03b08e379ddc1d996b71e026d62b1b0ae4ea0fe7e36cc507711df502ac7e5b3124a4c13afda69b06d7e39660b5bd13a17aa6723c4a953cca9f0e6c");

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
                Mechanism = DrbgMechanism.HMAC,
                Mode = DrbgMode.SHA512t256,
                ReseedImplemented = true
            };

            var subject = factory.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(new BitString("46fc5de4ea7d31c60cc62b49d283b7bc71778a028273c0bd4e59f590e168ceb7")); // Entropy Input
            entropyProvider.AddEntropy(new BitString("d91d148f4a52bc99c269d46729d594bf")); // Nonce
            subject.Instantiate(256, new BitString(0)); // Perso string
            entropyProvider.AddEntropy(new BitString("76b955c9aa4c290418159f4d805b427de35e42aa1cf10f59e743c56c92280a55")); // Entropy Input Reseed

            subject.Reseed(new BitString("6722b8f0ae83cc1f52e3ded6f588b638881d5e097d8bee2675ae30efb417936a"));
            subject.Generate(parameters.ReturnedBitsLen, new BitString("d7eae425d0a484693557a53ef97dedf43f255c2a9e38a8334ccef35829e04f17")); // Additional Input
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString("eb3d3e3fff84d72a84bf94fa49b643d7d1ebe768b4d508360cdf962eb3cd48d7")); // Additional Input

            var expectation = new BitString("9fe074750995b0c2638eb48ab66957b4033fa34202a7cc9a5354ea2db4aba89ca10dc5eda1f03f9f5a9fc4cd9b9191d96d056ab74f5a4fda15442f1839dff8f240b06e9276f03fb1d7c3b5a8a571c37dbf89e9ed8afe09e542add83332d730c4f74124b7f59fea6a6a8e816ac5e0d66d7944cf9cf18936163bb8a81a34f0d4a4");

            Assert.AreEqual(expectation, result.Bits);
        }
    }
}
