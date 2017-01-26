using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA1.Tests
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private int[] _msgLen;
        private int[] _digLen;
        private bool _bitOriented;
        private bool _includeNull;

        public ParameterBuilder()
        {
            _algorithm = "SHA1";
            _digLen = new int[] { 160 };
            _msgLen = new int[] { 128 };
            _bitOriented = false;
            _includeNull = false;
        }

        public ParameterBuilder WithAlgorithm(string value)
        {
            _algorithm = value;
            return this;
        }

        public ParameterBuilder WithDigestLen(int[] value)
        {
            _digLen = value;
            return this;
        }

        public ParameterBuilder WithMessageLen(int[] value)
        {
            _msgLen = value;
            return this;
        }

        public ParameterBuilder WithBitOrientation(bool value)
        {
            _bitOriented = value;
            return this;
        }

        public ParameterBuilder WithNullInclusion(bool value)
        {
            _includeNull = value;
            return this;
        }

        public Parameters Build()
        {
            return new Parameters()
            {
                Algorithm = _algorithm,
                MessageLen = _msgLen,
                DigestLen = _digLen,
                BitOriented = _bitOriented,
                IncludeNull = _includeNull
            };
        }
    }
}
