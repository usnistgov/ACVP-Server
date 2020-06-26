using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Models
{
	public class CompositeCollectionCapability : BaseCapability
	{
		public IEnumerable<CompositeCapability> ChildCapabilities { get; set; }

		public override string HTML
		{
			get
			{
				string childHTML = "";
				foreach (BaseCapability child in ChildCapabilities)
				{
					childHTML += $"<li>{child.HTML}</li>";
				}

				return (Historical) ? $"<ul class=\"historical list-unstyled\">{childHTML}</ul>" : $"<ul class=\"list-unstyled\">{childHTML}</ul>";
			}
		}

		public CompositeCollectionCapability(RawCapability rawCapability, IEnumerable<RawCapability> rawCapabilities)
		{
			Label = rawCapability.PropertyDisplayName;
			Historical = rawCapability.IsHistorical;
			ChildCapabilities = rawCapabilities.Where(c => c.ParentCapabilityID == rawCapability.ID).OrderBy(c => c.CapabilityOrderIndex).Select(c => new CompositeCapability(c, rawCapabilities));
		}
	}
}
