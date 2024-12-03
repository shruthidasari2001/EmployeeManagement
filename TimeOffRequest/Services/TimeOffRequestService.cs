using NHibernate;
using EmployeeManagement.Models;
using ISession = NHibernate.ISession;

namespace EmployeeManagement.Services
{
    public class TimeOffRequestService : ITimeOffRequestService
    {
        private readonly ISession _session;

        public TimeOffRequestService(ISession session)
        {
            _session = session;
        }
public void ApplyForTimeOff(TimeOffRequest timeOffRequest)
{
    // Ensure the employee exists by querying the database
    var employee = _session.Get<Employee>(timeOffRequest.EmployeeID);
    if (employee == null)
    {
        throw new ArgumentException("The employee with the provided ID does not exist.");
    }

    // Ensure the leave request is valid
    if (timeOffRequest.EmployeeID <= 0)
        throw new ArgumentException("Invalid Employee ID.");

    if (string.IsNullOrEmpty(timeOffRequest.RequestType))
        throw new ArgumentException("Leave type must be specified.");

    if (timeOffRequest.StartDate >= timeOffRequest.EndDate)
        throw new ArgumentException("End date must be later than the start date.");

    // Set the default status to 'Pending'
    timeOffRequest.Status = "Pending";

    // Set ApproverID to null since the employee cannot approve their own leave
    timeOffRequest.ApproverID = null;

    // Generate the next available TOR ID (could be from sequence or other mechanism)
    var torId = GetNextTORId();

    // Set the RequestID to the newly generated TOR ID
    timeOffRequest.RequestID = torId;

    // Save the leave request to the database
    using (var transaction = _session.BeginTransaction())
    {
        _session.Save(timeOffRequest);
        transaction.Commit();
    }
}

        public List<TimeOffRequest> GetTimeOffRequestsByEmployee(int employeeID)
        {
            return _session.Query<TimeOffRequest>()
                .Where(r => r.EmployeeID == employeeID)
                .ToList();
        }

        public void ApproveTimeOffRequest(int requestID, int approverID)
        {
            var timeOffRequest = _session.Get<TimeOffRequest>(requestID);
            if (timeOffRequest != null)
            {
                timeOffRequest.Status = "Approved";
                timeOffRequest.ApproverID = approverID;

                using (var transaction = _session.BeginTransaction())
                {
                    _session.Update(timeOffRequest);
                    transaction.Commit();
                }
            }
        }

        public void RejectTimeOffRequest(int requestID, int approverID)
        {
            var timeOffRequest = _session.Get<TimeOffRequest>(requestID);
            if (timeOffRequest != null)
            {
                timeOffRequest.Status = "Denied";
                timeOffRequest.ApproverID = approverID;

                using (var transaction = _session.BeginTransaction())
                {
                    _session.Update(timeOffRequest);
                    transaction.Commit();
                }
            }
        }

        private int GetNextTORId()
        {
            // Fetch the next value from the sequence
            var query = "SELECT NEXT VALUE FOR TimeOffRquestIdSequence"; // Use your actual sequence name
            var userId = _session.CreateSQLQuery(query).UniqueResult<int>();
            return userId;
        }
    }
}
