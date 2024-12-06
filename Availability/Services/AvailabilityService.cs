using EmployeeManagement.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using ISession = NHibernate.ISession;

namespace EmployeeManagement.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly ISession _session;

        public AvailabilityService(ISession session)
        {
            _session = session;
        }

        public void AddAvailability(Availability availability)
        {
            // Validate: Employee existence
            var employee = _session.Get<Employee>(availability.EmployeeID);
            if (employee == null)
            {
                throw new ArgumentException("The employee with the provided ID does not exist.");
            }

            // Validate: Start date is before End date
            if (availability.StartDate >= availability.EndDate)
            {
                throw new ArgumentException("StartDate must be before EndDate.");
            }

            // Validate: Overlapping dates for the same employee
            var overlappingAvailability = _session.Query<Availability>()
                .Where(a => a.EmployeeID == availability.EmployeeID &&
                            ((availability.StartDate >= a.StartDate && availability.StartDate <= a.EndDate) ||
                             (availability.EndDate >= a.StartDate && availability.EndDate <= a.EndDate) ||
                             (availability.StartDate <= a.StartDate && availability.EndDate >= a.EndDate)))
                .ToList();

            if (overlappingAvailability.Any())
            {
                throw new ArgumentException("The specified availability dates overlap with an existing record.");
            }
            var availabilityId = GetNextAvailabilityId();

            // Set the RequestID to the newly generated TOR ID
            availability.AvailabilityID = availabilityId;

            // Save the new availability
            using (var transaction = _session.BeginTransaction())
            {
                _session.Save(availability);
                transaction.Commit();
            }
        }

        public IEnumerable<Availability> GetEmployeeAvailability(int employeeID)
        {
            return _session.Query<Availability>().Where(a => a.EmployeeID == employeeID).ToList();
        }

        public void DeleteAvailability(long availabilityID)
        {
            var availability = _session.Get<Availability>(availabilityID);
            if (availability == null)
            {
                throw new ArgumentException("The availability record does not exist.");
            }

            using (var transaction = _session.BeginTransaction())
            {
                _session.Delete(availability);
                transaction.Commit();
            }
        }

        private long GetNextAvailabilityId()
        {
            // Fetch the next value from the sequence
            var query = "SELECT NEXT VALUE FOR AvailabilityIdSequence"; // Use your actual sequence name
            var userId = _session.CreateSQLQuery(query).UniqueResult<long>();
            return userId;
        }
    }
}
