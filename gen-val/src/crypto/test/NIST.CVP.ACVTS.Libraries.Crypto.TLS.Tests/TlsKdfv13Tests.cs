using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS;
using NIST.CVP.ACVTS.Libraries.Crypto.HKDF;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.TLS.Tests
{
    [TestFixture, FastCryptoTest]
    public class TlsKdfv13Tests
    {
        private readonly ITLsKdfFactory_v1_3 _tlsFactory =
            new TlsKdfFactoryV13(new HkdfFactory(new HmacFactory(new NativeShaFactory())), new NativeShaFactory());

        /// <summary>
        /// Test from https://tools.ietf.org/html/draft-ietf-tls-tls13-vectors-05#page-3 "Simple 1-RTT Handshake"
        /// </summary>
        [Test]
        public void TestVectorIetfDraft_Simple1RttHandshake()
        {
            var kdf = (TlsKdfv13)_tlsFactory.GetInstance(HashFunctions.Sha2_d256);
            var hashLengthBytes = 256 / 8;

            var clientHello = new BitString("16 03 01 00 be 01 00 00 ba 03 03 01 6a 95 72 55 63 a4 a5 2c 6a ae 5b 86 f8 ec a3 21 a9 a3 57 48 1e b7 84 7e 9a 9d a4 12 20 b6 66 00 00 06 13 01 13 03 13 02 01 00 00 8b 00 00 00 0b 00 09 00 00 06 73 65 72 76 65 72 ff 01 00 01 00 00 0a 00 14 00 12 00 1d 00 17 00 18 00 19 01 00 01 01 01 02 01 03 01 04 00 23 00 00 00 33 00 26 00 24 00 1d 00 20 2e 59 6f fe 6d 68 c4 f4 02 cb 0f 49 84 1f 11 f1 ff 97 32 1d 32 42 54 d3 18 52 9a 77 cc d9 88 06 00 2b 00 03 02 7f 1c 00 0d 00 20 00 1e 04 03 05 03 06 03 02 03 08 04 08 05 08 06 04 01 05 01 06 01 02 01 04 02 05 02 06 02 02 02 00 2d 00 02 01 01");
            var serverHello = BitString.Empty();

            var early = kdf.GetDerivedEarlySecret(
                true,
                new BitString(32 * 8),
                clientHello);

            Assert.That(
                early.EarlySecret.ToHex(), Is.EqualTo(new BitString("33 ad 0a 1c 60 7e c0 3b 09 e6 cd 98 93 68 0c e2 10 ad f3 00 aa 1f 26 60 e1 b2 2e 10 f1 70 f9 2a").ToHex()),
                "Early Secret");

            var deriveSecretForHandshake = kdf.ExpandLabel(
                early.EarlySecret,
                "derived",
                new BitString("e3 b0 c4 42 98 fc 1c 14 9a fb f4 c8 99 6f b9 24 27 ae 41 e4 64 9b 93 4c a4 95 99 1b 78 52 b8 55"),
                hashLengthBytes);

            Assert.That(
                deriveSecretForHandshake.ToHex(), Is.EqualTo(new BitString("6f 26 15 a1 08 c7 02 c5 67 8f 54 fc 9d ba b6 97 16 c0 76 18 9c 48 25 0c eb ea c3 57 6c 36 11 ba").ToHex()),
                "Derive secret for handshake");

            var dhSharedSecret =
                new BitString("0b c3 7c 6e 7c 83 66 38 4b ad d8 e9 00 57 b9 c2 39 21 3e 19 8e f3 95 aa 2d 69 0a ae 1b 4e 9a 44");
            var handshakeFromEarly = kdf.GetDerivedHandshakeSecret(
                dhSharedSecret,
                early.DerivedEarlySecret,
                clientHello,
                serverHello);
            var handshake = kdf.GetDerivedHandshakeSecret(
                dhSharedSecret,
                deriveSecretForHandshake,
                clientHello,
                serverHello);

            Assert.That(handshake.HandshakeSecret.ToHex(), Is.EqualTo(handshakeFromEarly.HandshakeSecret.ToHex()), "Check handshake secret through normal process vs specific test.");

            var extractSecretHandshakeExpected = new BitString("ee ef ce 91 5d c4 8b 22 a7 ae 76 4a d2 82 ba 41 6f 97 fe 89 e5 d1 bc 89 5b 2d 91 62 35 aa a2 ae");
            Assert.That(handshakeFromEarly.HandshakeSecret, Is.EqualTo(extractSecretHandshakeExpected), "Extracted Handshake Secret");

            var deriveSecretClientHandshakeTraffic = kdf.ExpandLabel(
                handshakeFromEarly.HandshakeSecret,
                "c hs traffic",
                new BitString("df 94 98 64 2c c0 b3 7f 60 42 53 bf 34 1b b0 44 8e 3d b5 f5 c8 ab b2 39 31 9b 1c 7b 7b 2e ac 63"),
                hashLengthBytes);

            Assert.That(deriveSecretClientHandshakeTraffic, Is.EqualTo(new BitString("a4 d4 cd ed fb 3c 07 d7 be 78 85 8c 0b 63 38 eb 48 02 f1 58 88 ad 14 c1 ef 56 20 74 35 84 06 04")),
                "c hs traffic");
        }

        [Test]
        public void TestVectorIetfDraft_TranscriptHashEmptyPayload()
        {
            var shaFactory = new NativeShaFactory();
            var sha = shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256));
            var tls = (TlsKdfv13)_tlsFactory.GetInstance(HashFunctions.Sha2_d256);

            var emptyPayload = sha.HashMessage(BitString.Empty());

            var expectedHash =
                new BitString("e3 b0 c4 42 98 fc 1c 14 9a fb f4 c8 99 6f b9 24 27 ae 41 e4 64 9b 93 4c a4 95 99 1b 78 52 b8 55");

            Assert.That(emptyPayload.Digest.ToHex(), Is.EqualTo(expectedHash.ToHex()), nameof(emptyPayload));
            Assert.That(tls.TranscriptHash(BitString.Empty()).ToHex(), Is.EqualTo(expectedHash.ToHex()), "Transcript hash");
        }

        [Test]
        public void TestVectorIetfDraft_TranscriptHashNonEmptyPayloadOneContribution()
        {
            var shaFactory = new NativeShaFactory();
            var sha = shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256));
            var tls = (TlsKdfv13)_tlsFactory.GetInstance(HashFunctions.Sha2_d256);

            var clientHello = new BitString("01 00 01 fc 03 03 eb ef 0b 92 25 8b ec d1 07 3d cf f0 bb a7 da ad c7 b4 e8 14 df dd 1b 77 4b 0d 43 53 95 2b c4 2b 00 00 06 13 01 13 03 13 02 01 00 01 cd 00 00 00 0b 00 09 00 00 06 73 65 72 76 65 72 ff 01 00 01 00 00 0a 00 14 00 12 00 1d 00 17 00 18 00 19 01 00 01 01 01 02 01 03 01 04 00 33 00 26 00 24 00 1d 00 20 a2 e0 04 93 2f 3c d0 b3 c6 a2 9a de 11 8b 46 7c 69 55 a6 c3 6a 1d 44 27 38 60 59 b2 26 f5 0c 0f 00 2a 00 00 00 2b 00 03 02 7f 1c 00 0d 00 20 00 1e 04 03 05 03 06 03 02 03 08 04 08 05 08 06 04 01 05 01 06 01 02 01 04 02 05 02 06 02 02 02 00 2d 00 02 01 01 00 15 00 5d 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 29 00 dd 00 b8 00 b2 0f 63 7d a7 09 04 33 70 d0 60 00 06 00 00 00 00 2d fe b5 7a a8 7b 9c f1 76 0a 8a b4 91 d4 fb 0f 00 70 3d 7a 42 b6 a9 87 ef d2 4a fb bd 2b c6 06 9d c9 03 d4 c2 d3 f0 4f dd 3d 8e 95 97 0a 7b 78 aa 2c e8 28 75 72 4f 8a 82 75 d1 65 e7 7b e4 7d 59 0e aa ab fa 5f 4c 2d f0 46 71 a0 44 d8 4c f5 cc da c5 88 7d 6b e7 fe 2e 52 80 d7 a5 0f 23 fc 9c d4 a5 43 01 9e 41 94 63 c4 ee 29 8f d3 2c 01 93 34 b7 ab bb 78 d4 f2 a1 cf 4e 0f e1 60 aa 72 86 19 3f da 28 8c 97 d5 ba 39 75 5f 25 b7 a4 a8 f0 63 01 24 88 3d 2c 66 78 78 75 d6 7a 0f 6e b0 ba 71 f4 34 71 a5 00 21 20 b1 da ce 1d 97 d7 ff bf 46 1d f9 4d ec 70 f1 30 08 f9 13 4b 9c c0 40 88 d9 6d 93 cf 73 18 5b d8");

            var expectedHash = new BitString("8a ec fe eb b4 23 6e fd 8b 78 bb 3f f1 c7 af e0 87 2b fb b2 60 0f 04 69 ed 58 6f 23 39 7a e0 2d");

            Assert.That(
                sha.HashMessage(clientHello).Digest.ToHex(), Is.EqualTo(expectedHash.ToHex()),
                "concatenate no lengths");
            Assert.That(tls.TranscriptHash(clientHello).ToHex(), Is.EqualTo(expectedHash.ToHex()), "Transcript hash");
        }

        [Test]
        public void TestVectorIetfDraft_TranscriptHashNonEmptyPayloadTwoContribution()
        {
            var shaFactory = new NativeShaFactory();
            var sha = shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256));
            var tls = (TlsKdfv13)_tlsFactory.GetInstance(HashFunctions.Sha2_d256);

            var clientHello = new BitString("01 00 00 b6 03 03 82 97 3b d3 3b b4 81 f5 37 de c6 5a cd 48 5b d4 bd aa 20 f7 d2 2f 68 0c 89 2f 68 45 06 51 a5 0e 00 00 06 13 01 13 03 13 02 01 00 00 87 00 00 00 0b 00 09 00 00 06 73 65 72 76 65 72 ff 01 00 01 00 00 0a 00 14 00 12 00 1d 00 17 00 18 00 19 01 00 01 01 01 02 01 03 01 04 00 33 00 26 00 24 00 1d 00 20 79 fd 6e fb c1 92 04 40 aa 32 5c dc ea 3f 3c b7 07 8f ea 03 13 fa 76 6a c3 76 1e dc 62 ad 2c 31 00 2b 00 03 02 7f 1c 00 0d 00 20 00 1e 04 03 05 03 06 03 02 03 08 04 08 05 08 06 04 01 05 01 06 01 02 01 04 02 05 02 06 02 02 02 00 2d 00 02 01 01");
            var serverHello = new BitString("02 00 00 56 03 03 e1 6b 86 5e 76 5e 84 ba 47 b4 2d f2 62 e3 8e 2d e6 1e 95 e3 75 3b ad fd 98 76 5c 62 98 4f 28 d3 00 13 01 00 00 2e 00 33 00 24 00 1d 00 20 c3 ec 4f 42 40 70 ce 83 c7 91 fa 32 8f e9 ae 00 96 ab fc cc 15 b9 aa ec eb f6 0b f4 8f 0b 0f 2e 00 2b 00 02 7f 1c");

            var expectedHash = new BitString("57 65 19 76 4b f9 ac e3 84 32 c8 6d 9e 0f 72 f2 ef 6b a3 7c 9f 76 30 6e fc bb e7 78 56 ad b3 41");

            Assert.That(
                sha.HashMessage(clientHello.ConcatenateBits(serverHello)).Digest.ToHex(), Is.EqualTo(expectedHash.ToHex()),
                "concatenate no lengths");
            Assert.That(tls.TranscriptHash(clientHello, serverHello).ToHex(), Is.EqualTo(expectedHash.ToHex()), "Transcript hash");
        }

        [Test]
        public void WhenGivenDeployedRefactoredVectorWithNewTranscriptHash_ShouldPassThroughCrypto()
        {
            var tls = _tlsFactory.GetInstance(HashFunctions.Sha2_d256);

            var clientHello = new BitString("43E50D9385348E472D7437069F11F4BC9A9E91419F4D181B684D7FDCCB283C6F9B77FDEB");
            var serverHello = new BitString("28590A320ED4C84B4D83FAC83794D7EBF7C2575151416A3D0F5FD69E557FC6F4BB801945");
            var clientFinished = new BitString("7F89C4A133CCBC88862F2AB141B2ACEFC7A3EBB245C298946900A50B635F63D62F83CCB1");
            var serverFinished = new BitString("6F41191133DC5101D60A3243F9F348856E57ED4B3B33E24DDDEF64D83000BB74F6A5E210");

            var psk = new BitString("7EF1F0E2830A8B277B3EEBD55C911CF8EFB4BC40D18C8575161252B99CB2458BE7504EB6");
            var dhe = new BitString("0000000000000000000000000000000000000000000000000000000000000000");

            var expectedResumption = new BitString("42D6655172F7A28942566EF46574B51CB7DC9E8772E24A4C0695663F7A62E5C3");

            var result = tls.GetFullKdf(
                false, psk, dhe,
                clientHello, serverHello, serverFinished, clientFinished);

            Assert.That(result.MasterSecretResult.ResumptionMasterSecret.ToHex(), Is.EqualTo(expectedResumption.ToHex()));
        }

        [Test]
        public void WhenIntegrationTest_ShouldPassThroughCrypto()
        {
            var tls = _tlsFactory.GetInstance(HashFunctions.Sha2_d256);

            var clientHello = new BitString("80ABF9173154BB77CCB228E6F1C9EBA5190939B97BF376C3B1D25110FEE69BD430");
            var serverHello = new BitString("CBCA99A1FC54861AE8AC4A7E657480B5E719ADE631188436E2AB6CE9AAFCBF3ABD");
            var clientFinished = new BitString("12CF6F665ADEF8668E7E5694523758C146F47F4E089B6E46F5C346DA9F2E28BF4D");
            var serverFinished = new BitString("B99C40AE1862901313F03F1B367862493B0E919A99A3AF0F1E7FBC6E0C797EBD72");

            var psk = new BitString("702F6026CEF0526761258F48E8B4FC7CE79198936B2DB00B1FE621A723D9F14170");
            var dhe = new BitString("0000000000000000000000000000000000000000000000000000000000000000");

            var expectedResumption = new BitString("02CD04B88A20035376B33C2E7BD9F2E4DDE551A645B05DD90B6AA932BBB429DB");

            var result = tls.GetFullKdf(
                false, psk, dhe,
                clientHello, serverHello, serverFinished, clientFinished);

            Assert.That(result.MasterSecretResult.ResumptionMasterSecret.ToHex(), Is.EqualTo(expectedResumption.ToHex()));
        }

        // [Test]
        // public void TestVectorIetfDraft_TranscriptHashHelloRetry()
        // {
        // 	var shaFactory = new ShaFactory();
        // 	var sha = shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256));
        // 	var tls = (TlsKdfv13)_tlsFactory.GetInstance(HashFunctions.Sha2_d256);
        // 	
        // 	var clientHello = new BitString("01 00 01 fc 03 03 fd a5 c0 5a 01 de 6f 64 0f 13 2a 1a a8 b7 a0 5a 9f 17 91 ca 88 fd f1 ac 8e 07 5e 50 cf 69 0c c9 00 00 06 13 01 13 03 13 02 01 00 01 cd 00 00 00 0b 00 09 00 00 06 73 65 72 76 65 72 ff 01 00 01 00 00 0a 00 08 00 06 00 1d 00 17 00 18 00 33 00 47 00 45 00 17 00 41 04 9c 86 50 ec 41 c5 a8 df da c7 8b 1f 35 65 42 16 cf cf 8c 2d b5 09 31 58 59 3b 33 22 1a 60 4b f7 df f9 a4 7d cf 13 ee cb 29 be 5c 24 73 21 48 2f 44 51 57 b7 33 1e e4 af 71 7b 59 7e 07 6d 56 e9 00 2b 00 03 02 7f 1c 00 0d 00 20 00 1e 04 03 05 03 06 03 02 03 08 04 08 05 08 06 04 01 05 01 06 01 02 01 04 02 05 02 06 02 02 02 00 2c 00 74 00 72 be 27 61 a6 66 36 1c 81 90 47 cf 51 00 00 00 00 5a 99 8e 4c c3 d8 dd 02 5b bb e1 0d a6 f2 b2 d1 00 30 b0 3a 58 2f 9c c5 81 d1 0f 62 6c f0 e3 b9 3d 14 d4 65 f9 48 83 5a 2a b5 31 3a 23 a1 9a eb a3 67 1e 7a 0d 41 0e 17 4f d0 04 f6 53 f1 08 25 17 3d 1a 90 37 cd ea b4 86 df 4e 79 c6 87 f9 d9 b1 b9 e2 ae 81 1e 0b 97 4e 8f 82 7b b1 66 a8 2d f7 a1 00 2d 00 02 01 01 00 15 00 b5 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
        // 	var serverHello = new BitString("02 00 00 ac 03 03 cf 21 ad 74 e5 9a 61 11 be 1d 8c 02 1e 65 b8 91 c2 a2 11 16 7a bb 8c 5e 07 9e 09 e2 c8 a8 33 9c 00 13 01 00 00 84 00 33 00 02 00 17 00 2c 00 74 00 72 be 27 61 a6 66 36 1c 81 90 47 cf 51 00 00 00 00 5a 99 8e 4c c3 d8 dd 02 5b bb e1 0d a6 f2 b2 d1 00 30 b0 3a 58 2f 9c c5 81 d1 0f 62 6c f0 e3 b9 3d 14 d4 65 f9 48 83 5a 2a b5 31 3a 23 a1 9a eb a3 67 1e 7a 0d 41 0e 17 4f d0 04 f6 53 f1 08 25 17 3d 1a 90 37 cd ea b4 86 df 4e 79 c6 87 f9 d9 b1 b9 e2 ae 81 1e 0b 97 4e 8f 82 7b b1 66 a8 2d f7 a1 00 2b 00 02 7f 1c");
        //
        // 	var early = tls.GetDerivedEarlySecret(false, new BitString(32 * BitString.BITSINBYTE), clientHello);
        // 	
        // 	Assert.AreEqual(new BitString("6f 26 15 a1 08 c7 02 c5 67 8f 54 fc 9d ba b6 97 16 c0 76 18 9c 48 25 0c eb ea c3 57 6c 36 11 ba").ToHex(), early.DerivedEarlySecret.ToHex(), "Derived early sercret");
        //
        // 	var handshake = tls.GetDerivedHandshakeSecret(
        // 		new BitString("fe b0 20 4b f7 6c ce 95 68 ae ef fa 0b 10 ef c7 64 06 5c 03 48 cc f4 f2 f8 97 22 f2 f5 5c df a8"),
        // 		early.DerivedEarlySecret,
        // 		clientHello,
        // 		serverHello);
        // 	
        // 	Assert.AreEqual(
        // 		new BitString("91 35 3f 07 99 0d 6d 5a e0 43 f2 dd 4b 36 45 a8 2d d7 a4 8b 91 73 36 5c af 7e 09 80 ba f4 9d 15").ToHex(), 
        // 		handshake.HandshakeSecret.ToHex(), 
        // 		"Handshake secret");
        // 	Assert.AreEqual(
        // 		new BitString("66 65 be 10 30 f9 05 87 74 35 d5 6b 4a 9b d8 de 7f 4e 37 1c ef 29 5b ac 39 7b 98 d7 35 f5 16 54").ToHex(), 
        // 		handshake.ClientHandshakeTrafficSecret.ToHex(), 
        // 		"tls13 c hs traffic");
        // }
    }
}
