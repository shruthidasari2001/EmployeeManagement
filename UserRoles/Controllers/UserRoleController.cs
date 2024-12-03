using EmployeeManagement.Models;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserRole>> GetRoles()
        {
            return Ok(_userRoleService.GetAllRoles());
        }

        [HttpGet("{id}")]
        public ActionResult<UserRole> GetRoleById(int id)
        {
            var role = _userRoleService.GetRoleById(id);
            if (role == null)
                return NotFound();

            return Ok(role);
        }

        [HttpPost]
        public ActionResult AddRole([FromBody] UserRole role)
        {
            _userRoleService.AddRole(role);
            return Ok();
        }

        [HttpPut]
        public ActionResult UpdateRole([FromBody] UserRole role)
        {
            _userRoleService.UpdateRole(role);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteRole(int id)
        {
            _userRoleService.DeleteRole(id);
            return Ok();
        }
    }
}
