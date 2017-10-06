namespace NIST.CVP.Generation.KAS.FFC.Enums
{
    public enum TestCaseDispositionOption
    {
        Success,
        SuccessLeadingZeroNibbleZ,
        SuccessLeadingZeroNibbleDkm,
        FailAssuranceIutStaticPrivateKey,
        FailAssuranceIutStaticPublicKey,
        FailAssuranceServerStaticPublicKey,
        FailAssuranceServerEphemeralPublicKey,
        FailChangedZ,
        FailChangedDkm,
        FailChangedOi,
        FailChangedMacData,
        FailChangedTag
    }
}