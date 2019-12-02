using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Fakes
{
    /// <summary>
    /// Class is used as an alternate factory to inject errors into the macData prior to a mac being run on it.
    /// </summary>
    public class FakeKeyConfirmationFactory_BadMacData : IKeyConfirmationFactory
    {
        private readonly IKeyConfirmationFactory _keyConfirmationFactory;

        public FakeKeyConfirmationFactory_BadMacData()
        {
            _keyConfirmationFactory = new KeyConfirmationFactory(
                new FakeKeyConfirmationMacDataCreator(new KeyConfirmationMacDataCreator()));
        }

        public IKeyConfirmation GetInstance(IKeyConfirmationParameters parameters)
        {
            var keyConfirmation = _keyConfirmationFactory.GetInstance(parameters);
            
            return new FakeKeyConfirmation_BadMacData(keyConfirmation);
        }

        /// <summary>
        /// Fake <see cref="IKeyConfirmationMacDataCreator"/> (which utilizes a real implementation) to
        /// alter the actual result of the macData prior to its return.
        /// </summary>
        private class FakeKeyConfirmationMacDataCreator : IKeyConfirmationMacDataCreator
        {
            private readonly IKeyConfirmationMacDataCreator _keyConfirmationMacDataCreator;

            public FakeKeyConfirmationMacDataCreator(IKeyConfirmationMacDataCreator keyConfirmationMacDataCreator)
            {
                _keyConfirmationMacDataCreator = keyConfirmationMacDataCreator;
            }
            
            public BitString GetMacData(IKeyConfirmationParameters param)
            {
                var result = _keyConfirmationMacDataCreator.GetMacData(param);
                result[0] += 2;

                return result;
            }
        }
        
        /// <summary>
        /// Wrapper class utilizing a fake implementation of its dependencies to manipulate macData
        /// </summary>
        private class FakeKeyConfirmation_BadMacData : IKeyConfirmation
        {
            private readonly IKeyConfirmation _keyConfirmation;

            public FakeKeyConfirmation_BadMacData(IKeyConfirmation keyConfirmation)
            {
                _keyConfirmation = keyConfirmation;
            }

            public ComputeKeyMacResult ComputeMac()
            {
                return _keyConfirmation.ComputeMac();
            }
        }
    }
}