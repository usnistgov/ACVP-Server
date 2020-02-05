using System;
using ACVPCore.ExtensionMethods;
using ACVPCore.Models;
using ACVPWorkflow;
using ACVPWorkflow.Adapters;
using ACVPWorkflow.Models;
using ACVPWorkflow.Results;
using ACVPWorkflow.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkflowsController
    {
        private readonly ILogger<WorkflowsController> _logger;
        private readonly IWorkflowService _workflowService;
        private readonly IWorkflowApproveRejectAdapter _workflowApproveRejectAdapter;

        public WorkflowsController(
            ILogger<WorkflowsController> logger, 
            IWorkflowService workflowService,
            IWorkflowApproveRejectAdapter workflowApproveRejectAdapter)
        {
            _logger = logger;
            _workflowService = workflowService;
            _workflowApproveRejectAdapter = workflowApproveRejectAdapter;
        }
        
        [HttpGet("status/{workflowStatus}")]
        public ActionResult<WrappedEnumerable<WorkflowItemLite>> GetWorkflows(string workflowStatus)
        {
            if(Enum.TryParse<WorkflowStatus>(workflowStatus, true, out var parsedStatus))
            {
                return _workflowService.GetWorkflowItems(parsedStatus).WrapEnumerable();
            }

            _logger.LogWarning($"{nameof(workflowStatus)} ({workflowStatus}) could not be parsed into a type of {nameof(WorkflowStatus)}");
            
            return new BadRequestResult();
        }

        [HttpGet("{workflowId}")]
        public ActionResult<WorkflowItem> GetWorkFlow(long workflowId)
        {
            var result = _workflowService.GetWorkflowItem(workflowId);
            
            if (result == null)
                return new NotFoundResult();

            return result;
        }

        [HttpPost("{workflowId}/approve")]
        public Result ApproveWorkflow(long workflowId)
        {
            return _workflowApproveRejectAdapter.Approve(workflowId);
        }
        
        [HttpPost("{workflowId}/reject")]
        public Result RejectWorkflow(long workflowId)
        {
            return _workflowApproveRejectAdapter.Reject(workflowId);
        }
    }
}