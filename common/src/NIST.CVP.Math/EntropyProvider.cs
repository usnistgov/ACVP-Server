namespace NIST.CVP.Math
{
    public class EntropyProvider : IEntropyProvider
    {
        private readonly IRandom800_90 _random;

        public EntropyProvider(IRandom800_90 random)
        {
            _random = random;
        }

        public BitString GetEntropy(int numberOfBits)
        {
            return _random.GetRandomBitString(numberOfBits);
        }
    }
}
