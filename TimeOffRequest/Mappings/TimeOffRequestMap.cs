using FluentNHibernate.Mapping;
using EmployeeManagement.Models;

namespace EmployeeManagement.Mappings
{
    public class TimeOffRequestMap : ClassMap<TimeOffRequest>
    {
        public TimeOffRequestMap()
        {
            Table("TimeOffRequest");
            Id(x => x.RequestID).Column("RequestID").GeneratedBy.Assigned();
            Map(x => x.EmployeeID);
            Map(x => x.RequestType);
            Map(x => x.StartDate);
            Map(x => x.EndDate);
            Map(x => x.Status);
            Map(x => x.Comment);
            Map(x => x.ApproverID);
        }
    }
}
