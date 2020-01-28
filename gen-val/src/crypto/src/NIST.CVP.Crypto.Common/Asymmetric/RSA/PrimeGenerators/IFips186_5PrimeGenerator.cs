namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators
{
    public interface IFips186_5PrimeGenerator : IPrimeGenerator
    {
        PrimeGeneratorResult GeneratePrimesFips186_5(PrimeGeneratorParameters param);
    }
}