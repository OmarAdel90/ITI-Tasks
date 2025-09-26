using Day_3.Models;
using Microsoft.EntityFrameworkCore;

namespace Day_3.Models
{
    public class StudentContext : DbContext
    {
        public StudentContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-3144E5L\\SQLEXPRESS;Database=StudentDB;Trusted_Connection=True;Encrypt=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Age).IsRequired();
                entity.Property(e => e.City).HasMaxLength(100).IsRequired();
            });
        }
    }
}
