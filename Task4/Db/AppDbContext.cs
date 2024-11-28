using Microsoft.EntityFrameworkCore;

namespace Task4.Db
{
    public class AppDbContext : DbContext
    {
        private IConfiguration _config;

        public AppDbContext(IConfiguration config)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("UserDb"));
        }

        public DbSet<User> Users { get; set; }
        
    }
}
