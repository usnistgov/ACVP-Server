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
        private bool _bitOrientedInput;
        private bool _bitOrientedOutput;
        private bool _bitOrientedKey;
        private bool _includeNull;
        private bool _xof;
        private bool _nonxof;
        private int[] _digestSizes;

        /// <summary>
        /// Provides default parameters that are valid (as of construction)
        /// </summary>
        public ParameterBuilder()
        {
            _algorithm = "KMAC";
            _keyLen = new MathDomain();
            _keyLen = _keyLen.AddSegment(new RangeDomainSegment(null, ParameterValidator._MIN_KEY_LENGTH, ParameterValidator._MAX_KEY_LENGTH, 8));
            _macLen = new MathDomain();
            _macLen = _macLen.AddSegment(new RangeDomainSegment(null, 32, 65536, 8));
            _bitOrientedInput = false;
            _bitOrientedOutput = false;
            _bitOrientedKey = false;
            _includeNull = false;
            _xof = false;
            _nonxof = true;
            _digestSizes = new int[] { 128, 256 };
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

        public ParameterBuilder WithBitOrientedInput(bool value)
        {
            _bitOrientedInput = value;
            return this;
        }

        public ParameterBuilder WithBitOrientedOutput(bool value)
        {
            _bitOrientedOutput = value;
            return this;
        }

        public ParameterBuilder WithBitOrientedKey(bool value)
        {
            _bitOrientedKey = value;
            return this;
        }

        public ParameterBuilder WithIncludeNull(bool value)
        {
            _includeNull = value;
            return this;
        }

        public ParameterBuilder WithXOF(bool value)
        {
            _xof = value;
            return this;
        }

        public ParameterBuilder WithNonXOF(bool value)
        {
            _nonxof = value;
            return this;
        }

        public ParameterBuilder WithDigestSizes(int[] values)
        {
            _digestSizes = GetDeepCopy(values);
            return this;
        }

        public Parameters Build()
        {
            return new Parameters()
            {
                Algorithm = _algorithm,
                KeyLen = _keyLen,
                MacLen = _macLen,
                BitOrientedInput = _bitOrientedInput,
                BitOrientedOutput = _bitOrientedOutput,
                BitOrientedKey = _bitOrientedKey,
                DigestSizes = _digestSizes,
                IncludeNull = _includeNull,
                XOF = _xof,
                NonXOF = _nonxof
            };
        }

        private int[] GetDeepCopy(int[] values)
        {
            var copy = new int[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                copy[i] = values[i];
            }
            return copy;
        }
    }
}
