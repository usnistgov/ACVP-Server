using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.DRBG;
using NIST.CVP.Crypto.DRBG.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.DRBG.Tests
{
    [TestFixture, FastIntegrationTest, UnitTest]
    public class DRBGTests
    {

        [SetUp]
        public void Setup()
        {
            LoggingHelper.ConfigureLogging("DRBGTests", "DRBGCtr");
        }

        [Test]
        public void ShouldAes128NoReseedNoDfNoPrNoNonceNoAddInput()
        {
            TestableEntropyProvider entropyProvider = new TestableEntropyProvider();

            DrbgFactory factory = new DrbgFactory();

            DrbgParameters parameters = new DrbgParameters()
            {
                PredResistanceEnabled = false,
                EntropyInputLen = 256,
                NonceLen = 0,
                PersoStringLen = 0,
                AdditionalInputLen = 0,
                ReturnedBitsLen = 512,
                DerFuncEnabled = false,
                Mechanism = DrbgMechanism.Counter,
                Mode = DrbgMode.AES128,
                ReseedImplemented = false
            };

            var subject = factory.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(new BitString("ed435694a0c1bc5a11ceb7c315984d8f62fda0b1327840c0775569a86257f566")); // Entropy Input
            entropyProvider.AddEntropy(new BitString(0)); // Nonce
            subject.Instantiate(256, new BitString(0));
            entropyProvider.AddEntropy(new BitString(0)); // Entropy Input Reseed

            //subject.Reseed(false, new BitString(0));
            subject.Generate(parameters.ReturnedBitsLen, new BitString(0));
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString(0));

            var expectation = new BitString("35a540967abf5eb8dd0ce312771c3799fa7e39d2e2085e6e99668c2ae127eb9a4e02a4a55662127458d9a41e945ab924ec4cb61352041cc21676de7b0ed714aa");

            Assert.AreEqual(expectation, result.Bits);
        }

        [Test]
        public void ShouldAes256NoDfNoPrNoAddInputNoNonce()
        {
            TestableEntropyProvider entropyProvider = new TestableEntropyProvider();

            DrbgFactory factory = new DrbgFactory();

            DrbgParameters parameters = new DrbgParameters()
            {
                PredResistanceEnabled = false,
                EntropyInputLen = 384,
                NonceLen = 0,
                PersoStringLen = 0,
                AdditionalInputLen = 0,
                ReturnedBitsLen = 512,
                DerFuncEnabled = false,
                Mechanism = DrbgMechanism.Counter,
                Mode = DrbgMode.AES256,
                ReseedImplemented = true
            };

            var subject = factory.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(new BitString("0510f0fea150f9f193fda43dc3f2b6115ff5aa5f9c6aab7529ca007acd5d755422bb0250df0e0289576f3c6d0501701c")); // Entropy Input
            entropyProvider.AddEntropy(new BitString(0)); // Nonce
            subject.Instantiate(256, new BitString(0));
            entropyProvider.AddEntropy(new BitString("9b8f648ac1467122208193923bf1847dd24831f2940a1827783f64adb8c5e223475bf83ca10b4486b6a51419c00d0abe")); // Entropy Input Reseed
            subject.Reseed(new BitString(0));
            subject.Generate(parameters.ReturnedBitsLen, new BitString(0));
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString(0));

            var expectation = new BitString("b720ddf5c154b4e77e008bf910a26f2978571e03cbdefd94b3abf76267d819d9314c003c899af6978f3514b68829366b61548fc8bade86d90f2a1538bbc9ff1e");

            Assert.AreEqual(expectation, result.Bits);
        }

        [Test]
        public void ShouldAes256NoDfNoPrNoNonceAddInput()
        {
            TestableEntropyProvider entropyProvider = new TestableEntropyProvider();

            DrbgFactory factory = new DrbgFactory();

            DrbgParameters parameters = new DrbgParameters()
            {
                PredResistanceEnabled = false,
                EntropyInputLen = 384,
                NonceLen = 0,
                PersoStringLen = 0,
                AdditionalInputLen = 384,
                ReturnedBitsLen = 512,
                DerFuncEnabled = false,
                Mechanism = DrbgMechanism.Counter,
                Mode = DrbgMode.AES256,
                ReseedImplemented = true
            };

            var subject = factory.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(new BitString("5f59060ab389bce8641d52253d2ab6d88d9b114a64b00de522b4f07be26a56c8f0cbdf33b81a888d389f343d19909f34")); // Entropy Input
            entropyProvider.AddEntropy(new BitString(0)); // Nonce
            subject.Instantiate(256, new BitString(0));
            entropyProvider.AddEntropy(new BitString("f9ec27e777b9385cd77573668140b21a5e5d1b0ea66f950ee054c1c483b9062d93ea406a7c3a95286029803161fe0865")); // Entropy Input Reseed
            
            subject.Reseed(new BitString("005debda17ad3a6b8600314fd19be0871da2f4f3f58946e6745554f79d0915bb5ea0f5fe2eb794d44742c1e14461cd87"));
            subject.Generate(parameters.ReturnedBitsLen, new BitString("1ceb9c2772b1bcb91880f49862377ef3c9d7590a67ab78426a9549ebbe93f6656fb4f5ee2ddc45fd5502cf5160aea69a"));
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString("1047c50840b200de336babebee430d5a258ad98017d504b8ec9916e1284fa4b82ba4317bd9a6c090455944845dc700c4"));

            var expectation = new BitString("4ba0a32db68d2feeef2ea9ce0982f9e7072112eafe05553bc9ef381c16abf4dd2fc28e5a1c62532da760e87c28e24c641281727e5c56912884dda39445995323");

            Assert.AreEqual(expectation, result.Bits);
        }

        [Test]
        public void ShouldAes256NoDfNoPrNonceNoAddInput()
        {
            TestableEntropyProvider entropyProvider = new TestableEntropyProvider();

            DrbgFactory factory = new DrbgFactory();

            DrbgParameters parameters = new DrbgParameters()
            {
                PredResistanceEnabled = false,
                EntropyInputLen = 384,
                NonceLen = 0,
                PersoStringLen = 0,
                AdditionalInputLen = 0,
                ReturnedBitsLen = 512,
                DerFuncEnabled = false,
                Mechanism = DrbgMechanism.Counter,
                Mode = DrbgMode.AES256,
                ReseedImplemented = true
            };

            var subject = factory.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(new BitString("0510f0fea150f9f193fda43dc3f2b6115ff5aa5f9c6aab7529ca007acd5d755422bb0250df0e0289576f3c6d0501701c")); // Entropy Input
            entropyProvider.AddEntropy(new BitString(0)); // Nonce
            subject.Instantiate(256, new BitString(0));
            entropyProvider.AddEntropy(new BitString("9b8f648ac1467122208193923bf1847dd24831f2940a1827783f64adb8c5e223475bf83ca10b4486b6a51419c00d0abe")); // Entropy Input Reseed

            subject.Reseed(new BitString(0));
            subject.Generate(parameters.ReturnedBitsLen, new BitString(0));
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString(0));

            var expectation = new BitString("b720ddf5c154b4e77e008bf910a26f2978571e03cbdefd94b3abf76267d819d9314c003c899af6978f3514b68829366b61548fc8bade86d90f2a1538bbc9ff1e");

            Assert.AreEqual(expectation, result.Bits);
        }

        [Test]
        public void ShouldAes256NoDfPrNoNonceNoAddInput()
        {
            TestableEntropyProvider entropyProvider = new TestableEntropyProvider();

            DrbgFactory factory = new DrbgFactory();

            DrbgParameters parameters = new DrbgParameters()
            {
                PredResistanceEnabled = true,
                EntropyInputLen = 384,
                NonceLen = 0,
                PersoStringLen = 0,
                AdditionalInputLen = 0,
                ReturnedBitsLen = 512,
                DerFuncEnabled = false,
                Mechanism = DrbgMechanism.Counter,
                Mode = DrbgMode.AES256,
                ReseedImplemented = true
            };

            var subject = factory.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(new BitString("de23cda6223a2fb5cfbd23952bc4a703301c0a7804da5ce657c0c6c07eaf0d0460542647d4deb1d9c2efe26aa83ff2a8")); // Entropy Input
            entropyProvider.AddEntropy(new BitString(0)); // Nonce
            subject.Instantiate(256, new BitString(0));

            entropyProvider.AddEntropy(new BitString("f9268584daf03990fc86c20e83a94fb7523e158e1a17c5b81c9e517c5ae59489f7c1c211e226fdeddfaed7d28f810d12"));
            entropyProvider.AddEntropy(new BitString("c361e0499dd377466ccbd02d746bc5edecb4de10cdd2af1185bafbd1ffeb71e86d808481f2eda93bb0130ebf999a0a5e"));

            //subject.Reseed(true, new BitString(0));
            subject.Generate(parameters.ReturnedBitsLen, new BitString(0));
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString(0));

            var expectation = new BitString("652205d5ee220a7f64c7387758324b333dddb37b1da2e52f9a4eedeb55d23df6caf74d2545005261bb710dcf5b864d4679d0ee4d7ec3665f76147d401f7b5542");

            Assert.AreEqual(expectation, result.Bits);
        }

        [Test]
        public void ShouldAes256DfNoPrNonceNoAddInput()
        {
            TestableEntropyProvider entropyProvider = new TestableEntropyProvider();

            DrbgFactory factory = new DrbgFactory();

            DrbgParameters parameters = new DrbgParameters()
            {
                PredResistanceEnabled = false,
                EntropyInputLen = 256,
                NonceLen = 128,
                PersoStringLen = 0,
                AdditionalInputLen = 0,
                ReturnedBitsLen = 512,
                DerFuncEnabled = true,
                Mechanism = DrbgMechanism.Counter,
                Mode = DrbgMode.AES256,
                ReseedImplemented = true
            };

            var subject = factory.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(new BitString("a621cd10710b229bdfd61db8e6424865db9fe09c2e6d4ecc13f8336c106f8d90")); // Entropy Input
            entropyProvider.AddEntropy(new BitString("5ff15f2ea0815396a7b89e86925c9a87")); // Nonce
            subject.Instantiate(256, new BitString(0));

            entropyProvider.AddEntropy(new BitString("9499b0a15d80feb893bb08177ce00d9cf3fcd7c0792c93c4aa394a7b56b819fa"));

            subject.Reseed(new BitString(0));
            subject.Generate(parameters.ReturnedBitsLen, new BitString(0));
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString(0));

            var expectation = new BitString("331834c48b4f442ac02a7638a75aba03463dfab258b2618106791ddd71b665d5165aeb3f677a4ef0fb648db393c37ce01885dc8bf329fb9bbf4fea8fa2bbd821");

            Assert.AreEqual(expectation, result.Bits);
        }

        [Test]
        public void ShouldAes256DfPrNonceNoAddInput()
        {
            TestableEntropyProvider entropyProvider = new TestableEntropyProvider();

            DrbgFactory factory = new DrbgFactory();

            DrbgParameters parameters = new DrbgParameters()
            {
                PredResistanceEnabled = true,
                EntropyInputLen = 256,
                NonceLen = 128,
                PersoStringLen = 0,
                AdditionalInputLen = 0,
                ReturnedBitsLen = 512,
                DerFuncEnabled = true,
                Mechanism = DrbgMechanism.Counter,
                Mode = DrbgMode.AES256,
                ReseedImplemented = true
            };

            var subject = factory.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(new BitString("2027493bdcc0b3c31bf77ff3bca20b08971246694f6ed1b5eb5250a2cd1371b5")); // Entropy Input
            entropyProvider.AddEntropy(new BitString("445ba13c425ece265930de9a56251d49")); // Nonce
            subject.Instantiate(256, new BitString(0));
            
            entropyProvider.AddEntropy(new BitString("50da3436de6d65bd109c91045bb86aa14c6d42fadf74b21dd2cb84fe09e3523a"));
            entropyProvider.AddEntropy(new BitString("4d95ca31f7dee0940fc77aed0a5b78d7e62725e2136cb0ab86f9189c26b36267"));

            //subject.Reseed(false, new BitString("005debda17ad3a6b8600314fd19be0871da2f4f3f58946e6745554f79d0915bb5ea0f5fe2eb794d44742c1e14461cd87"));
            subject.Generate(parameters.ReturnedBitsLen, new BitString(0));
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString(0));

            var expectation = new BitString("32e647774db03fafdd5f37c0805008bdf918256ebbb4362d577b3882a51cead1a206a08afe9eea20ce3d768463658e9259a1125b34f8d14ee0c7746e14b8a925");

            Assert.AreEqual(expectation, result.Bits);
        }

        [Test]
        public void ShouldAes256DfPrNonceAddInput()
        {
            TestableEntropyProvider entropyProvider = new TestableEntropyProvider();

            DrbgFactory factory = new DrbgFactory();

            DrbgParameters parameters = new DrbgParameters()
            {
                PredResistanceEnabled = true,
                EntropyInputLen = 256,
                NonceLen = 128,
                PersoStringLen = 256,
                AdditionalInputLen = 256,
                ReturnedBitsLen = 512,
                DerFuncEnabled = true,
                Mechanism = DrbgMechanism.Counter,
                Mode = DrbgMode.AES256,
                ReseedImplemented = true
            };

            var subject = factory.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(new BitString("9c78fc049d3f0eb4d537d2cc4ddd790aeaaba616cb4bfc6fce61f2a4d93813d5")); // Entropy Input
            entropyProvider.AddEntropy(new BitString("a5bb6569e0b6c60e28fa7333ee8667b6")); // Nonce
            subject.Instantiate(256, new BitString("4420106a71f7e44a2123b00ed0f864ae9b9921458de642c359684317a369911d"));

            entropyProvider.AddEntropy(new BitString("99db5ca6cce73e6b9e50eabeb27fbf74d69eb3f4fb506a8b28e017a99d4a9898"));
            entropyProvider.AddEntropy(new BitString("303893d497aca34e4e03647ead9ffbd2880ca841a007a1a9e1f1f8f38b8ee704"));

            subject.Generate(parameters.ReturnedBitsLen, new BitString("10069750a43c2f7392510d7d124eb8e942a689264a636ac4bc9411da877e9278"));
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString("932751572c9d8d46c49175acbdb52ff54fea38b7de0108c60d8cd5285b797d42"));

            var expectation = new BitString("684e1586de693b2c2422216ba4c05dc48e183cd9990f09ff36a8618347ad6e72a9b416247d3f5530dfefea67edeca44a78b000b865b2af763a6fc4de36185a80");

            Assert.AreEqual(expectation, result.Bits);
        }
    }
}
