using KERP.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace KERP.Infrastructure.Database
{
    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext(DbContextOptions<ServiceDbContext> options)
        : base(options)
        {
        }

        public DbSet<MassUpdatePurchase> MassUpdatePurchase { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MassUpdatePurchase>(entity =>
            { 
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.PurchaseOrderNumber)
                      .IsRequired()
                      .HasMaxLength(9)
                      .IsFixedLength(); // Dokładna długość 9 znaków

                entity.Property(e => e.LineNumber)
                      .IsRequired();

                entity.Property(e => e.Sequence)
                      .IsRequired();

                entity.Property(e => e.ConfirmedReceiptDate)
                      .IsRequired(false);

                entity.Property(e => e.ChangedReceiptDate)
                      .IsRequired(false);

                entity.Property(e => e.AddedByUserId)
                       .IsRequired();

                entity.Property(e => e.IsGenerated)
                      .IsRequired();

                entity.Property(e => e.GeneratedDate)
                      .IsRequired(false);

                // Konfiguracja tabeli z ograniczeniami
                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_MassUpdatePurchase_LineNumber_Positive", "[LineNumber] > 0");
                    t.HasCheckConstraint("CK_MassUpdatePurchase_Sequence_Positive", "[Sequence] > 0");
                    t.HasCheckConstraint("CK_MassUpdatePurchase_AddedByUserId_Positive", "[AddedByUserId] > 0");
                });


            });

            modelBuilder.Entity<PurchaseOrder>()
                        .HasIndex(p => new { p.PurchaseOrderNumber, p.LineNumber, p.Sequence })
                        .IsUnique();

            modelBuilder.Entity<PurchaseOrder>(entity =>
            { 
                entity.HasKey(e =>e.Id);

                entity.Property(e => e.Id)
                          .ValueGeneratedOnAdd();

                entity.Property(e => e.PurchaseOrderNumber)
                      .IsRequired()
                      .HasMaxLength(9)
                      .IsFixedLength(); // Dokładna długość 9 znaków

                entity.Property(e => e.LineNumber)
                      .IsRequired();

                entity.Property(e => e.Sequence)
                      .IsRequired();

            });
        }
    }
}
