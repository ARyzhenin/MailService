using Mails.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Mails
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Log> Logs2 => Set<Log>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}