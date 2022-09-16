namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums
{
    /// <summary>
    /// Key Confirmation Key Attributes - number of ephemeral/static key pairs per type, 
    /// as well as which schemes apply to each type.
    /// 
    /// http://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-56Ar2.pdf
    /// Table 12: Key-agreement schemes
    /// </summary>
    // TODO Check if we can remove this
    public enum KeyAgreementKeyAttribute
    {
        /// <summary>
        /// 2 (E)phemeral keys, 2 (S)tatic keys
        /// 
        /// FCC DH - dhHybrid1
        /// ECC CDH - (Cofactor) Full Unified Model
        /// FFC MQV - MQV2
        /// ECC MQV - Full MQV
        /// </summary>
        E2S2,
        /// <summary>
        /// 2 (E)phemeral keys, 0 (S)tatic keys
        /// 
        /// FFC DH - dhEphem
        /// ECC CDH - (Cofactor) Ephemeral Unified Model
        /// 
        /// Note: Key Confirmation is not possible with this model, as it is reliant on static keys
        /// </summary>
        E2S0,
        /// <summary>
        /// 1 (E)phemeral keys, 2 (S)tatic keys
        /// 
        /// FFC DH - dhHybridOneFlow
        /// ECC CDH - (Cofactor) One-Pass Unified Model
        /// FFC MQV - MQV1
        /// ECC MQV - One-Pass MQV    
        /// 
        /// Note: Only Initiator (U) generates an ephemeral key pair.
        /// </summary>
        E1S2,
        /// <summary>
        /// 1 (E)phemeral keys, 1 (S)tatic keys
        /// 
        /// FFC DH - dhOneFlow
        /// ECC CDH - (Cofactor) One-Pass Diffie-Hellman
        /// 
        /// Note: Key Confirmation is only possible Responder (V) to Initiator (U).
        /// </summary>
        E1S1,
        /// <summary>
        /// 0 (E)phemeral keys, 2 (S)tatic keys
        /// 
        /// FFC DH - dhStatic
        /// ECC CDH - (Cofactor) Static Unified Model
        /// </summary>
        E0S2
    }
}
