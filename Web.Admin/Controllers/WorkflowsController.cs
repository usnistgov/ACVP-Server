using System;
using ACVPCore.ExtensionMethods;
using ACVPCore.Models;
using ACVPCore.Results;
using ACVPWorkflow;
using ACVPWorkflow.Models;
using ACVPWorkflow.Models.Parameters;
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

        public WorkflowsController(ILogger<WorkflowsController> logger, IWorkflowService workflowService)
        {
            _logger = logger;
            _workflowService = workflowService;
        }
        
        [HttpGet]
        public ActionResult<WrappedEnumerable<WorkflowItemLite>> GetWorkflows([FromBody] WorkflowListParameters param)
        {
            if (param == null)
                return new BadRequestResult();
            
            return _workflowService.GetWorkflowItems(param);
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
        public ActionResult<Result> ApproveWorkflow(long workflowId)
        {
            var workflow = _workflowService.GetWorkflowItem(workflowId);

            if (workflow == null)
            {
                return new NotFoundResult();
            }
            
            return _workflowService.Approve(workflow);
        }
        
        [HttpPost("{workflowId}/reject")]
        public ActionResult<Result> RejectWorkflow(long workflowId)
        {
            var workflow = _workflowService.GetWorkflowItem(workflowId);

            if (workflow == null)
            {
                return new NotFoundResult();
            }
            
            return _workflowService.Reject(workflow);
        }
    }
}