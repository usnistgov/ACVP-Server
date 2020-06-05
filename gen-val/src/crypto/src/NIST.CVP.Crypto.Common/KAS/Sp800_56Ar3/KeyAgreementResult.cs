using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3
{
    public class KeyAgreementResult : ICryptoResult
    {
        /// <summary>
        /// The secret keying material provided by party U.
        /// </summary>
        public ISecretKeyingMaterial SecretKeyingMaterialPartyU { get; }
        /// <summary>
        /// The secret keying material provided by party V.
        /// </summary>
        public ISecretKeyingMaterial SecretKeyingMaterialPartyV { get; }
        /// <summary>
        /// The derived secret
        /// </summary>
        public BitString Z { get; }
        /// <summary>
        /// The hash of Z 
        /// </summary>
        public BitString HashZ { get; set; }
        /// <summary>
        /// The fixed info used in construction of <see cref="Dkm"/>
        /// </summary>
        public BitString FixedInfo { get; }
        /// <summary>
        /// The negotiated key H(dkm, macData)
        /// </summary>
        public BitString Dkm { get; }
        /// <summary>
        /// The Key that is plugged into a MAC algorithm (taken from DKM) H(macKey, macData).
        /// </summary>
        public BitString MacKey { get; }
        /// <summary>
        /// The data portion of the mac function H(dkm, macData)
        /// </summary>
        public BitString MacData { get; }
        /// <summary>
        /// The result of hashing/macing the derived secret
        /// </summary>
        public BitString Tag { get; }

        /// <summary>
        /// indicates success/failure of Kas operation
        /// </summary>
        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        /// <summary>
        /// The error message returned by the Kas operation
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// Construct the Result for KAS without a KDF or KeyConfirmation (component test)
        /// </summary>
        /// <param name="secretKeyingMaterialPartyU">The secret keying material for party U.</param>
        /// <param name="secretKeyingMaterialPartyV">The secret keying material for party V.</param>
        /// <param name="z">The shared secret.</param>
        public KeyAgreementResult(ISecretKeyingMaterial secretKeyingMaterialPartyU, 
            ISecretKeyingMaterial secretKeyingMaterialPartyV, 
            BitString z)
        {
            SecretKeyingMaterialPartyU = secretKeyingMaterialPartyU;
            SecretKeyingMaterialPartyV = secretKeyingMaterialPartyV;
            Z = z;
        }
        
        /// <summary>
        /// Construct the Result for KAS without a KDF or KeyConfirmation (component test).
        /// In this instance the hash of Z is performed to accommodate implementations unable
        /// to return Z in the clear 
        /// </summary>
        /// <param name="secretKeyingMaterialPartyU">The secret keying material for party U.</param>
        /// <param name="secretKeyingMaterialPartyV">The secret keying material for party V.</param>
        /// <param name="z">The shared secret.</param>
        /// <param name="hashZ">The hash of the shared secret.</param>
        public KeyAgreementResult(ISecretKeyingMaterial secretKeyingMaterialPartyU, 
            ISecretKeyingMaterial secretKeyingMaterialPartyV, 
            BitString z, BitString hashZ) : this(secretKeyingMaterialPartyU, secretKeyingMaterialPartyV, z)
        {
            HashZ = hashZ;
        }
        
        /// <summary>
        /// Construct the Result for KAS without KeyConfirmation
        /// </summary>
        /// <param name="secretKeyingMaterialPartyU">The secret keying material for party U.</param>
        /// <param name="secretKeyingMaterialPartyV">The secret keying material for party V.</param>
        /// <param name="z">The shared secret.</param>
        /// <param name="fixedInfo">The fixed info used as a KDF input parameter.</param>
        /// <param name="dkm">The derived keying material.</param>
        public KeyAgreementResult(
            ISecretKeyingMaterial secretKeyingMaterialPartyU, 
            ISecretKeyingMaterial secretKeyingMaterialPartyV, 
            BitString z, 
            BitString fixedInfo, 
            BitString dkm) : this(secretKeyingMaterialPartyU, secretKeyingMaterialPartyV, z)
        {
            FixedInfo = fixedInfo;
            Dkm = dkm;
        }

        /// <summary>
        /// Construct the Result for KAS using key confirmation.
        /// </summary>
        /// <param name="secretKeyingMaterialPartyU">The secret keying material for party U.</param>
        /// <param name="secretKeyingMaterialPartyV">The secret keying material for party V.</param>
        /// <param name="z">The shared secret.</param>
        /// <param name="fixedInfo">The fixed info used as a KDF input parameter.</param>
        /// <param name="dkm">The derived keying material.</param>
        /// <param name="macKey">The key used in a MAC function.</param>
        /// <param name="macData">The data used in a MAC function.</param>
        /// <param name="tag">The resulting tag of MAC(macKey, macData).</param>
        public KeyAgreementResult(ISecretKeyingMaterial secretKeyingMaterialPartyU, 
            ISecretKeyingMaterial secretKeyingMaterialPartyV, 
            BitString z, 
            BitString fixedInfo, 
            BitString dkm,
            BitString macKey,
            BitString macData,
            BitString tag) : this(secretKeyingMaterialPartyU, secretKeyingMaterialPartyV, z, fixedInfo, dkm)
        {
            MacKey = macKey;
            MacData = macData;
            Tag = tag;
        }

        public KeyAgreementResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}