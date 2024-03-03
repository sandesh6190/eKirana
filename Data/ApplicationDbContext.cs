using eKirana.Models;
using eKirana.Models.SetUp;
using Microsoft.EntityFrameworkCore;

namespace eKirana.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PurchaseDetail>()
            .HasOne(pd => pd.Units)
            .WithMany()
            .HasForeignKey(pd => pd.UnitId)
            .OnDelete(DeleteBehavior.Restrict); // Change behavior here by .Cascade

        modelBuilder.Entity<SaleDetail>()
        .HasOne(sd => sd.Units)
        .WithMany()
        .HasForeignKey(sd => sd.UnitId)
        .OnDelete(DeleteBehavior.Restrict); // Change behavior here by .Cascade
    }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductPurchaseRate> ProductPurchaseRates { get; set; }
    public DbSet<ProductSaleRate> ProductSaleRates { get; set; }
    public DbSet<Unit> Units { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<PurchaseDetail> PurchaseDetails { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SaleDetail> SaleDetails { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<SuperAdmin> SuperAdmins { get; set; }


}
