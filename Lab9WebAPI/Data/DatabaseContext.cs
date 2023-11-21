using Lab9Lib;
using Microsoft.EntityFrameworkCore;

namespace Lab9WebAPI.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base()
        {
        }
        public DbSet<Product> Product { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            string str = "server=.;database=DMAWSDB;uid=sa;pwd=123;trustServerCertificate=true";
            optionsBuilder.UseSqlServer(str);
        }
    }
}
