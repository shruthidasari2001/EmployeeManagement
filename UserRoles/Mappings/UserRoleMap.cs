using FluentNHibernate.Mapping;
using EmployeeManagement.Models;

namespace EmployeeManagement.Mappings
{
    public class UserRoleMap : ClassMap<UserRole>
    {
        public UserRoleMap()
        {
            Table("UserRole");
            Id(x => x.RoleID);
            Map(x => x.RoleName).Not.Nullable();
            HasMany(x => x.Employees)
                     .KeyColumn("RoleID") // This assumes RoleID is a foreign key in the Employee table
                     .Inverse() // Use inverse if the relationship is managed by Employee
                     .Cascade.All(); // Optional: Cascade delete, update, etc.
        }
    }
}
