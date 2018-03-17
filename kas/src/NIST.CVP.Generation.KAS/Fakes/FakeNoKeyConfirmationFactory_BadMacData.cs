using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.NoKC;


namespace NIST.CVP.Generation.KAS.Fakes
{
    public class FakeNoKeyConfirmationFactory_BadMacData : INoKeyConfirmationFactory
    {
        private readonly INoKeyConfirmationFactory _noKeyConfirmationFactory;

        public FakeNoKeyConfirmationFactory_BadMacData(INoKeyConfirmationFactory noKeyConfirmationFactory)
        {
            _noKeyConfirmationFactory = noKeyConfirmationFactory;
        }

        public INoKeyConfirmation GetInstance(INoKeyConfirmationParameters parameters)
        {
            var noKeyConfirmation = _noKeyConfirmationFactory.GetInstance(parameters);

            return new FakeNoKeyConfirmation_BadMacData(noKeyConfirmation);
        }

        public class FakeNoKeyConfirmation_BadMacData : INoKeyConfirmation
        {
            private readonly INoKeyConfirmation _noKeyConfirmation;

            public FakeNoKeyConfirmation_BadMacData(INoKeyConfirmation noKeyConfirmation)
            {
                _noKeyConfirmation = noKeyConfirmation;
            }

            public ComputeKeyMacResult ComputeMac()
            {
                var result = _noKeyConfirmation.ComputeMac();

                return new ComputeKeyMacResult(result.MacData,
                    result.Mac.GetMostSignificantBits(result.Mac.BitLength - 2));
            }
        }
    }
}