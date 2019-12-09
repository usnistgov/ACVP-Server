using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Crypto.Common.DRBG.Enums;
using NIST.CVP.Crypto.DRBG;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.DRBG.Tests
{
    [TestFixture,  FastCryptoTest]
    public class DrbgCounterTests
    {
        [SetUp]
        public void Setup()
        {
            
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
            subject.Instantiate(128, new BitString(0));
            entropyProvider.AddEntropy(new BitString(0)); // Entropy Input Reseed

            //subject.Reseed(false, new BitString(0));
            subject.Generate(parameters.ReturnedBitsLen, new BitString(0));
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString(0));

            var expectation = new BitString("35a540967abf5eb8dd0ce312771c3799fa7e39d2e2085e6e99668c2ae127eb9a4e02a4a55662127458d9a41e945ab924ec4cb61352041cc21676de7b0ed714aa");

            Assert.AreEqual(expectation, result.Bits);
        }

        [Test]
        public void ShouldTdesNoDfNoPrNoReseedNoNonceNoAddInput()
        {
            var entropyProvider = new TestableEntropyProvider();
            var factory = new DrbgFactory();
            var parameters = new DrbgParameters
            {
                PredResistanceEnabled = false,
                EntropyInputLen = 58 * 4,
                NonceLen = 0,
                PersoStringLen = 0,
                AdditionalInputLen = 0,
                ReturnedBitsLen = 128,
                DerFuncEnabled = false,
                Mechanism = DrbgMechanism.Counter,
                Mode = DrbgMode.TDES,
                ReseedImplemented = false
            };

            var subject = factory.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(new BitString("000102030405060708090A0B0C0D0E0F101112131415161718191A1B1C")); // Entropy Input
            entropyProvider.AddEntropy(new BitString(0)); // Nonce
            subject.Instantiate(112, new BitString(0));
            //entropyProvider.AddEntropy(new BitString(0)); // Entropy Input Reseed

            entropyProvider.AddEntropy(new BitString("80 81828384 85868788 898A8B8C 8D8E8F90 91929394 95969798 999A9B9C"));      // EI1
            entropyProvider.AddEntropy(new BitString("C0 C1C2C3C4 C5C6C7C8 C9CACBCC CDCECFD0 D1D2D3D4 D5D6D7D8 D9DADBDC"));      // EI2

            //subject.Reseed(false, new BitString(0));
            subject.Generate(parameters.ReturnedBitsLen, new BitString(0));
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString(0));

            var expectation = new BitString("C78CC305 D0D238C9 AA647225 6FFFD0F9");

            Assert.AreEqual(expectation.ToHex(), result.Bits.ToHex());
        }

        [Test]
        public void ShouldTdesNoReseedNoAddInput()
        {
            var entropyProvider = new TestableEntropyProvider();
            var factory = new DrbgFactory();
            var parameters = new DrbgParameters
            {
                PredResistanceEnabled = true,
                EntropyInputLen = 112,
                NonceLen = 56,
                PersoStringLen = 0,
                AdditionalInputLen = 0,
                ReturnedBitsLen = 256,
                DerFuncEnabled = true,
                Mechanism = DrbgMechanism.Counter,
                Mode = DrbgMode.TDES,
                ReseedImplemented = false
            };

            var subject = factory.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(new BitString("8525cdd834b2a0ba472882e5d56d")); // Entropy Input
            entropyProvider.AddEntropy(new BitString("8d435165c7f4fa")); // Nonce
            subject.Instantiate(112, new BitString(0));
            //entropyProvider.AddEntropy(new BitString(0)); // Entropy Input Reseed

            entropyProvider.AddEntropy(new BitString("9c68b90f9a66ebf958ee599feb41"));
            entropyProvider.AddEntropy(new BitString("ad30faee396c9da2b7a94094d39f"));

            //subject.Reseed(false, new BitString(0));
            subject.Generate(parameters.ReturnedBitsLen, new BitString(0));
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString(0));

            var expectation = new BitString("eedfcafe99418eddbf3f72d9620d6502f60cb1b89acbd00177d697f41b1f9d3d");

            Assert.AreEqual(expectation.ToHex(), result.Bits.ToHex());
        }

        [Test]
        public void ShouldAes128NoReseedNoAddInput()
        {
            var entropyProvider = new TestableEntropyProvider();
            var factory = new DrbgFactory();
            var parameters = new DrbgParameters
            {
                PredResistanceEnabled = true,
                EntropyInputLen = 128,
                NonceLen = 64,
                PersoStringLen = 0,
                AdditionalInputLen = 0,
                ReturnedBitsLen = 512,
                DerFuncEnabled = true,
                Mechanism = DrbgMechanism.Counter,
                Mode = DrbgMode.AES128,
                ReseedImplemented = false
            };

            var subject = factory.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(new BitString("9605714ae47145fc9612d2b031122ce6")); // Entropy Input
            entropyProvider.AddEntropy(new BitString("65ddb5cda412f14d")); // Nonce
            subject.Instantiate(128, new BitString(0));

            entropyProvider.AddEntropy(new BitString("9c0de210427a30f05f7ed1bf14d0d377"));
            entropyProvider.AddEntropy(new BitString("bcbdfc86694bad03cb75d5ac3cb8dd1e"));

            subject.Generate(parameters.ReturnedBitsLen, new BitString(0));
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString(0));

            var expectation = new BitString("b7ab81ed6e9562f81850c6e94e1cbc1339c7dfe09bbc31a7d7bf876f11a2a56f8640f30be79a0dc2c055a750c9b4b3dff61eb4b240c2e8793873b8813002db2a");

            Assert.AreEqual(expectation.ToHex(), result.Bits.ToHex());
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

        [Test]
        public void ShouldAes128DfPredResistNoReseed()
        {
            TestableEntropyProvider entropyProvider = new TestableEntropyProvider();

            DrbgFactory factory = new DrbgFactory();

            DrbgParameters parameters = new DrbgParameters()
            {
                PredResistanceEnabled = true,
                EntropyInputLen = 128,
                NonceLen = 64,
                PersoStringLen = 0,
                AdditionalInputLen = 0,
                ReturnedBitsLen = 512,
                DerFuncEnabled = true,
                Mechanism = DrbgMechanism.Counter,
                Mode = DrbgMode.AES128,
                ReseedImplemented = false
            };

            var subject = factory.GetDrbgInstance(parameters, entropyProvider);

            entropyProvider.AddEntropy(new BitString("66ae5f101636bd4e0ae6cbd08b63d536")); // Entropy Input
            entropyProvider.AddEntropy(new BitString("18d1f9b15079bdf8")); // Nonce
            subject.Instantiate(128, new BitString(0));

            entropyProvider.AddEntropy(new BitString("8948ec6a48135bfd2637b325617d8304"));
            entropyProvider.AddEntropy(new BitString("33f961fbf39b621145db2b61bd7be663"));

            //subject.Reseed(false, new BitString("005debda17ad3a6b8600314fd19be0871da2f4f3f58946e6745554f79d0915bb5ea0f5fe2eb794d44742c1e14461cd87"));
            subject.Generate(parameters.ReturnedBitsLen, new BitString(0));
            var result = subject.Generate(parameters.ReturnedBitsLen, new BitString(0));

            var expectation = new BitString("fd6f5ae6a2f1dd244b12052cfbbccddda0d0f950fbdce6e1e3817f4122bec3268d0aa3a88424c1b1739613dd669fa45ea9b44779869354738010785c8eee4d21");

            Assert.AreEqual(expectation, result.Bits);
        }
    }
}
