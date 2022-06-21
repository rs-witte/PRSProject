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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=localhost\\sqlexpress;database=prsdb; Integrated Security = true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) //Default values for Request and RequestLine
        {

            modelBuilder.Entity<Request>().Property(t => t.Total)
                    .HasDefaultValue(0.00);
            modelBuilder.Entity<Request>().Property(s => s.Status)
                    .HasDefaultValue("NEW");
            modelBuilder.Entity<Request>().Property(d => d.DeliveryMode)
                    .HasDefaultValue("Pickup");
            modelBuilder.Entity<RequestLine>().Property(q => q.Quantity)
                    .HasDefaultValue(1);
        }
        public PRSDbContext()
        { }   
            public PRSDbContext(DbContextOptions<PRSDbContext>options) : base (options)
        { }
    }
}
