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
            string connStr = Environment.GetEnvironmentVariable("TASK_4_CONN_STRING", EnvironmentVariableTarget.Process)
                ?? Environment.GetEnvironmentVariable("TASK_4_CONN_STRING", EnvironmentVariableTarget.Machine)
                ?? throw new InvalidOperationException("Connection string not found in environment variables.");

            optionsBuilder.UseSqlServer(connStr);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

        public DbSet<User> Users { get; set; }
        
    }
}
