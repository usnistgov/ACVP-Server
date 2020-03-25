using System.Collections.Generic;
using System.Linq;

namespace LCAVPCore
{
	public class InfFileSection
	{
		private List<string> _lines;

		public List<string> Lines
		{
			get
			{
				return _lines;
			}
			set
			{
				_lines = CleanupNonStandardSelectedLine(value);
			}
		}

		public List<string> FilteredLines
		{
			get { return Lines.Where(l => !l.StartsWith("Selected=")).ToList(); }       //So all the lines minus the Selected line
		}

		public string SectionName { get; private set; }

		public bool Selected
		{
			get
			{
				//Selected value typically comes from the first line within an algorithm section (but allowing for it to be elsewhere in the section, albeit extremely unlikely), expecting true/false after the equals
				var selectedLine = Lines?.FirstOrDefault(l => l.StartsWith("Selected="));
				if (selectedLine == null)
				{
					return false;       //No selected line found, so must not be selected
				}

				//Split the selected line on the =
				string[] setting = selectedLine.Split("=".ToCharArray());
				if (setting[0] != "Selected")
				{
					return false;           //Seemingly impossible situation that the line started with "Selected=", yet somehow after the split on = the first member of the array is not "Selected"
				}

				//Parse what is right of the = to a boolean and return it
				return ParsingHelper.ParseValueToBoolean(setting[1]);
			}
		}

		public List<KeyValuePair<string, string>> KeyValuePairs
		{
			get
			{
				var valuePairs = new List<KeyValuePair<string, string>>();
				foreach (var line in FilteredLines)
				{
					string[] parts = line.Split("=".ToCharArray());

					switch (parts.Length)
					{
						case 1:
							//Normal case, value on right of =, no = in the value
							valuePairs.Add(new KeyValuePair<string, string>(parts[0].Trim(), parts[1].Trim()));
							break;
						case 0:
							//Odd case that there is no value
							valuePairs.Add(new KeyValuePair<string, string>(parts[0].Trim(), "No Value"));
							break;
						default:
							//Odd case where there is a = in the value portion of the line, so this puts them all back together
							valuePairs.Add(new KeyValuePair<string, string>(parts[0].Trim(), string.Join("=", parts.Skip(1)).Trim()));
							break;
					}
				}
				return valuePairs;
			}
		}

		public InfFileSection(string headerLine)
		{
			SectionName = headerLine.Substring(1, headerLine.IndexOf("]") - 1);
			//kludge to deal with odd naming of GCM section.
			if (SectionName == "AES GCM")
			{
				SectionName = "GCM";
			}
			_lines = new List<string>();
		}

		private List<string> CleanupNonStandardSelectedLine(List<string> suppliedLines)
		{
			//See if the line that indicated whether this section is selected starts with "Selected=" like we're expecting, or "Something_Selected=" like sometimes happens when they prefix with the section name
			string selectedLineWithPrefix = suppliedLines.FirstOrDefault(l => l.StartsWith(SectionName + "_Selected="));

			if (selectedLineWithPrefix != null)
			{
				string newSelectedLine = selectedLineWithPrefix.Replace(SectionName + "_", "");     //Clean the extra junk out of the line
				suppliedLines.Remove(selectedLineWithPrefix);                                       //Remove the dirty line from the collection
				suppliedLines.Insert(0, newSelectedLine);                                           //Replace it with the clean line, putting it at the beginning
			}

			return suppliedLines;
		}
	}
}