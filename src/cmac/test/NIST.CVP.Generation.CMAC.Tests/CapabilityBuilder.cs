using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.CMAC.Tests
{
    public class CapabilityBuilder
    {
        private string _mode;
        private string _direction;
        private MathDomain _msgLen;
        private MathDomain _macLen;
        private int _keyingOption;

        public CapabilityBuilder()
        {
            _mode = "AES-128";
            _direction = "gen";
            _msgLen = new MathDomain().AddSegment(new ValueDomainSegment(128));
            _macLen = new MathDomain().AddSegment(new ValueDomainSegment(128));
            _keyingOption = 0;
        }

        public CapabilityBuilder WithMode(string value)
        {
            _mode = value;
            return this;
        }

        public CapabilityBuilder WithDirection(string value)
        {
            _direction = value;
            return this;
        }

        public CapabilityBuilder WithMsgLen(MathDomain value)
        {
            _msgLen = value;
            return this;
        }

        public CapabilityBuilder WithMacLen(MathDomain value)
        {
            _macLen = value;
            return this;
        }
        
        public CapabilityBuilder WithKeyingOption(int value)
        {
            _keyingOption = value;
            return this;
        }

        public Capability Build()
        {
            return new Capability()
            {
                Direction = _direction,
                KeyingOption = _keyingOption,
                MacLen = _macLen,
                Mode = _mode,
                MsgLen = _msgLen
            };
        }
    }
}