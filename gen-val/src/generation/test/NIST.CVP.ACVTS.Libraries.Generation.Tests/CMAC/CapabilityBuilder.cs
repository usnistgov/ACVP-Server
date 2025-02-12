﻿using NIST.CVP.ACVTS.Libraries.Generation.CMAC.v1_0;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.CMAC
{
    public class CapabilityBuilder
    {
        private string _direction;
        private MathDomain _msgLen;
        private MathDomain _macLen;
        private int _keyLen;
        private int _keyingOption;

        public CapabilityBuilder()
        {
            _direction = "gen";
            _msgLen = new MathDomain().AddSegment(new ValueDomainSegment(128));
            _macLen = new MathDomain().AddSegment(new ValueDomainSegment(64));
            _keyLen = 128;
            _keyingOption = 0;
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

        public CapabilityBuilder WithKeyLen(int value)
        {
            _keyLen = value;
            return this;
        }

        public CapabilityBuilder WithKeyingOption(int value)
        {
            _keyingOption = value;
            return this;
        }

        public Capability Build()
        {
            return new Capability
            {
                Direction = new[] { _direction },
                KeyLen = new[] { _keyLen },
                KeyingOption = new[] { _keyingOption },
                MacLen = _macLen,
                MsgLen = _msgLen
            };
        }
    }
}
