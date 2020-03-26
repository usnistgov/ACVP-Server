using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Web.Public.Helpers;
using Web.Public.Models;
using Web.Public.Services;

namespace Web.Public.Controllers
{
	[Route("acvp/vendors")]
	//[Authorize]
	[ApiController]
	public class OrganizationController : ControllerBase
	{
		private readonly IOrganizationService _organizationService;
		private readonly List<(string Property, bool IsNumeric, List<string> Operators)> _legalPropertyDefinitions = new List<(string Property, bool IsNumeric, List<string> Operators)> { 
			("name", false, new List<string> { "eq", "start", "end", "contains" }),
			("website", false, new List<string> { "eq", "start", "end", "contains" }),
			("email", false, new List<string> { "eq", "start", "end", "contains" }),
			("phoneNumber", false, new List<string> { "eq", "start", "end", "contains" })
		};

		public OrganizationController(IOrganizationService organizationService)
		{
			_organizationService = organizationService;
		}

		[HttpGet("{id}")]
		public JsonResult GetVendor(long id)
		{
			var result = _organizationService.Get(id);
			return new JsonResult(result);
		}

		[HttpGet]
		public PagedResponse<Organization> GetFilteredVendorList()
		{
			//Try to read limit and offset, if passed in
			int limit = 0;
			int offset = 0;
			if (Request.Query.TryGetValue("limit", out StringValues stringLimit)) { int.TryParse(stringLimit.First(), out limit); }
			if (Request.Query.TryGetValue("offset", out StringValues stringOffset)) { int.TryParse(stringLimit.First(), out offset); }

			//If limit was not present, or a garbage value, make it a default
			if (limit <= 0) limit = 20;

			//Create a collection of all the query parameters except limit and offset - these would be the filter pieces
			//var filterParameters = Request.Query.Where(x => x.Key != "limit" && x.Key != "offset").Select(x => (x.Key, x.Value.FirstOrDefault())).ToList();

			//Build the querystring that excluded limit and offset
			string filterString = string.Join("&", Request.Query.Where(x => x.Key != "limit" && x.Key != "offset").Select(x => $"{x.Key}={x.Value.FirstOrDefault()}"));

			//Try to build a filter string from those parameters
			//var filter = FilterStringService.BuildFilterString(filterParameters, _legalPropertyDefinitions);
			var filter = FilterStringService.BuildFilterString(filterString, _legalPropertyDefinitions);

			if (filter.IsValid)
			{
				var pagingOptions = new PagingOptions
				{
					Limit = limit,
					Offset = offset
				};

				var data = _organizationService.GetFilteredList(filter.FilterString, pagingOptions, filter.OrDelimiter, filter.AndDelimiter);

				return new PagedResponse<Organization>(data.TotalCount, data.Organizations, "/acvp/v1/vendors", pagingOptions, filterString);
			}

			return null;
		}
	}
}