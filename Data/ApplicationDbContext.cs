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
            .HasOne(pd => pd.Unit)
            .WithMany()
            .HasForeignKey(pd => pd.UnitId)
            .OnDelete(DeleteBehavior.Restrict); // Change behavior here by .Cascade

        modelBuilder.Entity<PurchaseDetail>()
    .HasOne(pd => pd.Product)
    .WithMany()
    .HasForeignKey(pd => pd.ProductId)
    .OnDelete(DeleteBehavior.Restrict); // Change behavior here by .Cascade

        modelBuilder.Entity<SaleDetail>()
        .HasOne(sd => sd.Unit)
        .WithMany()
        .HasForeignKey(sd => sd.UnitId)
        .OnDelete(DeleteBehavior.Restrict); // Change behavior here by .Cascade

        modelBuilder.Entity<SaleDetail>()
        .HasOne(sd => sd.Product)
        .WithMany()
        .HasForeignKey(sd => sd.ProductId)
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
    public DbSet<ProductQuantityUnitRate> ProductQuantityUnitRates { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<StockQuantityHistory> StockQuantityHistories { get; set; }




}
