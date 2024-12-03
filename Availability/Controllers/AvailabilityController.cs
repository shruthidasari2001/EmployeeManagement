using EmployeeManagement.Models;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailabilityController : ControllerBase
    {
        private readonly IAvailabilityService _availabilityService;

        public AvailabilityController(IAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }

        [HttpPost]
        public ActionResult AddAvailability([FromBody] Availability availability)
        {
            try
            {
                _availabilityService.AddAvailability(availability);
                return Ok(new { Message = "Availability added successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("employee/{employeeID}")]
        public ActionResult<IEnumerable<Availability>> GetEmployeeAvailability(int employeeID)
        {
            var availabilities = _availabilityService.GetEmployeeAvailability(employeeID);
            return Ok(availabilities);
        }

        [HttpDelete("{availabilityID}")]
        public ActionResult DeleteAvailability(int availabilityID)
        {
            try
            {
                _availabilityService.DeleteAvailability(availabilityID);
                return Ok(new { Message = "Availability deleted successfully." });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }
    }
}
