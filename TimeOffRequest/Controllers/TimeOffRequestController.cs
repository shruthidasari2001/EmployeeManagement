using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Models;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeOffController : ControllerBase
    {
        private readonly ITimeOffRequestService _torService;

        public TimeOffController(ITimeOffRequestService torService)
        {
            _torService = torService;
        }

        [HttpPost]
        public IActionResult ApplyForTimeOff([FromBody] TimeOffRequest timeOffRequest)
        {
            try
            {
                // Call the service method to apply for time off
                _torService.ApplyForTimeOff(timeOffRequest);

                // Return a success response if no exception occurred
                return Ok(new { Message = "Time-off request submitted successfully." });
            }
            catch (ArgumentException ex)
            {
                // If the employee doesn't exist, return a 404 with the error message
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                // Catch other exceptions and return a 500 status code with error details
                return StatusCode(500, new { Message = "An error occurred while processing your request.", Error = ex.Message });
            }
        }


        [HttpGet("employee/{employeeID}")]
        public IActionResult GetTimeOffRequestsByEmployee(int employeeID)
        {
            var timeOffRequests = _torService.GetTimeOffRequestsByEmployee(employeeID);
            return Ok(timeOffRequests);
        }

        [HttpPut("approve/{requestID}")]
        public IActionResult ApproveTimeOffRequest(int requestID, [FromBody] int approverID)
        {
            _torService.ApproveTimeOffRequest(requestID, approverID);
            return Ok(new { Message = "Time-off request approved." });
        }

        [HttpPut("reject/{requestID}")]
        public IActionResult RejectTimeOffRequest(int requestID, [FromBody] int approverID)
        {
            _torService.RejectTimeOffRequest(requestID, approverID);
            return Ok(new { Message = "Time-off request rejected." });
        }
    }
}
