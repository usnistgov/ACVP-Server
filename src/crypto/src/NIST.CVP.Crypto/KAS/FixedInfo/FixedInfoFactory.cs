using NIST.CVP.Crypto.Common.KAS.FixedInfo;

namespace NIST.CVP.Crypto.KAS.FixedInfo
{
    public class FixedInfoFactory : IFixedInfoFactory
    {
        private IFixedInfoStrategyFactory _fixedInfoStrategyFactory;

        public FixedInfoFactory(IFixedInfoStrategyFactory fixedInfoStrategyFactory)
        {
            _fixedInfoStrategyFactory = fixedInfoStrategyFactory;
        }
        
        public IFixedInfo Get()
        {
            return new FixedInfo(_fixedInfoStrategyFactory);
        }
    }
}