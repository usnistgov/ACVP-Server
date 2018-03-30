namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators
{
    public interface IPrimeGeneratorFactory
    {
        IPrimeGeneratorBase GetPrimeGenerator(string type);
    }
}