namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.PrimeGenerators
{
    public interface IFips186_4PrimeGenerator : IPrimeGenerator
    {
        PrimeGeneratorResult GeneratePrimesFips186_4(PrimeGeneratorParameters param);
    }
}
