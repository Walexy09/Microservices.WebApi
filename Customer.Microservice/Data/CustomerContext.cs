using Microsoft.EntityFrameworkCore;
using Customer.Microservice.Models;

namespace Customer.Microservice.Data
{
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options) { 
        }
        public DbSet<Models.Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Customer>().HasData(
                new Models.Customer { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "1234567890", DateOfBirth = new DateTime(1990, 1, 1), Address = "123 Main St", City = "Anytown", State = "Anystate", ZipCode = "12345" },
                new Models.Customer { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", PhoneNumber = "0987654321", DateOfBirth = new DateTime(1985, 5, 15), Address = "456 Elm St", City = "Othertown", State = "Otherstate", ZipCode = "67890" }
            );
        }
    }
}
