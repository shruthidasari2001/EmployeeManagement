namespace EmployeeManagement.Models
{
    public class Employee
    {
        public virtual int UserID { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual DateTime BirthDate { get; set; }
        public virtual DateTime HireDate { get; set; }

        public virtual int RoleID { get; set; }
       
    }

}
