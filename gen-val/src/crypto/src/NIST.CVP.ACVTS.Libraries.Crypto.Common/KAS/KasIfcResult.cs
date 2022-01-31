using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS
{
    public class KasIfcResult : ICryptoResult
    {
        public KasIfcResult(IIfcSecretKeyingMaterial keyingMaterialPartyU,
            IIfcSecretKeyingMaterial keyingMaterialPartyV,
            BitString dkm)
        {
            KeyingMaterialPartyU = keyingMaterialPartyU;
            KeyingMaterialPartyV = keyingMaterialPartyV;
            Dkm = dkm;
        }

        /// <summary>
        /// Constructor used to create result for KAS-IFC with key confirmation.
        /// </summary>
        /// <param name="dkm">The derived keying material minus any bits used for the macKey.</param>
        /// <param name="macKey">The macKey that is used for keyConfirmation, taken from most significant bits of the derivedKey.</param>
        /// <param name="macData">The data that is plugged into the message parameter of a mac function.</param>
        /// <param name="tag">The resulting tag of H(macKey, macData).</param>
        public KasIfcResult(
            IIfcSecretKeyingMaterial keyingMaterialPartyU,
            IIfcSecretKeyingMaterial keyingMaterialPartyV,
            BitString dkm, BitString macKey, BitString macData, BitString tag)
        {
            KeyingMaterialPartyU = keyingMaterialPartyU;
            KeyingMaterialPartyV = keyingMaterialPartyV;
            Dkm = dkm;
            MacKey = macKey;
            MacData = macData;
            Tag = tag;
        }

        /// <summary>
        /// Kas error constructor
        /// </summary>
        /// <param name="errorMessage">Error information</param>
        public KasIfcResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// The secret keying material generated/used by party U.
        /// </summary>
        public IIfcSecretKeyingMaterial KeyingMaterialPartyU { get; }
        /// <summary>
        /// The secret keying material generated/used by party V.
        /// </summary>
        public IIfcSecretKeyingMaterial KeyingMaterialPartyV { get; }

        /// <summary>
        /// The result of hashing/macing the derived secret
        /// </summary>
        public BitString Tag { get; }
        /// <summary>
        /// The negotiated key minus any bits that were used for a macKey in keyConfirmation
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
        /// indicates success/failure of Kas operation
        /// </summary>
        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        /// <summary>
        /// The error message returned by the Kas operation
        /// </summary>
        public string ErrorMessage { get; }
    }
}
