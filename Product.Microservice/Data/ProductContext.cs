using Microsoft.EntityFrameworkCore;

namespace Product.Microservice.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }
        public DbSet<Models.Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Product>().HasData(
                new Models.Product { Id = Guid.NewGuid(), Name = "Solid Shoes", Description = "This is a solid soled shoe", Price = 10.99m, StockQuantity = 100, Category = "Category 1" },
                new Models.Product { Id = Guid.NewGuid(), Name = "Black Bag", Description = "Black Gucci bag for ladies", Price = 19.99m, StockQuantity = 200, Category = "Category 2" }
            );
        }
    }
}
