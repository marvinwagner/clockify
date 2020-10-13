using Clockify.Tracking.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Clockify.Tracking.Data.Mappings
{
    public class ConfigurationMapping : IEntityTypeConfiguration<Configuration>
    {
        public void Configure(EntityTypeBuilder<Configuration> builder)
        {
            builder.HasKey(c => c.Id);
        }
    }
}
