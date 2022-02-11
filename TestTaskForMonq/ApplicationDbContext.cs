using Mails.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Mails
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Log> Logs2 => Set<Log>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
             Database.EnsureCreated();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LogMailsMonqDB;Trusted_Connection=True;MultipleActiveResultSets=True");
        //    //Выше строка подлкючения к БД - если сработает файл конфигурации - удалить
        //    //optionsBuilder.LogTo(System.Console.WriteLine, LogLevel.Error);
        //}
    }
}