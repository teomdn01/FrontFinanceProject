using FrontFinanceBackend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrontFinanceBackend.Config
{
    public partial class ApplicationDbContext : IdentityDbContext<FrontUser>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<FrontUser> FrontUsers { get; set; } = null!;
        public virtual DbSet<StockBar> StockBars { get; set; } = null!;
        public virtual DbSet<StockData> StockData { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:front-finance-server.database.windows.net,1433;Initial Catalog=Front-Finance;Persist Security Info=False;User ID=front-finance-admin;Password=ZnRHC49a*;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<StockBar>(StockBarConfigure);
        }

        public void StockDataConfigure(EntityTypeBuilder<StockData> builder)
        {
            builder.Property(data => data.Id)
                .ValueGeneratedOnAdd();
        }

        public void StockBarConfigure(EntityTypeBuilder<StockBar> builder)
        {
            builder.Property(bar => bar.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(child => child.StockData)
                .WithMany(parent => parent.Bars)
                .HasForeignKey(child => child.StockId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}