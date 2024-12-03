using EmployeeManagement.Models;
using FluentNHibernate.Mapping;

namespace EmployeeManagement.Mappings
{
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Table("Employee");  // Map to the Employee table
            Id(x => x.UserID).Column("UserID").GeneratedBy.Assigned() ; 
            Map(x => x.FirstName).Not.Nullable();
            Map(x => x.LastName).Not.Nullable();
            Map(x => x.Email).Not.Nullable().Unique();
            Map(x => x.Password).Not.Nullable();
            Map(x => x.BirthDate).Not.Nullable();
            Map(x => x.HireDate).Not.Nullable();
            Map(x => x.RoleID).Not.Nullable();
        }
    }
}


