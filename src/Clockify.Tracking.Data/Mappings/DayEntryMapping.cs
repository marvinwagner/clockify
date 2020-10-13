using Clockify.Tracking.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clockify.Tracking.Data.Mappings
{
    public class DayEntryMapping : IEntityTypeConfiguration<DayEntry>
    {
        public void Configure(EntityTypeBuilder<DayEntry> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasMany(x => x.Points).WithOne(x => x.Day).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
