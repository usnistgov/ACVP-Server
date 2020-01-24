using System;
using System.Collections.Generic;
using System.Text;
using ACVPCore.Models.Capabilities;

namespace ACVPCore.Algorithms.Persisted
{
	public class AES_CBC : AlgorithmBase
	{
		[Property("direction")]
		public StringArrayCapability Direction { get; set; }

		[Property("key")]
		public NumericArrayCapability KeyLength { get; set; }

		public AES_CBC()
		{
			Name = "ACVP-AES-CBC";
		}

		public AES_CBC(ACVPCore.Algorithms.External.AES_CBC external) : this()
		{
			Direction = CreateStringArrayCapability(external.Direction);
			KeyLength = CreateNumericArrayCapability(external.KeyLength);
		}
	}
}
