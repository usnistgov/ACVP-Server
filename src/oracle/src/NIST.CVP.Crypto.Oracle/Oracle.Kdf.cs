using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.ANSIX963;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.Common.KDF.Components.IKEv1.Enums;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.IKEv1;
using NIST.CVP.Crypto.IKEv2;
using NIST.CVP.Crypto.KDF;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.SNMP;
using NIST.CVP.Crypto.SRTP;
using NIST.CVP.Crypto.SSH;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Crypto.TLS;
using NIST.CVP.Crypto.TPM;
using NIST.CVP.Math;
using System;
using System.Threading.Tasks;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private readonly KdfFactory _kdfFactory = new KdfFactory(new CmacFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory()), new HmacFactory(new ShaFactory()));
        private readonly AnsiX963Factory _ansiFactory = new AnsiX963Factory(new ShaFactory());
        private readonly IkeV1Factory _ikeV1Factory = new IkeV1Factory();
        private readonly IkeV2Factory _ikeV2Factory = new IkeV2Factory(new HmacFactory(new ShaFactory()));
        private readonly SnmpFactory _snmpFactory = new SnmpFactory();
        private readonly SrtpFactory _srtpFactory = new SrtpFactory();
        private readonly SshFactory _sshFactory = new SshFactory();
        private readonly TlsKdfFactory _tlsFactory = new TlsKdfFactory();
        private readonly TpmFactory _tpmFactory = new TpmFactory(new HmacFactory(new ShaFactory()));

        private KdfResult GetDeferredKdfCase(KdfParameters param)
        {
            var rand = new Random800_90();
            return new KdfResult
            {
                KeyIn = rand.GetRandomBitString(128),
                Iv = rand.GetRandomBitString(param.ZeroLengthIv ? 0 : 128),
                FixedData = rand.GetRandomBitString(128),
                BreakLocation = rand.GetRandomInt(1, 128)
            };
        }

        private KdfResult CompleteDeferredKdfCase(KdfParameters param, KdfResult fullParam)
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

        private AnsiX963KdfResult GetAnsiX963KdfCase(AnsiX963Parameters param)
        {
            var rand = new Random800_90();
            var ansi = _ansiFactory.GetInstance(param.HashAlg);

            var z = rand.GetRandomBitString(param.FieldSize);
            var sharedInfo = rand.GetRandomBitString(param.SharedInfoLength);

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

        private IkeV1KdfResult GetIkeV1KdfCase(IkeV1KdfParameters param)
        {
            var rand = new Random800_90();
            var ike = _ikeV1Factory.GetIkeV1Instance(param.AuthenticationMethod, param.HashAlg);

            var nInit = rand.GetRandomBitString(param.NInitLength);
            var nResp = rand.GetRandomBitString(param.NRespLength);
            var ckyInit = rand.GetRandomBitString(64);
            var ckyResp = rand.GetRandomBitString(64);
            var gxy = rand.GetRandomBitString(param.GxyLength);
            var preSharedKey = param.AuthenticationMethod == AuthenticationMethods.Psk ? rand.GetRandomBitString(param.PreSharedKeyLength) : null;

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

        private IkeV2KdfResult GetIkeV2KdfCase(IkeV2KdfParameters param)
        {
            var rand = new Random800_90();
            var ike = _ikeV2Factory.GetInstance(param.HashAlg);

            var nInit = rand.GetRandomBitString(param.NInitLength);
            var nResp = rand.GetRandomBitString(param.NRespLength);
            var gir = rand.GetRandomBitString(param.GirLength);
            var girNew = rand.GetRandomBitString(param.GirLength);
            var spiInit = rand.GetRandomBitString(64);
            var spiResp = rand.GetRandomBitString(64);

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

        private SnmpKdfResult GetSnmpKdfCase(SnmpKdfParameters param)
        {
            var rand = new Random800_90();
            var password = rand.GetRandomAlphaCharacters(param.PasswordLength);

            var result = _snmpFactory.GetInstance().KeyLocalizationFunction(param.EngineId, password);
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

        private SrtpKdfResult GetSrtpKdfCase(SrtpKdfParameters param)
        {
            var rand = new Random800_90();
            var key = rand.GetRandomBitString(param.AesKeyLength);
            var salt = rand.GetRandomBitString(112);
            var index = rand.GetRandomBitString(48);
            var srtcpIndex = rand.GetRandomBitString(32);

            var result = _srtpFactory.GetInstance()
                .DeriveKey(param.AesKeyLength, key, salt, param.KeyDerivationRate, index, srtcpIndex);
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

        private SshKdfResult GetSshKdfCase(SshKdfParameters param)
        {
            var rand = new Random800_90();
            var ssh = _sshFactory.GetSshInstance(param.HashAlg, param.Cipher);

            var k = rand.GetRandomBitString(2048);

            // If the MSbit is a 1, append "00" to the front
            if (k.GetMostSignificantBits(1).Equals(BitString.One()))
            {
                k = BitString.Zeroes(8).ConcatenateBits(k);
            }

            // Append the length (32-bit) to the front (in bytes, so 256 or 257 bytes)
            var fullK = BitString.To32BitString(k.BitLength / 8).ConcatenateBits(k);

            var h = rand.GetRandomBitString(param.HashAlg.OutputLen);
            var sessionId = rand.GetRandomBitString(param.HashAlg.OutputLen);

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

        private TlsKdfResult GetTlsKdfCase(TlsKdfParameters param)
        {
            var rand = new Random800_90();
            var tls = _tlsFactory.GetTlsKdfInstance(param.TlsMode, param.HashAlg);

            var preMasterSecret = rand.GetRandomBitString(param.PreMasterSecretLength);
            var clientHelloRandom = rand.GetRandomBitString(256);
            var serverHelloRandom = rand.GetRandomBitString(256);
            var clientRandom = rand.GetRandomBitString(256);
            var serverRandom = rand.GetRandomBitString(256);

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

        private TpmKdfResult GetTpmKdfCase()
        {
            var rand = new Random800_90();
            var tpm = _tpmFactory.GetTpm();

            var auth = rand.GetRandomBitString(160);
            var nonceEven = rand.GetRandomBitString(160);
            var nonceOdd = rand.GetRandomBitString(160);

            var result = tpm.DeriveKey(auth, nonceEven, nonceOdd);
            if (!result.Success)
            {
                throw new Exception();
            }

            return new TpmKdfResult
            {
                Auth = auth,
                NonceEven = nonceEven,
                NonceOdd = nonceOdd,
                SKey = result.SKey
            };
        }

        public async Task<KdfResult> GetDeferredKdfCaseAsync(KdfParameters param)
        {
            return await _taskFactory.StartNew(() => GetDeferredKdfCase(param));
        }

        public async Task<KdfResult> CompleteDeferredKdfCaseAsync(KdfParameters param, KdfResult fullParam)
        {
            return await _taskFactory.StartNew(() => CompleteDeferredKdfCase(param, fullParam));
        }

        public async Task<AnsiX963KdfResult> GetAnsiX963KdfCaseAsync(AnsiX963Parameters param)
        {
            return await _taskFactory.StartNew(() => GetAnsiX963KdfCase(param));
        }

        public async Task<IkeV1KdfResult> GetIkeV1KdfCaseAsync(IkeV1KdfParameters param)
        {
            return await _taskFactory.StartNew(() => GetIkeV1KdfCase(param));
        }

        public async Task<IkeV2KdfResult> GetIkeV2KdfCaseAsync(IkeV2KdfParameters param)
        {
            return await _taskFactory.StartNew(() => GetIkeV2KdfCase(param));
        }

        public async Task<SnmpKdfResult> GetSnmpKdfCaseAsync(SnmpKdfParameters param)
        {
            return await _taskFactory.StartNew(() => GetSnmpKdfCase(param));
        }

        public async Task<SrtpKdfResult> GetSrtpKdfCaseAsync(SrtpKdfParameters param)
        {
            return await _taskFactory.StartNew(() => GetSrtpKdfCase(param));
        }

        public async Task<SshKdfResult> GetSshKdfCaseAsync(SshKdfParameters param)
        {
            return await _taskFactory.StartNew(() => GetSshKdfCase(param));
        }

        public async Task<TlsKdfResult> GetTlsKdfCaseAsync(TlsKdfParameters param)
        {
            return await _taskFactory.StartNew(() => GetTlsKdfCase(param));
        }

        public async Task<TpmKdfResult> GetTpmKdfCaseAsync()
        {
            return await _taskFactory.StartNew(GetTpmKdfCase);
        }
    }
}
