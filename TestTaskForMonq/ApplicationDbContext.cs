using TestTaskForMonq.Models;
using Microsoft.EntityFrameworkCore;

namespace TestTaskForMonq
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Log> Logs => Set<Log>();
        public DbSet<Recipient> Recipients => Set<Recipient>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
       
    }
}