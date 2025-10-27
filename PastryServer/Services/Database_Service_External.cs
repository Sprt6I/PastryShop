using Microsoft.EntityFrameworkCore;
using PastryServer.Models;

namespace PastryServer.Services
{
    public class Database_Service_External : DbContext
    {
        public Database_Service_External(DbContextOptions<Database_Service_External> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Verification_Code> VerificationCodes { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Product_Category> product_Categories { get; set; }
    }
}
