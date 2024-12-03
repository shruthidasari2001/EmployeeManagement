using System.Collections.Generic;
using EmployeeManagement.Models;
using NHibernate;
using System.Security.Cryptography;
using System.Text;
using ISession = NHibernate.ISession;

namespace EmployeeManagement.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ISession _session;

        public EmployeeService(ISession session)
        {
            _session = session;
        }

        // Get all employees
        public IEnumerable<Employee> GetAllEmployees()
        {
            return _session.Query<Employee>(); // Retrieves all employees from the database
        }

        // Get a single employee by ID
        public object GetEmployeeById(int id)
        {
            var employee = _session.Get<Employee>(id);
            if (employee == null) return null;
          
            // Exclude the password field
            return new
            {
                employee.UserID,
                employee.FirstName,
                employee.LastName,
                employee.Email,
                employee.BirthDate,
                employee.HireDate,
                employee.RoleID
            };
        }

        // Add a new employee
        public void AddEmployee(Employee employee)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Save(employee); // Saves the employee in the database
                transaction.Commit(); // Commits the transaction
            }
        }

        // Update an existing employee
        public void UpdateEmployee(Employee employee)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Update(employee); // Updates the employee in the database
                transaction.Commit(); // Commits the transaction
            }
        }

        // Delete an employee by ID
        public void DeleteEmployee(int id)
        {
            using (var transaction = _session.BeginTransaction())
            {
                var employee = _session.Get<Employee>(id);
                if (employee != null)
                {
                    _session.Delete(employee); // Deletes the employee from the database
                    transaction.Commit(); // Commits the transaction
                }
            }
        }

        // Register a new employee
        public Employee RegisterEmployee(Employee employee)
        {
            var userId = GetNextUserId();
           
            // Check if the email already exists in the database
            var existingEmployee = _session.Query<Employee>().FirstOrDefault(e => e.Email == employee.Email);
            if (existingEmployee != null)
            {
                throw new Exception("An employee with this email already exists.");
            }
            employee.UserID = userId;

            // Hash the password
            employee.Password = HashPassword(employee.Password);

            // Save the employee to the database
            using (var transaction = _session.BeginTransaction())
            {
                _session.Save(employee);
                transaction.Commit();
            }

            return employee; // Return the newly created employee
        }

        // Helper method to hash the password
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes); // Store the hashed password
            }
        }

        private int GetNextUserId()
        {
            // Fetch the next value from the sequence
            var query = "SELECT NEXT VALUE FOR EmployeeUserIDSequence"; // Use your actual sequence name
            var userId = _session.CreateSQLQuery(query).UniqueResult<int>();
            return userId;
        }

        public Employee Login(string email, string password)
        {
            // Find the employee by email
            var employee = _session.Query<Employee>().FirstOrDefault(e => e.Email == email);

            if (employee == null || employee.Password != HashPassword(password))
            {
                return null; // Invalid credentials
            }

            return employee; // Return the employee if credentials match
        }
    }
}
