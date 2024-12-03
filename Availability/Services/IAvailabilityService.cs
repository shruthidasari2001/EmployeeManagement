using EmployeeManagement.Models;
using System.Collections.Generic;

namespace EmployeeManagement.Services
{
    public interface IAvailabilityService
    {
        void AddAvailability(Availability availability);
        IEnumerable<Availability> GetEmployeeAvailability(int employeeID);
        void DeleteAvailability(int availabilityID);
    }
}
