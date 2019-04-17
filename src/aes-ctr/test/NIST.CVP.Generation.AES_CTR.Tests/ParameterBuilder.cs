using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.AES_CTR.v1_0;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.AES_CTR.Tests
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private string[] _direction;
        private int[] _keyLen;
        private MathDomain _dataLen;
        private bool _overflowCounter;
        private bool _incrementalCounter;

        public ParameterBuilder()
        {
            // Provides a valid (as of construction) set of parameters
            _algorithm = "ACVP-AES";
            _mode = "CTR";
            _direction = ParameterValidator.VALID_DIRECTIONS;
            _keyLen = ParameterValidator.VALID_KEY_SIZES;
            _dataLen = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 1, 128));
            _overflowCounter = true;
            _incrementalCounter = true;
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

        public ParameterBuilder WithKeyLen(int[] value)
        {
            _keyLen = value;
            return this;
        }

        public ParameterBuilder WithDirection(string[] value)
        {
            _direction = value;
            return this;
        }

        public ParameterBuilder WithDataLen(MathDomain value)
        {
            _dataLen = value;
            return this;
        }

        public ParameterBuilder WithOverflowCounter(bool value)
        {
            _overflowCounter = value;
            return this;
        }

        public ParameterBuilder WithIncrementalCounter(bool value)
        {
            _incrementalCounter = value;
            return this;
        }

        public Parameters Build()
        {
            return new Parameters
            {
                Algorithm = _algorithm,
                Mode = _mode,
                Revision = "1.0",
                KeyLen = _keyLen,
                Direction = _direction,
                PayloadLen = _dataLen,
                IncrementalCounter = _incrementalCounter,
                OverflowCounter = _overflowCounter
            };
        }
    }
}
