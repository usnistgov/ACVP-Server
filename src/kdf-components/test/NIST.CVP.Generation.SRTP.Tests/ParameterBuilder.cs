using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.SRTP.Tests
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private int[] _keyLength;
        private int[] _kdr;
        private bool _zeroKdr;

        public ParameterBuilder()
        {
            _algorithm = "kdf-components";
            _mode = "srtp";
            _keyLength = new[] {128, 192};
            _kdr = new[] {1, 2, 3, 24};
            _zeroKdr = true;
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

        public ParameterBuilder WithKeyLength(int[] value)
        {
            _keyLength = value;
            return this;
        }

        public ParameterBuilder WithKdr(int[] value)
        {
            _kdr = value;
            return this;
        }

        public ParameterBuilder WithZeroKdr(bool value)
        {
            _zeroKdr = value;
            return this;
        }

        public Parameters Build()
        {
            return new Parameters
            {
                Algorithm = _algorithm,
                Mode = _mode,
                AesKeyLength = _keyLength,
                KdrExponent = _kdr,
                SupportsZeroKdr = _zeroKdr
            };
        }
    }
}
