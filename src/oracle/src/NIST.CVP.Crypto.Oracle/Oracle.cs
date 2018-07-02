using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using System;
using System.Linq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Math;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Crypto.Symmetric.MonteCarlo;

namespace NIST.CVP.Crypto.Oracle
{
    public class Oracle : IOracle
    {
        private readonly Random800_90 _rand = new Random800_90();

        #region AEAD
        public AeadResult GetAesCcmCase() => throw new NotImplementedException();
        public AeadResult GetAesGcmCase() => throw new NotImplementedException();
        public AeadResult GetAesXpnCase() => throw new NotImplementedException();
        #endregion AEAD

        #region AES
        private AesResult DoSimpleAes(IModeBlockCipher<SymmetricCipherResult> cipher, AesParameters param)
        {
            var direction = BlockCipherDirections.Encrypt;
            if (param.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = _rand.GetRandomBitString(param.DataLength);
            var key = _rand.GetRandomBitString(param.KeyLength);
            var iv = _rand.GetRandomBitString(128);

            var blockCipherParams = new ModeBlockCipherParameters(direction, iv, key, payload);
            var result = cipher.ProcessPayload(blockCipherParams);

            if (!result.Success)
            {
                // Log error somewhere
                throw new Exception();
            }

            return new AesResult
            {
                PlainText = direction == BlockCipherDirections.Encrypt ? payload : result.Result,
                CipherText = direction == BlockCipherDirections.Decrypt ? payload : result.Result,
                Key = key,
                Iv = iv
            };
        }

        private MctResult<AesResult> DoSimpleAesMct(IMonteCarloTester<MCTResult<AlgoArrayResponse>, AlgoArrayResponse> cipher, AesParameters param)
        {
            var direction = BlockCipherDirections.Encrypt;
            if (param.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = _rand.GetRandomBitString(param.DataLength);
            var key = _rand.GetRandomBitString(param.KeyLength);
            var iv = _rand.GetRandomBitString(128);

            var blockCipherParams = new ModeBlockCipherParameters(direction, iv, key, payload);
            var result = cipher.ProcessMonteCarloTest(blockCipherParams);

            if (!result.Success)
            {
                // Log error somewhere
                throw new Exception();
            }

            return new MctResult<AesResult>
            {
                Results = Array.ConvertAll(result.Response.ToArray(), element => new AesResult
                {
                    Key = element.Key,
                    Iv = element.IV,
                    PlainText = element.PlainText,
                    CipherText = element.CipherText
                }).ToList()
            };
        }

        public AesResult GetAesCbcCase(AesParameters param)
        {
            // Check Pool first
            var cipher = new CbcBlockCipher(new AesEngine());
            return DoSimpleAes(cipher, param);
        }

        public AesResult GetAesEcbCase(AesParameters param)
        {
            // Check Pool first
            var cipher = new EcbBlockCipher(new AesEngine());
            return DoSimpleAes(cipher, param);
        }

        public AesResult GetAesOfbCase(AesParameters param)
        {
            // Check Pool first
            var cipher = new OfbBlockCipher(new AesEngine());
            return DoSimpleAes(cipher, param);
        }

        public AesResult GetAesXtsCase(AesParameters param)
        {
            // Check Pool first
            var cipher = new XtsBlockCipher(new AesEngine());
            return DoSimpleAes(cipher, param);
        }

        public AesResult GetAesCfbCase(AesParameters param) => throw new NotImplementedException();
        public AesResult GetAesCtrCase(AesParameters param) => throw new NotImplementedException();

        public MctResult<AesResult> GetAesCbcMctCase(AesParameters param)
        {
            var cipher = new MonteCarloAesCbc(new BlockCipherEngineFactory(), new ModeBlockCipherFactory(), new AesMonteCarloKeyMaker());
            return DoSimpleAesMct(cipher, param);
        }

        public MctResult<AesResult> GetAesEcbMctCase(AesParameters param)
        {
            var cipher = new MonteCarloAesEcb(new BlockCipherEngineFactory(), new ModeBlockCipherFactory(), new AesMonteCarloKeyMaker());
            return DoSimpleAesMct(cipher, param);
        }

        public MctResult<AesResult> GetAesOfbMctCase(AesParameters param)
        {
            var cipher = new MonteCarloAesOfb(new BlockCipherEngineFactory(), new ModeBlockCipherFactory(), new AesMonteCarloKeyMaker());
            return DoSimpleAesMct(cipher, param);
        }

        public MctResult<AesResult> GetAesCfbMctCase(AesParameters param) => throw new NotImplementedException();
        #endregion AES

        #region Drbg
        public DrbgResult GetDrbgCase() => throw new NotImplementedException();
        #endregion Drbg

        #region DSA
        public DsaDomainParametersResult GetDsaDomainParameters() => throw new NotImplementedException();
        public VerifyResult<DsaDomainParametersResult> GetDsaDomainParametersVerify() => throw new NotImplementedException();
        public DsaKeyResult GetDsaKey() => throw new NotImplementedException();
        public VerifyResult<DsaKeyResult> GetDsaKeyVerify() => throw new NotImplementedException();
        public DsaSignatureResult GetDsaSignature() => throw new NotImplementedException();
        public VerifyResult<DsaSignatureResult> GetDsaVerifyResult() => throw new NotImplementedException();
        #endregion DSA

        #region ECDSA
        // TODO same as DSA
        #endregion ECDSA

        #region Hash
        public HashResult GetSha1Case() => throw new NotImplementedException();
        public HashResult GetSha2Case() => throw new NotImplementedException();
        public HashResult GetSha3Case() => throw new NotImplementedException();
        public HashResult GetShakeCase() => throw new NotImplementedException();

        public MctResult<HashResult> GetSha1MctCase() => throw new NotImplementedException();
        public MctResult<HashResult> GetSha2MctCase() => throw new NotImplementedException();
        public MctResult<HashResult> GetSha3MctCase() => throw new NotImplementedException();
        public MctResult<HashResult> GetShakeMctCase() => throw new NotImplementedException();

        public HashResult GetShakeVotCase() => throw new NotImplementedException();
        #endregion Hash

        #region KAS
        // TODO
        #endregion KAS

        #region KDF
        public KdfResult GetKdfCase() => throw new NotImplementedException();
        // All the components individually probably, but those are straight-forward input/output
        #endregion KDF

        #region KeyWrap
        public AesResult GetAesKeyWrapCase() => throw new NotImplementedException();
        public TdesResult GetTdesKeyWrapCase() => throw new NotImplementedException();
        #endregion KeyWrap

        #region MAC
        public MacResult GetCmacCase() => throw new NotImplementedException();
        public MacResult GetHmacCase() => throw new NotImplementedException();
        #endregion MAC

        #region RSA
        public RsaKeyResult GetRsaKey() => throw new NotImplementedException();
        public VerifyResult<RsaKeyResult> GetRsaKeyVerify() => throw new NotImplementedException();
        public RsaSignatureResult GetRsaSignature() => throw new NotImplementedException();
        public VerifyResult<RsaSignatureResult> GetRsaVerify() => throw new NotImplementedException();
        #endregion RSA

        #region TDES
        public TdesResult GetTdesCbcCase() => throw new NotImplementedException();
        public TdesResult GetTdesCfbCase() => throw new NotImplementedException();
        public TdesResult GetTdesEcbCase() => throw new NotImplementedException();
        public TdesResult GetTdesOfbCase() => throw new NotImplementedException();

        public TdesResultWithIvs GetTdesCbcICase() => throw new NotImplementedException();
        public TdesResultWithIvs GetTdesOfbICase() => throw new NotImplementedException();

        public MctResult<TdesResult> GetTdesCbcMctCase() => throw new NotImplementedException();
        public MctResult<TdesResult> GetTdesCfbMctCase() => throw new NotImplementedException();
        public MctResult<TdesResult> GetTdesEcbMctCase() => throw new NotImplementedException();
        public MctResult<TdesResult> GetTdesOfbMctCase() => throw new NotImplementedException();

        public MctResult<TdesResultWithIvs> GetTdesCbcIMctCase() => throw new NotImplementedException();
        public MctResult<TdesResultWithIvs> GetTdesOfbIMctCase() => throw new NotImplementedException();
        #endregion TDES
    }
}
