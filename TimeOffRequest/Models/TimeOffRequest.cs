using EmployeeManagement.Models;

public class TimeOffRequest
{
    public virtual int RequestID { get; set; }  // Marked as virtual
    public virtual int EmployeeID { get; set; } // Marked as virtual
    public virtual string RequestType { get; set; } // Marked as virtual
    public virtual DateTime StartDate { get; set; } // Marked as virtual
    public virtual DateTime EndDate { get; set; } // Marked as virtual
    public virtual string? Status { get; set; } // Marked as virtual
    public virtual string Comment { get; set; } // Marked as virtual
    public virtual int? ApproverID { get; set; } // Marked as virtual

}
