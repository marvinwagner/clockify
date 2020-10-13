
using Clockify.Core.Data;
using Clockify.Core.Messages;
using Clockify.Tracking.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Clockify.Tracking.Data
{
    public class TrackingContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public TrackingContext(DbContextOptions<TrackingContext> options, IMediator mediator)
            : base(options)
        {
            _mediator = mediator;
        }

        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<DayEntry> DayEntries { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }


        public async Task<bool> Commit()
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("CreatedAt") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedAt").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("CreatedAt").IsModified = false;
                }
            }

            var sucesso = await base.SaveChangesAsync() > 0;
            if (sucesso) await _mediator.PublishEvents(this);

            return sucesso;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)"); // caso não declare, o padrão é varchar para evitar que se crie colunas nvarchar

            modelBuilder.Ignore<Event>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrackingContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            base.OnModelCreating(modelBuilder);
        }
    }
}
