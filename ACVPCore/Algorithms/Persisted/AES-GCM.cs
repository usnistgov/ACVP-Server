﻿using System.Collections.Generic;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.Persisted
{
	public class AES_GCM : PersistedAlgorithmBase
	{
		[AlgorithmProperty(Name = "direction", Type = AlgorithmPropertyType.StringArray)]
		public List<string> Direction { get; set; }

		[AlgorithmProperty(Name = "ivGen", Type = AlgorithmPropertyType.String)]
		public string IVGen { get; set; }

		[AlgorithmProperty(Name = "ivGenMode", Type = AlgorithmPropertyType.String)]
		public string IVGenMode { get; set; }

		[AlgorithmProperty(Name = "key", Type = AlgorithmPropertyType.NumberArray)]
		public List<int> KeyLength { get; set; }

		[AlgorithmProperty(Name = "tagLen", Type = AlgorithmPropertyType.NumberArray)]
		public List<int> TagLength { get; set; }

		[AlgorithmProperty(Name = "ivLen", Type = AlgorithmPropertyType.Domain)]
		public Domain IVLength { get; set; }

		[AlgorithmProperty(Name = "payloadLen", Type = AlgorithmPropertyType.Domain)]
		public Domain PayloadLength { get; set; }

		[AlgorithmProperty(Name = "aadLen", Type = AlgorithmPropertyType.Domain)]
		public Domain AADLength { get; set; }

		public AES_GCM()
		{
			Name = "ACVP-AES-GCM";
			Revision = "1.0";
		}

		public AES_GCM(ACVPCore.Algorithms.External.AES_GCM external) : this()
		{
			Direction = external.Direction;
			IVGen = external.IVGen;
			IVGenMode = external.IVGenMode;
			KeyLength = external.KeyLength;
			TagLength = external.TagLength;
			IVLength = external.IVLength;
			PayloadLength = external.PayloadLength;
			AADLength = external.AADLength;
		}
	}
}