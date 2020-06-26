using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Models
{
	public class CompositeCapability : BaseCapability
	{
		public IEnumerable<BaseCapability> ChildCapabilities { get; set; }

		public override string HTML
		{
			get
			{
				string childHTML = "";
				foreach (BaseCapability child in ChildCapabilities)
				{
					//If a boolean child that isn't required, and the value is false, then no HTML will be returned, so don't want the list item to be created
					if (!string.IsNullOrEmpty(child.HTML))
					{
						childHTML += $"<li>{child.HTML}</li>";
					}
				}
				return (Historical) ? $"<s>{Label}:</s> <ul class=\"historical unstyled-indented\">{childHTML}</ul>" : $"{Label}: <ul class=\"unstyled-indented\">{childHTML}</ul>";
			}
		}

		public CompositeCapability(RawCapability rawCapability, IEnumerable<RawCapability> rawCapabilities)
		{
			Label = rawCapability.PropertyDisplayName;
			Historical = rawCapability.IsHistorical;
			ChildCapabilities = rawCapabilities.Where(c => c.ParentCapabilityID == rawCapability.ID).OrderBy(c => c.PropertyOrderIndex).ThenBy(c => c.PropertyDisplayName).Select(c => CapabilityFactory.BuildCapability(c, rawCapabilities));
		}
	}
}
