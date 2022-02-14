using Mails.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Mails
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