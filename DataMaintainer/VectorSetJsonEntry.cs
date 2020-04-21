using System;
using System.Text.Json;
using NIST.CVP.Libraries.Internal.ACVPCore;

namespace DataMaintainer
{
	public class VectorSetJsonEntry
	{
		public long VectorSetID { get; set; }
		public VectorSetJsonFileTypes FileType { get; set; }
		public JsonElement Content { get; set; }
		public DateTime CreatedOn { get; set; }
	}
}
