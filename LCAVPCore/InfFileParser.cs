using LCAVPCore.AlgorithmChunkParsers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LCAVPCore
{
	public class InfFileParser : IInfFileParser
	{
		public IAlgorithmChunkParserFactory _algorithmChunkParserFactory;

		public InfFileParser(IAlgorithmChunkParserFactory algorithmChunkParserFactory)
		{
			_algorithmChunkParserFactory = algorithmChunkParserFactory;
		}


		public InfFile Parse(string filePath)
		{
			//Create the object to output
			InfFile infFile = new InfFile();

			//Pull all the lines of text into a list of strings. Use a try/catch in order to catch any weird things that happen trying to read the file - like it somehow doesn't exist, or is unreadable
			List<string> lines;

			try
			{
				lines = File.ReadAllLines(filePath, Encoding.GetEncoding(1252)).ToList();
			}
			catch
			{
				infFile.Valid = false;
				return infFile;
			}

			//Fail if no lines
			if (lines == null)
			{
				infFile.Valid = false;
				return infFile;
			}

			//Read through all those lines and break it into the vendor/implementation and "algorithm" sections
			var sections = SeparateIntoSections(lines);
			infFile.VendorAndImplementationSection = sections.VendorAndImplementationSection;
			infFile.AlgorithmChunks = sections.AlgorithmChunks;

			//Because there are some algorithms in the new model that come from multiple chunks in the inf file, need to merge those chunks.
			//Commenting this out because it looks like maybe don't need the 186-2 stuff, plus it complicates things...
			//MergeAlgorithmChunks("[ECDSAMerged]", new List<string> { "ECDSA", "ECDSA2" }, "ECDSA2");

			//Parse the selected AlgorithmChunks into usable algorithms aligning with our algorithm model
			foreach (InfFileSection algoChunk in infFile.AlgorithmChunks.Where(c => c.Selected))
			{
				IAlgorithmChunkParser algorithmChunkParser = _algorithmChunkParserFactory.GetParser(algoChunk);
				if (algorithmChunkParser != null)       //May come back null if the file indicated a dead old chunk was selected (like DSA). So just ignore that they selected it.
				{
					infFile.Algorithms.AddRange(algorithmChunkParser.Parse());
				}
			}

			infFile.Valid = true;
			return infFile;
		}

		private (InfFileSection VendorAndImplementationSection, List<InfFileSection> AlgorithmChunks) SeparateIntoSections(List<string> lines)
		{
			InfFileSection vendorAndImplementationSection = null;
			List<InfFileSection> algorithmChunks = new List<InfFileSection>();

			InfFileSection currentSection = null;
			List<string> sectionLines = null;

			foreach (string line in lines)
			{
				if (string.IsNullOrEmpty(line))     //Skip blank lines
				{
					continue;
				}

				//Bracketed text indicates the start of a new section
				if (line.StartsWith("["))
				{
					if (currentSection != null)     //Were already in a section, so need to close it out
					{
						currentSection.Lines = sectionLines;

						if (currentSection.SectionName == "Vendor")     //If in vendor/implementation section, set that property of this object
						{
							vendorAndImplementationSection = currentSection;
						}
						else
						{
							algorithmChunks.Add(currentSection);      //If was an algorithm section, add that section to the algorithms collection
						}
					}

					//Start the new section
					currentSection = new InfFileSection(line);
					sectionLines = new List<string>();
				}
				else
				{
					//Not a section header line, so just add it to the current section - as long as there is one
					if (sectionLines == null)   //Not in a section, so will ignore this line
					{
						continue;
					}

					//Add the line to the current section
					sectionLines.Add(line);
				}
			}

			//Close the last section (will only be null if never had a section tag, which shouldn't happen)
			if (currentSection != null)
			{
				currentSection.Lines = sectionLines;
				algorithmChunks.Add(currentSection);
			}

			return (vendorAndImplementationSection, algorithmChunks);
		}
	}
}
