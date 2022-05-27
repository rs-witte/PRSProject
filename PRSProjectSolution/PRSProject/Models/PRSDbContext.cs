using Microsoft.EntityFrameworkCore;

namespace PRSProject.Models
{
    public class PRSDbContext: DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestLine> RequestLines { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vendor> Vendors { get; set; }

        public PRSDbContext()
        { }   
            public PRSDbContext(DbContextOptions<PRSDbContext>options) : base (options)
        { }
    }
}
