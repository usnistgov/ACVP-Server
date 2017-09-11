using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.CMAC_AES.Tests
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private string[] _direction;
        private MathDomain _msgLen;
        private MathDomain _macLen;

        public ParameterBuilder()
        {
            _algorithm = "CMAC-AES-128";
            _direction = new[] {"gen", "ver"};
            _msgLen = new MathDomain().AddSegment(new ValueDomainSegment(8));
            _macLen = new MathDomain().AddSegment(new ValueDomainSegment(64));
        }

        public ParameterBuilder WithAlgorithm(string value)
        {
            _algorithm = value;
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
        
        public Parameters Build()
        {
            Parameters p = new Parameters();
            p.Algorithm = _algorithm;
            p.Direction = _direction;
            p.MsgLen = _msgLen;
            p.MacLen = _macLen;

            p.IsSample = false;
            p.Mode = string.Empty;
            
            return p;
        }
    }
}
