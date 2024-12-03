namespace EmployeeManagement.Models
{
    public class Availability
    {
        public virtual long AvailabilityID { get; set; }
        public virtual int EmployeeID { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual string AvailabilityStatus { get; set; }
    }
}
