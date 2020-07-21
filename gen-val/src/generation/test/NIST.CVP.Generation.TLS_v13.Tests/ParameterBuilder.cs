using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.Generation.TLSv13.RFC8446;

namespace NIST.CVP.Generation.TLS_v13.Tests
{
	internal class ParameterBuilder
	{
		private string _algorithm = "TLS-v1.3";
		private string _mode = "KDF";
		private string _revision = "RFC8446";
		private IEnumerable<HashFunctions> _hashFunctions = new[] {HashFunctions.Sha1};

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

		public ParameterBuilder WithRevision(string value)
		{
			_revision = value;
			return this;
		}

		public ParameterBuilder WithHashAlgs(IEnumerable<HashFunctions> value)
		{
			_hashFunctions = value;
			return this;
		}

		public Parameters Build()
		{
			return new Parameters()
			{
				Algorithm = _algorithm,
				Mode = _mode,
				Revision = _revision,
				HmacAlg = _hashFunctions.ToArray()
			};
		}
	}
}