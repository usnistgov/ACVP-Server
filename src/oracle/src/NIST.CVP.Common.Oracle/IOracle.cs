using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Common.Oracle
{
    public interface IOracle
    {
        #region AEAD
        AeadResult GetAesCcmCase();
        AeadResult GetAesGcmCase(AeadParameters param);
        AeadResult GetAesXpnCase(AeadParameters param);

        AeadResult GetDeferredAesGcmCase(AeadParameters param);
        AeadResult CompleteDeferredAesGcmCase(AeadParameters param, AeadResult fullParam);
        #endregion AEAD

        #region AES
        AesResult GetAesCbcCase(AesParameters param);
        AesResult GetAesCfbCase(AesParameters param);
        AesResult GetAesCtrCase(AesParameters param);
        AesResult GetAesEcbCase(AesParameters param);
        AesResult GetAesOfbCase(AesParameters param);
        AesResult GetAesXtsCase(AesParameters param);

        MctResult<AesResult> GetAesCbcMctCase(AesParameters param);
        MctResult<AesResult> GetAesCfbMctCase(AesParameters param);
        MctResult<AesResult> GetAesEcbMctCase(AesParameters param);
        MctResult<AesResult> GetAesOfbMctCase(AesParameters param);
        #endregion AES

        #region Drbg
        DrbgResult GetDrbgCase();
        #endregion Drbg

        #region DSA
        DsaDomainParametersResult GetDsaDomainParameters();
        VerifyResult<DsaDomainParametersResult> GetDsaDomainParametersVerify();
        DsaKeyResult GetDsaKey();
        VerifyResult<DsaKeyResult> GetDsaKeyVerify();
        DsaSignatureResult GetDsaSignature();
        VerifyResult<DsaSignatureResult> GetDsaVerifyResult();
        #endregion DSA

        #region ECDSA
        // TODO same as DSA
        #endregion ECDSA

        #region Hash
        HashResult GetSha1Case();
        HashResult GetSha2Case();
        HashResult GetSha3Case();
        HashResult GetShakeCase();

        MctResult<HashResult> GetSha1MctCase();
        MctResult<HashResult> GetSha2MctCase();
        MctResult<HashResult> GetSha3MctCase();
        MctResult<HashResult> GetShakeMctCase();

        HashResult GetShakeVotCase();
        #endregion Hash

        #region KAS
        // TODO
        #endregion KAS

        #region KDF
        KdfResult GetKdfCase();
        // All the components individually probably, but those are straight-forward input/output
        #endregion KDF

        #region KeyWrap
        AesResult GetAesKeyWrapCase();
        AesResult GetAesKeyWrapWithPaddingCase();
        TdesResult GetTdesKeyWrapCase();
        #endregion KeyWrap

        #region MAC
        MacResult GetCmacCase();
        MacResult GetHmacCase();
        #endregion MAC

        #region RSA
        RsaKeyResult GetRsaKey();
        VerifyResult<RsaKeyResult> GetRsaKeyVerify();
        RsaSignatureResult GetRsaSignature();
        VerifyResult<RsaSignatureResult> GetRsaVerify();
        #endregion RSA

        #region TDES
        TdesResult GetTdesCbcCase();
        TdesResult GetTdesCfbCase();
        TdesResult GetTdesEcbCase();
        TdesResult GetTdesOfbCase();

        TdesResultWithIvs GetTdesCbcICase();
        TdesResultWithIvs GetTdesOfbICase();

        MctResult<TdesResult> GetTdesCbcMctCase();
        MctResult<TdesResult> GetTdesCfbMctCase();
        MctResult<TdesResult> GetTdesEcbMctCase();
        MctResult<TdesResult> GetTdesOfbMctCase();

        MctResult<TdesResultWithIvs> GetTdesCbcIMctCase();
        MctResult<TdesResultWithIvs> GetTdesOfbIMctCase();
        #endregion TDES
    }
}
