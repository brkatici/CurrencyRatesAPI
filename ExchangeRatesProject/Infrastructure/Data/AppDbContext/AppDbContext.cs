using Microsoft.EntityFrameworkCore;
using ExchangeRatesProject.Domain.Models;
namespace ExchangeRatesProject.Infrastructure.Data.AppDbContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {


        }
        public DbSet<Tbl_Log> Logs { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("ExchangeApiDbCon");
            }
        }
    }
}
