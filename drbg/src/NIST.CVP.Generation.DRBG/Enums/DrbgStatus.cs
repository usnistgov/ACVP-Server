namespace NIST.CVP.Generation.DRBG.Enums
{
    public enum DrbgStatus
    {
        Success = 0,
        Error = -1,
        RequestedSecurityStrengthTooHigh,
        PredictionResistanceNotSupported,
        PersonalizationStringTooLong,
        ReseedRequired,
        CatastrophicError = -99
    }
}