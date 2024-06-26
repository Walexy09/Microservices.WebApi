using Microsoft.EntityFrameworkCore;
using Users.Microservice.Models;

namespace Users.Microservice.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }


    }
}
