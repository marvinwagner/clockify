using Clockify.Tracking.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clockify.Tracking.Data.Mappings
{
    public class TimeEntryMapping : IEntityTypeConfiguration<TimeEntry>
    {
        public void Configure(EntityTypeBuilder<TimeEntry> builder)
        {
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.Day).WithMany(p => p.Points).HasForeignKey(t => t.DayId);
        }
    }
}
