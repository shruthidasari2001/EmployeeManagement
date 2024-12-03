using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Services;
using EmployeeManagement.Models;
using System.Collections.Generic;

namespace EmployeeManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // Get all employees
        [HttpGet]
        public IEnumerable<Employee> GetAll()
        {
            return _employeeService.GetAllEmployees();
        }

        // Get employee by ID
        [HttpGet("{id}")]
    public IActionResult Get(int id)
{
    var employee = _employeeService.GetEmployeeById(id);
    if (employee == null)
    {
        return NotFound(new { message = "Employee not found" });
    }

    return Ok(employee);
}


        // Register a new employee
        [HttpPost("register")]
        public IActionResult Register([FromBody] Employee employee)
        {
            if (employee == null)
                return BadRequest("Employee data is null.");

            // Register the employee (this will also hash the password)
            var registeredEmployee = _employeeService.RegisterEmployee(employee);

            // Return Created status with the new employee data
            return CreatedAtAction(nameof(Get), new { id = registeredEmployee.UserID }, registeredEmployee);
        }

        // Update an existing employee
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Employee employee)
        {
            if (employee == null)
                return BadRequest("Employee data is null.");

            var existingEmployee = _employeeService.GetEmployeeById(id);
            if (existingEmployee == null)
                return NotFound();

            employee.UserID = id;
            _employeeService.UpdateEmployee(employee);

            return NoContent();
        }

        // Delete an employee by ID
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingEmployee = _employeeService.GetEmployeeById(id);
            if (existingEmployee == null)
                return NotFound();

            _employeeService.DeleteEmployee(id);

            return NoContent();
        }

        [HttpPost("login")]
        public IActionResult Login()
        {
            // Retrieve email and password from request body directly
            var email = Request.Form["email"];
            var password = Request.Form["password"];

            // Check if email or password is missing
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Email or password is missing.");
            }

            // Attempt to login
            var employee = _employeeService.Login(email, password);

            if (employee == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            // Set session or cookie to maintain login state
            HttpContext.Session.SetInt32("UserID", employee.UserID);

            return Ok(new { Message = "Login successful", UserID = employee.UserID, FirstName = employee.FirstName, RoleID = employee.RoleID });
        }

        // Logout Endpoint
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Clear session on logout
            HttpContext.Session.Clear();

            return Ok(new { Message = "Logout successful" });
        }
    }
}
