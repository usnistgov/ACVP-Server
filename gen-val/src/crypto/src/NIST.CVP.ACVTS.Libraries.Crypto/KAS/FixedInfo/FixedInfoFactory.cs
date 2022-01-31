using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.FixedInfo;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.FixedInfo
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
