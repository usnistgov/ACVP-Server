namespace NIST.CVP.Crypto.KAS.Scheme
{
    /// <summary>
    /// Describes the retrieving of an instance of <see cref="IScheme"/>
    /// </summary>
    public interface ISchemeFactory
    {
        IScheme GetInstance(SchemeParameters schemeParameters, KdfParameters kdfParameters, MacParameters macParameters);
    }
}