using System;
using System.Collections.Generic;
using Web.Public.Helpers;

namespace Web.Public.Models
{
	public class PagedResponse<T>
	{
		public long TotalCount { get;set;}
		public bool Incomplete => TotalCount != Data.Count;
		public Links Links { get; set; }
		public List<T> Data { get; set; }

		public PagedResponse(long totalCount, List<T> data, string baseUrl, PagingOptions pagingOptions)
		{
			TotalCount = totalCount;
			Data = data;
			Links = new Links
			{
				FirstPage = $"{baseUrl}?offset=0&limit={Math.Min(pagingOptions.Limit, pagingOptions.Offset)}",          //If the current offset is less than the limit, if the first page brought back the simple limit it would include some duplicate rows
				NextPage = pagingOptions.Offset + pagingOptions.Limit >= totalCount ? null : $"{baseUrl}?offset={pagingOptions.Offset + pagingOptions.Limit}&limit={pagingOptions.Limit}",    //If already at the end, don't have a next page
				PreviousPage = pagingOptions.Offset == 0 ? null : $"{baseUrl}?offset={(pagingOptions.Offset - pagingOptions.Limit < 0 ? 0 : pagingOptions.Offset - pagingOptions.Limit)}&limit={(pagingOptions.Offset - pagingOptions.Limit < 0 ? pagingOptions.Offset : pagingOptions.Limit)}",      //If the previous full page would start with a negative value, need to start at 0 offset and get limit, so ends with the last before this page. If it would be a positive value, then we can get the full limit length
				LastPage = $"{baseUrl}?offset={(totalCount - pagingOptions.Limit < 0 ? 0 : totalCount - pagingOptions.Limit)}&limit={pagingOptions.Limit}"            //Last page starts limit before the end, unless that is negative, then 0
			};
		}

		public PagedResponse(long totalCount, List<T> data, string baseUrl, PagingOptions pagingOptions, string filterQuerystringPortion) : this(totalCount, data, baseUrl, pagingOptions)
		{
			//Append a & and the filter querystring to all the links
			if (!string.IsNullOrEmpty(filterQuerystringPortion))
			{
				Links.FirstPage += $"&{filterQuerystringPortion}";
				Links.NextPage = Links.NextPage == null ? null : $"{Links.NextPage}&{filterQuerystringPortion}";
				Links.PreviousPage = Links.PreviousPage == null ? null : $"{Links.PreviousPage}&{filterQuerystringPortion}";
				Links.LastPage += $"&{filterQuerystringPortion}";
			}
		}
	}

	public class Links
	{
		public string FirstPage { get; set; }
		public string NextPage { get; set; }
		public string PreviousPage { get; set; }
		public string LastPage { get; set; }
	}
}
