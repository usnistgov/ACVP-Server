using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.KDF_Components.v1_0.ANXIX963;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.ANSIX963.Tests
{
    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private string[] _hashAlg;
        private MathDomain _sharedInfo;
        private MathDomain _keyData;
        private int[] _fieldSize;

        public ParameterBuilder()
        {
            _algorithm = "kdf-components";
            _mode = "ansix9.63";
            _hashAlg = new[] {"sha2-224", "sha2-512"};
            _sharedInfo = new MathDomain().AddSegment(new ValueDomainSegment(0));
            _keyData = new MathDomain().AddSegment(new ValueDomainSegment(128));
            _fieldSize = new[] {224};
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

        public ParameterBuilder WithFieldSize(int[] value)
        {
            _fieldSize = value;
            return this;
        }

        public ParameterBuilder WithHashAlg(string[] value)
        {
            _hashAlg = value;
            return this;
        }

        public ParameterBuilder WithSharedInfoLength(MathDomain value)
        {
            _sharedInfo = value;
            return this;
        }

        public ParameterBuilder WithKeyDataLength(MathDomain value)
        {
            _keyData = value;
            return this;
        }

        public Parameters Build()
        {
            return new Parameters
            {
                Algorithm = _algorithm,
                Mode = _mode,
                HashAlg = _hashAlg,
                SharedInfoLength = _sharedInfo,
                KeyDataLength = _keyData,
                FieldSize = _fieldSize
            };
        }
    }
}
