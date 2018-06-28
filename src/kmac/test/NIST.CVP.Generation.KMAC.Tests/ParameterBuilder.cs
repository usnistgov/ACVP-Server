using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KMAC.Tests
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private MathDomain _keyLen;
        private MathDomain _macLen;

        /// <summary>
        /// Provides default parameters that are valid (as of construction)
        /// </summary>
        public ParameterBuilder()
        {
            _algorithm = "KMAC";
            _keyLen = new MathDomain().AddSegment(new ValueDomainSegment(8));
            _macLen = new MathDomain().AddSegment(new ValueDomainSegment(80));
        }

        public ParameterBuilder WithAlgorithm(string value)
        {
            _algorithm = value;
            return this;
        }

        public ParameterBuilder WithKeyLen(MathDomain value)
        {
            _keyLen = value;
            return this;
        }

        public ParameterBuilder WithMacLen(MathDomain value)
        {
            _macLen = value;
            return this;
        }

        public Parameters Build()
        {
            return new Parameters()
            {
                Algorithm = _algorithm,
                KeyLen = _keyLen,
                MacLen = _macLen
            };
        }
    }
}
