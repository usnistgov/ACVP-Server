using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;
using Web.Public.Exceptions;
using Web.Public.JsonObjects;
using Web.Public.Models;
using Web.Public.Results;
using Web.Public.Services;
using Web.Public.Services.WorkflowItemPayloadValidators;

namespace Web.Public.Controllers
{
	[Route("acvp/v1/persons")]
	[Authorize]
	[TypeFilter(typeof(ExceptionFilter))]
	[ApiController]
	public class PersonController : ControllerBase
	{
		private readonly IPersonService _personService;
		private readonly IMessageService _messageService;
		private readonly IJsonReaderService _jsonReader;
		private readonly IJsonWriterService _jsonWriter;
		private readonly IWorkflowItemValidatorFactory _workflowItemValidatorFactory;
		
		private readonly List<(string Property, bool IsNumeric, List<string> Operators)> _legalPropertyDefinitions = new List<(string Property, bool IsNumeric, List<string> Operators)> { 
			("fullName", false, new List<string> { "eq", "start", "end", "contains" }),
			("email", false, new List<string> { "eq", "start", "end", "contains" }),
			("phoneNumber", false, new List<string> { "eq", "start", "end", "contains" }),
			("vendorId", true, new List<string> { "eq", "ne", "lt", "le", "gt", "ge" })
		};

		public PersonController(
			IPersonService personService,
			IMessageService messageService,
			IJsonReaderService jsonReader,
			IJsonWriterService jsonWriter,
			IWorkflowItemValidatorFactory workflowItemValidatorFactory)
		{
			_personService = personService;
			_messageService = messageService;
			_jsonReader = jsonReader;
			_jsonWriter = jsonWriter;
			_workflowItemValidatorFactory = workflowItemValidatorFactory;
		}

		[HttpPost]
		public JsonHttpStatusResult CreatePerson()
		{
			// Get user cert
			var certRawData = Request.HttpContext.Connection.ClientCertificate.RawData;

			// Get raw JSON
			var jsonBlob = _jsonReader.GetJsonFromBody(Request.Body);
			
			// Convert and validate
			var apiAction = APIAction.CreatePerson;
			var payload = _jsonReader.GetWorkflowItemPayloadFromBodyJson<PersonCreatePayload>(jsonBlob, apiAction);
			var validation = _workflowItemValidatorFactory.GetWorkflowItemPayloadValidator(apiAction).Validate(payload);
			if (!validation.IsSuccess)
			{
				throw new JsonReaderException(validation.Errors);
			}
			
			// Pass to message queue
			var requestID = _messageService.InsertIntoQueue(apiAction, certRawData, payload);
			
			// Build request object for response
			var requestObject = new RequestObject
			{
				RequestID = requestID,
				Status = RequestStatus.Initial
			};
			
			return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(requestObject), HttpStatusCode.Accepted);
		}

		[HttpPut("{id}")]
		public JsonHttpStatusResult UpdatePerson(long id)
		{
			// Get user cert
			var certRawData = Request.HttpContext.Connection.ClientCertificate.RawData;

			// Get raw JSON
			var jsonBlob = _jsonReader.GetJsonFromBody(Request.Body);
			
			// Convert and validate
			var apiAction = APIAction.UpdatePerson;
			var payload = _jsonReader.GetWorkflowItemPayloadFromBodyJson<PersonUpdatePayload>(jsonBlob, apiAction);
			payload.ID = id;
			var validation = _workflowItemValidatorFactory.GetWorkflowItemPayloadValidator(apiAction).Validate(payload);
			if (!validation.IsSuccess)
			{
				throw new JsonReaderException(validation.Errors);
			}

			// Pass to message queue
			var requestID = _messageService.InsertIntoQueue(apiAction, certRawData, payload);
			
			// Build request object for response
			var requestObject = new RequestObject
			{
				RequestID = requestID,
				Status = RequestStatus.Initial
			};
			
			return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(requestObject), HttpStatusCode.Accepted);
		}

		[HttpDelete("{id}")]
		public JsonHttpStatusResult DeletePerson(int id)
		{
			// Get user cert
			var certRawData = Request.HttpContext.Connection.ClientCertificate.RawData;

			var apiAction = APIAction.DeletePerson;
			var payload = new DeletePayload()
			{
				ID = id
			};
			
			var validation = _workflowItemValidatorFactory.GetWorkflowItemPayloadValidator(apiAction).Validate(payload);
			if (!validation.IsSuccess)
			{
				throw new JsonReaderException(validation.Errors);
			}
			
			// Pass to message queue
			var requestID = _messageService.InsertIntoQueue(apiAction, certRawData, payload);
			
			// Build request object for response
			var requestObject = new RequestObject
			{
				RequestID = requestID,
				Status = RequestStatus.Initial
			};
			
			return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(requestObject), HttpStatusCode.Accepted);
		}
		
		
		[HttpGet("{id}")]
		public JsonHttpStatusResult GetPerson(int id)
		{
			var result = _personService.Get(id);
			if (result == null)
			{
				return new JsonHttpStatusResult(
					_jsonWriter.BuildVersionedObject(
						new ErrorObject
						{
							Error = $"Unable to find person with id: {id}"
						}),
					HttpStatusCode.NotFound);
			}
			
			return new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(result));
		}

		[HttpGet]
		public JsonHttpStatusResult GetFilteredPersonList()
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

				var data = _personService.GetFilteredList(filter.FilterString, pagingOptions, filter.OrDelimiter, filter.AndDelimiter);
				var pagedData =  new PagedResponse<Person>(data.TotalCount, data.Persons, "/acvp/v1/persons", pagingOptions, filterString);
				
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