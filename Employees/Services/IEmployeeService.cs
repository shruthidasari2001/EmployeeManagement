
// IEmployeeService.cs
using System.Collections.Generic;
using EmployeeManagement.Models;

namespace EmployeeManagement.Services
{
    public interface IEmployeeService
    {
        IEnumerable<Employee> GetAllEmployees();
        object GetEmployeeById(int id);
        void AddEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(int id);
        Employee RegisterEmployee(Employee employee);

        Employee Login(string username, string password);
    }
}
