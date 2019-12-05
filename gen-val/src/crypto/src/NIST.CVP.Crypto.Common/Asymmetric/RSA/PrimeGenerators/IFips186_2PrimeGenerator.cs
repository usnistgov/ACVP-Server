namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators
{
    public interface IFips186_2PrimeGenerator : IPrimeGenerator
    {
        PrimeGeneratorResult GeneratePrimesFips186_2(PrimeGeneratorParameters param);
    }
}