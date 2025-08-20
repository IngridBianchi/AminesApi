using Microsoft.EntityFrameworkCore;
using AminesApi.Models;

namespace AminesApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Adult> Adults { get; set; }
        public DbSet<Child> Children { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Adult>().ToTable("ADULT", "INGRID");
            modelBuilder.Entity<Adult>().Property(a => a.Id).HasColumnName("ID");
            modelBuilder.Entity<Adult>().Property(a => a.Name).HasColumnName("NAME");
            modelBuilder.Entity<Adult>().Property(a => a.Lastname).HasColumnName("LASTNAME");
            modelBuilder.Entity<Adult>().Property(a => a.BirthYear).HasColumnName("BIRTHYEAR");
            modelBuilder.Entity<Adult>().Property(a => a.ImageURL).HasColumnName("IMAGEURL");

            modelBuilder.Entity<Child>().ToTable("CHILD", "INGRID");
            modelBuilder.Entity<Child>().Property(c => c.Id).HasColumnName("ID");
            modelBuilder.Entity<Child>().Property(c => c.Name).HasColumnName("NAME");
            modelBuilder.Entity<Child>().Property(c => c.Lastname).HasColumnName("LASTNAME");
            modelBuilder.Entity<Child>().Property(c => c.BirthYear).HasColumnName("BIRTHYEAR");
            modelBuilder.Entity<Child>().Property(c => c.ImageURL).HasColumnName("IMAGEURL");
        }
    }
}