using Microsoft.EntityFrameworkCore;

namespace TaskFlow.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Task> Tasks => Set<Task>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Task>()
                .Property(t => t.Priority)
                .HasDefaultValue(TaskPriority.Medium);

            modelBuilder.Entity<Task>()
                .Property(t => t.Status)
                .HasDefaultValue(TaskStatus.New);
        }
    }
}
