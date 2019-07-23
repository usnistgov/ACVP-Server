using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.NoKC;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Fakes
{
    /// <summary>
    /// Class is used as an alternate factory to inject errors into the macData prior to a mac being run on it.
    /// </summary>
    public class FakeNoKeyConfirmationFactory_BadMacData : INoKeyConfirmationFactory
    {
        private readonly INoKeyConfirmationFactory _noKeyConfirmationFactory;

        public FakeNoKeyConfirmationFactory_BadMacData()
        {
            _noKeyConfirmationFactory = new NoKeyConfirmationFactory(
                new FakeNoKeyConfirmationMacDataCreator(new NoKeyConfirmationMacDataCreator()));
        }

        public INoKeyConfirmation GetInstance(INoKeyConfirmationParameters parameters)
        {
            var noKeyConfirmation = _noKeyConfirmationFactory.GetInstance(parameters);

            return new FakeNoKeyConfirmation_BadMacData(noKeyConfirmation);
        }

        /// <summary>
        /// Fake <see cref="INoKeyConfirmationMacDataCreator"/> (which utilizes a real implementation) to
        /// alter the actual result of the macData prior to its return.
        /// </summary>
        private class FakeNoKeyConfirmationMacDataCreator : INoKeyConfirmationMacDataCreator
        {
            private readonly INoKeyConfirmationMacDataCreator _noKeyConfirmationMacDataCreator;

            public FakeNoKeyConfirmationMacDataCreator(INoKeyConfirmationMacDataCreator noKeyConfirmationMacDataCreator)
            {
                _noKeyConfirmationMacDataCreator = noKeyConfirmationMacDataCreator;
            }
            
            public BitString GetMacData(INoKeyConfirmationParameters param)
            {
                var result = _noKeyConfirmationMacDataCreator.GetMacData(param);
                result[0] += 2;

                return result;
            }
        }
        
        /// <summary>
        /// Wrapper class utilizing a fake implementation of its dependencies to manipulate macData
        /// </summary>
        private class FakeNoKeyConfirmation_BadMacData : INoKeyConfirmation
        {
            private readonly INoKeyConfirmation _noKeyConfirmation;

            public FakeNoKeyConfirmation_BadMacData(INoKeyConfirmation noKeyConfirmation)
            {
                _noKeyConfirmation = noKeyConfirmation;
            }

            public ComputeKeyMacResult ComputeMac()
            {
                return _noKeyConfirmation.ComputeMac();
            }
        }
    }
}