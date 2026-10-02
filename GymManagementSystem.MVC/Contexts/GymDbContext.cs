using GymManagementSystem.MVC.Configurations;
using GymManagementSystem.MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.MVC.Contexts
{
    public class GymDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=MARIAMETAFY\SQLEXPRESS;Database=GymManagementDb;Trusted_Connection=True;TrustServerCertificate=true");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PlanConfigurations());
        }

        public DbSet<Plan> Plans { get; set; }
    }
}
