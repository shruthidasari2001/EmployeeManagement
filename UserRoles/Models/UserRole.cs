using EmployeeManagement.Models;

public class UserRole
{
    public virtual int RoleID { get; set; }
    public virtual string RoleName { get; set; } // Make this nullable if it's not required
    public virtual IList<Employee> Employees { get; set; } // Make this nullable if it's not required
}
