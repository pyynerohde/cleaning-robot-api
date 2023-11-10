using System;
using Microsoft.EntityFrameworkCore;

namespace TibberCleaningRobotApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Execution> Executions { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"), npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorCodesToAdd: null);
                });
            }
            base.OnConfiguring(optionsBuilder);
        }

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<Execution>()
        //         .Property(p => p.Id)
        //         .ValueGeneratedOnAdd();
        // }
    }
}
