﻿using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
{
	public class KAS_ECC_CDH : PersistedAlgorithmBase
	{
		[AlgorithmProperty("function")]
		public List<string> Functions { get; set; }

		[AlgorithmProperty("curve")]
		public List<string> Curves { get; set; }

		public KAS_ECC_CDH()
		{
			Name = "KAS-ECC";
			Mode = "CDH-Component";
			Revision = "1.0";
		}

		public KAS_ECC_CDH(ACVPCore.Algorithms.External.KAS_ECC_CDH external) : this()
		{
			Functions = external.Functions;
			Curves = external.Curves;
		}
	}
}