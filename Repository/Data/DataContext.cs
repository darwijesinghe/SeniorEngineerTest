using Microsoft.EntityFrameworkCore;
using Repository.Domains;
using Repository.Domains.Calculation;

namespace Repository.Data
{
    /// <summary>
    /// Db context class
    /// </summary>
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        // Tables -------------------------------------------
        public DbSet<Products> Products         { get; set; }

        // Not mapped tables --------------------------------------------
        public DbSet<AveragePerCategory> AverageCategory    { get; set; }
        public DbSet<HighestStockCategory> HighestStock     { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Init data seeding list
            var products = new List<Products>()
            {
                new(){ ProductId = 1, Name = "Lenovo Thinkpad T480s", Category = "Electronic", Price = 1500, Stock = 10 }
            };

            // Add seed data
            modelBuilder.Entity<Products>().HasData(products);

            // No key entities only be usable for queries
            modelBuilder.Entity<AveragePerCategory>().HasNoKey();
            modelBuilder.Entity<HighestStockCategory>().HasNoKey();

            base.OnModelCreating(modelBuilder);
        }
    }
}
