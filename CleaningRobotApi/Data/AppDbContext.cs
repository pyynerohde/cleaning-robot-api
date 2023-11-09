using Microsoft.EntityFrameworkCore;

namespace tibber_robot_api.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Execution> Executions { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Execution>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"))
                              .UseSnakeCaseNamingConvention();
            }
        }
    }
}
