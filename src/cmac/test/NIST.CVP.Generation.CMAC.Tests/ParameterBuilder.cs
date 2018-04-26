using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math.Domain;
using NIST.CVP.Generation.CMAC.AES;

namespace NIST.CVP.Generation.CMAC_AES.Tests
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private string[] _direction;
        private MathDomain _msgLen;
        private MathDomain _macLen;
        private int[] _keyLen;

        public ParameterBuilder()
        {
            _algorithm = "CMAC";
            _mode = "AES";
            _direction = new[] {"gen", "ver"};
            _msgLen = new MathDomain().AddSegment(new ValueDomainSegment(8));
            _macLen = new MathDomain().AddSegment(new ValueDomainSegment(64));
            _keyLen = new int[] {128, 192, 256};
        }

        public ParameterBuilder WithAlgorithm(string value)
        {
            _algorithm = value;
            return this;
        }

        public ParameterBuilder WithMode(string value)
        {
            _mode = value;
            return this;
        }

        public ParameterBuilder WithDirection(string[] value)
        {
            _direction = value;
            return this;
        }
        public ParameterBuilder WithMsgLen(MathDomain value)
        {
            _msgLen = value;
            return this;
        }

        public ParameterBuilder WithMacLen(MathDomain value)
        {
            _macLen = value;
            return this;
        }

        public ParameterBuilder WithKeyLen(int[] value)
        {
            _keyLen = value;
            return this;
        }
        
        public Parameters Build()
        {
            Parameters p = new Parameters
            {
                Algorithm = _algorithm,
                Mode = _mode,
                Direction = _direction,
                MsgLen = _msgLen,
                MacLen = _macLen,
                KeyLen = _keyLen,

                IsSample = false
            };

            return p;
        }
    }
}
