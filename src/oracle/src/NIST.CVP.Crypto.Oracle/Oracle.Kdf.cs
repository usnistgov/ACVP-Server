using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.ANSIX963;
using NIST.CVP.Crypto.Common.KDF.Components.IKEv1.Enums;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.IKEv1;
using NIST.CVP.Crypto.IKEv2;
using NIST.CVP.Crypto.KDF;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.SNMP;
using NIST.CVP.Crypto.SRTP;
using NIST.CVP.Crypto.SSH;
using NIST.CVP.Crypto.TLS;
using NIST.CVP.Math;
using System;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private readonly KdfFactory _kdfFactory = new KdfFactory();
        private readonly AnsiX963Factory _ansiFactory = new AnsiX963Factory(new ShaFactory());
        private readonly IkeV1Factory _ikeV1Factory = new IkeV1Factory();
        private readonly IkeV2Factory _ikeV2Factory = new IkeV2Factory(new HmacFactory(new ShaFactory()));
        private readonly Snmp _snmp = new Snmp();
        private readonly Srtp _srtp = new Srtp();
        private readonly SshFactory _sshFactory = new SshFactory();
        private readonly TlsKdfFactory _tlsFactory = new TlsKdfFactory();

        public KdfResult GetDeferredKdfCase(KdfParameters param)
        {
            return new KdfResult
            {
                KeyIn = _rand.GetRandomBitString(128),
                Iv = _rand.GetRandomBitString(param.ZeroLengthIv ? 0 : 128),
                FixedData = _rand.GetRandomBitString(128),
                BreakLocation = _rand.GetRandomInt(1, 128)
            };
        }

        public KdfResult CompleteDeferredKdfCase(KdfParameters param, KdfResult fullParam)
        {
            var kdf = _kdfFactory.GetKdfInstance(param.Mode, param.MacMode, param.CounterLocation, param.CounterLength);

            var result = kdf.DeriveKey(fullParam.KeyIn, fullParam.FixedData, param.KeyOutLength, fullParam.Iv, fullParam.BreakLocation);

            if (!result.Success)
            {
                throw new Exception();
            }

            return new KdfResult
            {
                KeyOut = result.DerivedKey
            };
        }

        public AnsiX963KdfResult GetAnsiX963KdfCase(AnsiX963Parameters param)
        {
            var ansi = _ansiFactory.GetInstance(param.HashAlg);

            var z = _rand.GetRandomBitString(param.FieldSize);
            var sharedInfo = _rand.GetRandomBitString(param.SharedInfoLength);

            var result = ansi.DeriveKey(z, sharedInfo, param.KeyDataLength);
            if (!result.Success)
            {
                throw new Exception();
            }

            return new AnsiX963KdfResult
            {
                Z = z,
                SharedInfo = sharedInfo,
                KeyOut = result.DerivedKey
            };
        }

        public IkeV1KdfResult GetIkeV1KdfCase(IkeV1KdfParameters param)
        {
            var ike = _ikeV1Factory.GetIkeV1Instance(param.AuthenticationMethod, param.HashAlg);

            var nInit = _rand.GetRandomBitString(param.NInitLength);
            var nResp = _rand.GetRandomBitString(param.NRespLength);
            var ckyInit = _rand.GetRandomBitString(64);
            var ckyResp = _rand.GetRandomBitString(64);
            var gxy = _rand.GetRandomBitString(param.GxyLength);
            var preSharedKey = param.AuthenticationMethod == AuthenticationMethods.Psk ? _rand.GetRandomBitString(param.PreSharedKeyLength) : null;

            var result = ike.GenerateIke(nInit, nResp, gxy, ckyInit, ckyResp, preSharedKey);
            if (!result.Success)
            {
                throw new Exception();
            }

            return new IkeV1KdfResult
            {
                CkyInit = ckyInit,
                CkyResp = ckyResp,
                Gxy = gxy,
                NInit = nInit,
                NResp = nResp,
                PreSharedKey = preSharedKey,
                sKeyId = result.SKeyId,
                sKeyIdA = result.SKeyIdA,
                sKeyIdD = result.SKeyIdD,
                sKeyIdE = result.SKeyIdE
            };
        }

        public IkeV2KdfResult GetIkeV2KdfCase(IkeV2KdfParameters param)
        {
            var ike = _ikeV2Factory.GetInstance(param.HashAlg);

            var nInit = _rand.GetRandomBitString(param.NInitLength);
            var nResp = _rand.GetRandomBitString(param.NRespLength);
            var gir = _rand.GetRandomBitString(param.GirLength);
            var girNew = _rand.GetRandomBitString(param.GirLength);
            var spiInit = _rand.GetRandomBitString(64);
            var spiResp = _rand.GetRandomBitString(64);

            var result = ike.GenerateIke(nInit, nResp, gir, girNew, spiInit, spiResp, param.DerivedKeyingMaterialLength);
            if (!result.Success)
            {
                throw new Exception();
            }

            return new IkeV2KdfResult
            {
                DerivedKeyingMaterial = result.DKM,
                NResp = nResp,
                NInit = nInit,
                DerivedKeyingMaterialChild = result.DKMChildSA,
                DerivedKeyingMaterialDh = result.DKMChildSADh,
                Gir = gir,
                GirNew = girNew,
                SKeySeed = result.SKeySeed,
                SKeySeedReKey = result.SKeySeedReKey,
                SpiInit = spiInit,
                SpiResp = spiResp
            };
        }

        public SnmpKdfResult GetSnmpKdfCase(SnmpKdfParameters param)
        {
            var password = _rand.GetRandomAlphaCharacters(param.PasswordLength);

            var result = _snmp.KeyLocalizationFunction(param.EngineId, password);
            if (!result.Success)
            {
                throw new Exception();
            }

            return new SnmpKdfResult
            {
                Password = password,
                SharedKey = result.SharedKey
            };
        }

        public SrtpKdfResult GetSrtpKdfCase(SrtpKdfParameters param)
        {
            var key = _rand.GetRandomBitString(param.AesKeyLength);
            var salt = _rand.GetRandomBitString(112);
            var index = _rand.GetRandomBitString(48);
            var srtcpIndex = _rand.GetRandomBitString(32);

            var result = _srtp.DeriveKey(param.AesKeyLength, key, salt, param.KeyDerivationRate, index, srtcpIndex);
            if (!result.Success)
            {
                throw new Exception();
            }

            return new SrtpKdfResult
            {
                Index = index,
                MasterKey = key,
                MasterSalt = salt,
                SrtcpIndex = srtcpIndex,
                SrtcpAuthenticationKey = result.SrtcpResult.AuthenticationKey,
                SrtcpEncryptionKey = result.SrtcpResult.EncryptionKey,
                SrtcpSaltingKey = result.SrtcpResult.SaltingKey,
                SrtpAuthenticationKey = result.SrtpResult.AuthenticationKey,
                SrtpEncryptionKey = result.SrtpResult.EncryptionKey,
                SrtpSaltingKey = result.SrtpResult.SaltingKey
            };
        }

        public SshKdfResult GetSshKdfCase(SshKdfParameters param)
        {
            var ssh = _sshFactory.GetSshInstance(param.HashAlg, param.Cipher);

            var k = _rand.GetRandomBitString(2048);

            // If the MSbit is a 1, append "00" to the front
            if (k.GetMostSignificantBits(1).Equals(BitString.One()))
            {
                k = BitString.Zeroes(8).ConcatenateBits(k);
            }

            // Append the length (32-bit) to the front (in bytes, so 256 or 257 bytes)
            var fullK = BitString.To32BitString(k.BitLength / 8).ConcatenateBits(k);

            var h = _rand.GetRandomBitString(param.HashAlg.OutputLen);
            var sessionId = _rand.GetRandomBitString(param.HashAlg.OutputLen);

            var result = ssh.DeriveKey(fullK, h, sessionId);
            if (!result.Success)
            {
                throw new Exception();
            }

            return new SshKdfResult
            {
                H = h,
                K = fullK,
                SessionId = sessionId,
                EncryptionKeyClient = result.ClientToServer.EncryptionKey,
                EncryptionKeyServer = result.ServerToClient.EncryptionKey,
                InitialIvClient = result.ClientToServer.InitialIv,
                InitialIvServer = result.ServerToClient.InitialIv,
                IntegrityKeyClient = result.ClientToServer.IntegrityKey,
                IntegrityKeyServer = result.ServerToClient.IntegrityKey
            };
        }

        public TlsKdfResult GetTlsKdfCase(TlsKdfParameters param)
        {
            var tls = _tlsFactory.GetTlsKdfInstance(param.TlsMode, param.HashAlg);

            var preMasterSecret = _rand.GetRandomBitString(param.PreMasterSecretLength);
            var clientHelloRandom = _rand.GetRandomBitString(256);
            var serverHelloRandom = _rand.GetRandomBitString(256);
            var clientRandom = _rand.GetRandomBitString(256);
            var serverRandom = _rand.GetRandomBitString(256);

            var result = tls.DeriveKey(preMasterSecret, clientHelloRandom, serverHelloRandom, clientRandom, serverRandom, param.KeyBlockLength);
            if (!result.Success)
            {
                throw new Exception();
            }

            return new TlsKdfResult
            {
                PreMasterSecret = preMasterSecret,
                ClientHelloRandom = clientHelloRandom,
                ClientRandom = clientRandom,
                KeyBlock = result.DerivedKey,
                MasterSecret = result.MasterSecret,
                ServerHelloRandom = serverHelloRandom,
                ServerRandom = serverRandom
            };
        }
    }
}
