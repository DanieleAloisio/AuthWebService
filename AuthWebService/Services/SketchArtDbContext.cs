using AuthWebService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthWebService.Services
{
    public class SketchArtDbContext : DbContext
    {
        public SketchArtDbContext(DbContextOptions<SketchArtDbContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>() //Un Ruolo
           .HasMany<User>(s => s.Users) //ha molti User
           .WithOne(g => g.Role) //ad un user corrisponde un ruolo
           .HasForeignKey(s => s.IdRole); //la chiave esterna dell'entity
        }
    }
}
