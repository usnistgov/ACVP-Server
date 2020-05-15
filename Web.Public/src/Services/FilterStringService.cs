using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Web.Public.Services
{
	public class FilterStringService
	{
		public static (bool IsValid, string FilterString, string OrDelimiter, string AndDelimiter) BuildFilterString(string userFilter, List<(string Property, bool IsNumeric, List<string> Operators)> legalPropertyDefinitions)
		{
			string orDelimiter = "~";
			string andDelimiter = "||";

			//No filter => good
			if (string.IsNullOrWhiteSpace(userFilter)) return (true, null, null, null);

			//Split it on the &, which delimit the "query parameter elements"
			var chunks = userFilter.Split("&");

			//Generate all the regex patterns that are legal for this filter. Basic pattern is <property>[<index>]=<operation>:<value>
			List<Regex> patterns = new List<Regex>();
			foreach (var legalPropertyDefinition in legalPropertyDefinitions)
			{
				patterns.Add(new Regex(@$"^(?<property>{legalPropertyDefinition.Property})\[(?<index>\d+)]=(?<operator>{string.Join("|", legalPropertyDefinition.Operators)}):(?<value>{(legalPropertyDefinition.IsNumeric ? "\\d+" : ".*")})$"));
			}

			//Collection of the things produced from matching parsed chunks
			List<(string Property, string Index, string Operator, string Value)> parsedChunks = new List<(string Property, string Index, string Operator, string Value)>();

			//Compare each chunk against the regexes, extracting the 4 fields if a match is found
			foreach (string chunk in chunks)
			{
				foreach (Regex pattern in patterns)
				{
					Match match = pattern.Match(chunk);
					if (match.Success)
					{
						parsedChunks.Add((match.Groups["property"].Value, match.Groups["index"].Value, match.Groups["operator"].Value, match.Groups["value"].Value));
						break;
					}
				}
			}

			//Check if we broke out of the loop - # of chunks and # of parsedChunks would be different
			if (chunks.Length != parsedChunks.Count)
			{
				return (false, null, null, null);
			}

			//Need to combine all the chunks for a given index into a single OR line
			List<string> OrLines = new List<string>();

			foreach (string index in parsedChunks.Select(x => x.Index).Distinct())
			{
				OrLines.Add(string.Join(andDelimiter, parsedChunks.Where(x => x.Index == index).Select(x => $"{x.Property}{andDelimiter}{x.Operator}{andDelimiter}{x.Value}")));
			}

			return (true, string.Join(orDelimiter, OrLines), orDelimiter, andDelimiter);
		}

		public static (bool IsValid, string FilterString, string OrDelimiter, string AndDelimiter) BuildFilterString(List<(string PropertyAndIndex, string OperatorAndValue)> filterItems, List<(string Property, bool IsNumeric, List<string> Operators)> legalPropertyDefinitions)
		{
			string orDelimiter = "~";
			string andDelimiter = "||";

			//No filter => good
			if (filterItems.Count == 0) return (true, null, null, null);

			//Generate all the regex patterns that are legal for this filter. Basic pattern is <property>[<index>]=<operation>:<value>
			List<Regex> patterns = new List<Regex>();
			foreach (var legalPropertyDefinition in legalPropertyDefinitions)
			{
				patterns.Add(new Regex(@$"^(?<property>{legalPropertyDefinition.Property})\[(?<index>\d+)]=(?<operator>{string.Join("|", legalPropertyDefinition.Operators)}):(?<value>{(legalPropertyDefinition.IsNumeric ? "\\d+" : ".*")})$"));
			}

			//Collection of the things produced from matching parsed chunks
			List<(string Property, string Index, string Operator, string Value)> parsedChunks = new List<(string Property, string Index, string Operator, string Value)>();

			//Compare each chunk against the regexes, extracting the 4 fields if a match is found
			foreach (string chunk in filterItems.Select(x => $"{x.PropertyAndIndex}={x.OperatorAndValue}"))
			{
				foreach (Regex pattern in patterns)
				{
					Match match = pattern.Match(chunk);
					if (match.Success)
					{
						parsedChunks.Add((match.Groups["property"].Value, match.Groups["index"].Value, match.Groups["operator"].Value, match.Groups["value"].Value));
						break;
					}
				}
			}

			//Check if we broke out of the loop - # of chunks and # of parsedChunks would be different
			if (filterItems.Count != parsedChunks.Count)
			{
				return (false, null, null, null);
			}

			//Need to combine all the chunks for a given index into a single OR line
			List<string> OrLines = new List<string>();

			foreach (string index in parsedChunks.Select(x => x.Index).Distinct())
			{
				OrLines.Add(string.Join(andDelimiter, parsedChunks.Where(x => x.Index == index).Select(x => $"{x.Property}{andDelimiter}{x.Operator}{andDelimiter}{x.Value}")));
			}

			return (true, string.Join(orDelimiter, OrLines), orDelimiter, andDelimiter);
		}
	}
}
