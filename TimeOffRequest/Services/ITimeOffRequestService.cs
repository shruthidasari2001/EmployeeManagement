using EmployeeManagement.Models;

namespace EmployeeManagement.Services
{
    public interface ITimeOffRequestService
    {
            void ApplyForTimeOff(TimeOffRequest timeOffRequest);
            List<TimeOffRequest> GetTimeOffRequestsByEmployee(int employeeID);
            void ApproveTimeOffRequest(int requestID, int approverID);
            void RejectTimeOffRequest(int requestID, int approverID);
    }
}


