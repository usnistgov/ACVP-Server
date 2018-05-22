namespace NIST.CVP.Generation.KAS.Enums
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