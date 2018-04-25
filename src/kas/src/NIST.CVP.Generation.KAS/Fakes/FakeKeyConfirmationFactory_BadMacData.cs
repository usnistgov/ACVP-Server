using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.KC;

namespace NIST.CVP.Generation.KAS.Fakes
{
    public class FakeKeyConfirmationFactory_BadMacData : IKeyConfirmationFactory
    {
        private readonly IKeyConfirmationFactory _keyConfirmationFactory;

        public FakeKeyConfirmationFactory_BadMacData(IKeyConfirmationFactory keyConfirmationFactory)
        {
            _keyConfirmationFactory = keyConfirmationFactory;
        }

        public IKeyConfirmation GetInstance(IKeyConfirmationParameters parameters)
        {
            var keyConfirmation = _keyConfirmationFactory.GetInstance(parameters);
            
            return new FakeKeyConfirmation_BadMacData(keyConfirmation);
        }

        public class FakeKeyConfirmation_BadMacData : IKeyConfirmation
        {
            private readonly IKeyConfirmation _keyConfirmation;

            public FakeKeyConfirmation_BadMacData(IKeyConfirmation keyConfirmation)
            {
                _keyConfirmation = keyConfirmation;
            }

            public ComputeKeyMacResult ComputeMac()
            {
                var result = _keyConfirmation.ComputeMac();

                return new ComputeKeyMacResult(result.MacData,
                    result.Mac.GetMostSignificantBits(result.Mac.BitLength - 2));
            }
        }
    }
}