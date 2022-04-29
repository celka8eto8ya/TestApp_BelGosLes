using Microsoft.EntityFrameworkCore;
using TestEx_BelGosLes.Models.Entities;

namespace TestEx_BelGosLes.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
                : base(options)
        {
            Database.Migrate();
        }

    }
}
