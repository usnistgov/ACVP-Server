using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Web.Public.Models;

namespace Web.Public.Helpers
{
	public class FilterHelpers
	{
		public static (bool IsValid, string QuerystringWithoutPaging, List<OrClause> OrClauses) ParseFilter(string querystring, List<(string Property, bool IsNumeric, List<string> Operators)> legalPropertyDefinitions)
		{
			List<OrClause> orClauses = new List<OrClause>();

			//If there was no querystring, there's nothign to do here
			if (string.IsNullOrEmpty(querystring))
			{
				return (true, querystring, orClauses);
			}

			//Remove the leading ? from the querystring
			querystring = querystring.Substring(1);

			//Remove limit and offset from the querystring, if present
			Regex removeLimitOffset = new Regex(@"(?:limit|offset)=\d+");
			querystring = removeLimitOffset.Replace(querystring, "");

			//This might have left 2 or more & in a row, so turn it into a single
			Regex removeMultipleAmpersand = new Regex(@"&{2,}");
			querystring = removeMultipleAmpersand.Replace(querystring, "&");

			//Also might have wound up starting or ending with a &, so remove those
			if (querystring.StartsWith("&")) querystring = querystring.Substring(1);
			if (querystring.EndsWith("&")) querystring = querystring.Remove(querystring.Length - 1);

			//No filter => good
			if (string.IsNullOrWhiteSpace(querystring)) return (true, querystring, orClauses);

			// Escape "'", causes an error for invalid query when not escaped
			querystring = querystring.Replace("'", "''");

			//Split it on the &, which delimit the "query parameter elements"
			var chunks = querystring.Split("&");

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

			// Now that we've parsed out the chunks, the escaped "'" can be unescaped for use in the creation of the paging links
			querystring = querystring.Replace("''", "'");
			
			//Check if we broke out of the loop - # of chunks and # of parsedChunks would be different
			if (chunks.Length != parsedChunks.Count)
			{
				return (false, querystring, orClauses);
			}

			//Need to combine all the chunks for a given index into OR clauses
			foreach (string index in parsedChunks.Select(x => x.Index).Distinct())
			{
				orClauses.Add(new OrClause { AndClauses = parsedChunks.Where(x => x.Index == index).Select(x => new AndClause { Property = x.Property, Operator = x.Operator, Value = x.Value }).ToList() });
			}

			return (true, querystring, orClauses);
		}

		public static string GenerateOperatorAndValue(string op, string value) => op switch
		{
			"eq" => $"= '{value}'",
			"contains" => $"LIKE '%{value}%'",
			"start" => $"LIKE '{value}%'",
			"end" => $"LIKE '%{value}'",
			"ne" => $"<> '{value}'",
			_ => "",
		};

		public static string GenerateOperatorAndValue(string op, long value) => op switch
		{
			"eq" => $"= {value}",
			"ne" => $"<> {value}",
			"lt" => $"< {value}",
			"le" => $"<= {value}",
			"gt" => $"> {value}",
			"ge" => $">= {value}",
			_ => "",
		};
	}
}
