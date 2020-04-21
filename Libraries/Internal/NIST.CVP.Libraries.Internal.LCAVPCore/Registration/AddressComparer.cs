using System;
using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration
{
	public class AddressComparer : EqualityComparer<Address>
	{
		private const char SEPARATOR = (char)31;

		public override bool Equals(Address x, Address y)
		{
			//All these handle making null = empty string for comparison purposes
			return (x.Street1 ?? String.Empty) == (y.Street1 ?? String.Empty)
				&& (x.Street2 ?? String.Empty) == (y.Street2 ?? String.Empty)
				&& (x.Street3 ?? String.Empty) == (y.Street3 ?? String.Empty)
				&& (x.Locality ?? String.Empty) == (y.Locality ?? String.Empty)							
				&& (x.Region ?? String.Empty) == (y.Region ?? String.Empty)
				&& (x.Country ?? String.Empty) == (y.Country ?? String.Empty)
				&& (x.PostalCode ?? String.Empty) == (y.PostalCode ?? String.Empty);
		}

		public override int GetHashCode(Address obj)
		{
			return (obj.Street1 + SEPARATOR + obj.Street2 + SEPARATOR + obj.Street3 + SEPARATOR + obj.Locality + SEPARATOR + obj.Region + SEPARATOR + obj.Country + SEPARATOR + obj.PostalCode).GetHashCode();
		}
	}
}
