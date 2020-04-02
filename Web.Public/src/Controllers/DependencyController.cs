using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Public.Exceptions;
using Web.Public.JsonObjects;
using Web.Public.Models;
using Web.Public.Results;
using Web.Public.Services;

namespace Web.Public.Controllers
{
	[Route("acvp/dependencies")]
	[Authorize]
	[TypeFilter(typeof(ExceptionFilter))]
	[ApiController]
	public class DependencyController : ControllerBase
	{
		private readonly IDependencyService _dependencyService;
		private readonly IMessageService _messageService;
		private readonly IJsonReaderService _jsonReader;
		private readonly IJsonWriterService _jsonWriter;
		
		private readonly List<(string Property, bool IsNumeric, List<string> Operators)> _legalPropertyDefinitions = new List<(string Property, bool IsNumeric, List<string> Operators)> { 
			("name", false, new List<string> { "eq", "start", "end", "contains" }),
			("type", false, new List<string> { "eq", "start", "end", "contains" }),
			("description", false, new List<string> { "eq", "start", "end", "contains" })
		};

		public DependencyController(
			IDependencyService dependencyService,
			IMessageService messageService,
			IJsonReaderService jsonReader,
			IJsonWriterService jsonWriter)
		{
			_dependencyService = dependencyService;
			_messageService = messageService;
			_jsonReader = jsonReader;
			_jsonWriter = jsonWriter;
		}

		[HttpPost]
		public JsonHttpStatusResult CreateDependency()
		{
			throw new NotImplementedException();
		}

		[HttpPut("{id}")]
		public JsonHttpStatusResult UpdateDependency(int id)
		{
			throw new NotImplementedException();
		}

		[HttpDelete("{id}")]
		public JsonHttpStatusResult DeleteDependency(int id)
		{
			throw new NotImplementedException();
		}
		
		[HttpGet("{id}")]
		public JsonHttpStatusResult GetDependency(int id)
		{
			throw new NotImplementedException();
		}

		[HttpGet]
		public JsonHttpStatusResult GetFilteredDependencyList()
		{
			//Try to read limit and offset, if passed in
			var limit = 0;
			var offset = 0;
			if (Request.Query.TryGetValue("limit", out var stringLimit))
			{
				int.TryParse(stringLimit.First(), out limit);
			}

			if (Request.Query.TryGetValue("offset", out var stringOffset))
			{
				int.TryParse(stringOffset.First(), out offset);
			}

			//If limit was not present, or a garbage value, make it a default
			if (limit <= 0) limit = 20;

			//Build the querystring that excluded limit and offset
			var filterString = string.Join("&", Request.Query.Where(x => x.Key != "limit" && x.Key != "offset").Select(x => $"{x.Key}={x.Value.FirstOrDefault()}"));

			//Try to build a filter string from those parameters
			var filter = FilterStringService.BuildFilterString(filterString, _legalPropertyDefinitions);

			if (filter.IsValid)
			{
				var pagingOptions = new PagingOptions
				{
					Limit = limit,
					Offset = offset
				};

				var data = _dependencyService.GetFilteredList(filter.FilterString, pagingOptions, filter.OrDelimiter, filter.AndDelimiter);
				var pagedData =  new PagedResponse<Dependency>(data.TotalCount, data.Dependencys, "/acvp/v1/dependencies", pagingOptions, filterString);
				
				return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(pagedData));
			}

			var error = new ErrorObject
			{
				Error = $"Invalid filter applied: {filterString}"
			};
			
			return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(error), HttpStatusCode.RequestedRangeNotSatisfiable);
		}
	}
}