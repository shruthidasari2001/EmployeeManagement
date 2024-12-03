using FluentNHibernate.Mapping;
using EmployeeManagement.Models;

namespace EmployeeManagement.Mappings
{
    public class AvailabilityMap : ClassMap<Availability>
    {
        public AvailabilityMap()
        {
            Table("Availability");
            Id(x => x.AvailabilityID).Column("AvailabilityID").GeneratedBy.Assigned();
            Map(x => x.EmployeeID).Not.Nullable();
            Map(x => x.StartDate).Not.Nullable();
            Map(x => x.EndDate).Not.Nullable();
            Map(x => x.AvailabilityStatus)
                .Not.Nullable()
                .Check("AvailabilityStatus IN ('Available', 'Unavailable')");
        }
    }
}
