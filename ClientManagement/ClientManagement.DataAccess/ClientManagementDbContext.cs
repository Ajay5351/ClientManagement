using ClientManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClientManagement.Data
{
    public partial class ClientManagementDbContext : IdentityDbContext<ApplicationModel>
    {
        public ClientManagementDbContext()
        {
        }

        public ClientManagementDbContext(
            DbContextOptions<ClientManagementDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Clients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=.;Database=ClientManagementDb;Integrated Security=True;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // IMPORTANT FOR IDENTITY
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.ClientId);

                entity.Property(e => e.ClientName)
                      .HasMaxLength(100);

                entity.Property(e => e.Description)
                      .HasMaxLength(500);

                entity.Property(e => e.LicenceStartDate)
                      .HasColumnType("datetime");

                entity.Property(e => e.LicenceEndDate)
                      .HasColumnType("datetime");

                entity.Property(e => e.CreatedDate)
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdatedDate)
                      .HasColumnType("datetime")
                      .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LicenceKey)
                      .HasDefaultValueSql("(newid())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}