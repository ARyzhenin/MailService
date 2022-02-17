using TestTaskForMonq.Models;
using Microsoft.EntityFrameworkCore;

namespace TestTaskForMonq
{
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Database that stores information about sending email
        /// </summary>
        public DbSet<Log> Logs => Set<Log>();

        /// <summary>
        /// Database that stores email recipients
        /// </summary>
        public DbSet<Recipient> Recipients => Set<Recipient>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
           // Database.EnsureCreated();
        }

    }
}