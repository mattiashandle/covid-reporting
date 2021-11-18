using Karolinska.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Karolinska.Infrastructure.Contexts
{
    public class KarolinskaContext : DbContext
    {
        public virtual DbSet<CapacityReport> CapacityReports { get; set; }

        public virtual DbSet<ExpenditureReport> ExpenditureReports { get; set; }

        public virtual DbSet<HealthcareProvider> HealthcareProviders { get; set; }

        public virtual DbSet<OrderReport> OrderReports { get; set; }

        public virtual DbSet<ReceiptReport> ReceiptReports { get; set; }

        public virtual DbSet<StockBalanceReport> StockBalanceReports { get; set; }

        public virtual DbSet<Supplier> Suppliers { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public KarolinskaContext(DbContextOptions options) : base(options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HealthcareProvider>(entity =>
            {
                entity.ToTable("HealthcareProviders", schema: "karda");
                entity.HasKey(e => e.Id);

                entity.HasMany(e => e.CapacityReports)
                    .WithOne()
                    .HasForeignKey(e => e.HealthcareProviderId);

                entity.HasMany(e => e.OrderReports)
                   .WithOne()
                   .HasForeignKey(e => e.HealthcareProviderId);

                entity.HasMany(e => e.StockBalanceReports)
                   .WithOne()
                   .HasForeignKey(e => e.HealthcareProviderId);

                entity.HasMany(e => e.ReceiptReports)
                   .WithOne()
                   .HasForeignKey(e => e.HealthcareProviderId);

            });

            //modelBuilder.Entity<CapacityReport>(entity =>
            //{
            //    entity.ToTable("CapacityReports", schema: "karda");
            //    entity.HasKey(e => e.Id);
            //});
        }
    }
}
