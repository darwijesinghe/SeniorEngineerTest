using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Repository.Data
{
    /// <summary>
    /// DbContext factory class
    /// </summary>
    public class DbContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            // bild config
            var config = new ConfigurationBuilder().SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Web"))
                                                                    .AddJsonFile("appsettings.json")
                                                                    .Build();

            // gets connection string
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseSqlServer(config.GetConnectionString("ConString"));
            return new DataContext(builder.Options);
        }
    }
}
