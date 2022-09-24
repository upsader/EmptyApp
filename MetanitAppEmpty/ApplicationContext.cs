using Microsoft.EntityFrameworkCore;

namespace MetanitAppEmpty
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User() { Id = 1, Name = "Oleg", Age = 27},
                new User() { Id = 2, Name = "Ilja", Age = 2},
                new User() { Id = 3, Name = "Jana", Age = 27}
            );
        }
    }
}
