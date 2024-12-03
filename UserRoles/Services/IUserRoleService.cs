using EmployeeManagement.Models;
using System.Collections.Generic;

namespace EmployeeManagement.Services
{
    public interface IUserRoleService
    {
        IList<UserRole> GetAllRoles();
        UserRole GetRoleById(int id);
        void AddRole(UserRole role);
        void UpdateRole(UserRole role);
        void DeleteRole(int id);
    }
}
