using System.Collections.Generic;
using System.Threading.Tasks;
using TestTaskForMonq.Models;
using Microsoft.EntityFrameworkCore;


namespace TestTaskForMonq.Repository
{
    public interface ILogRepository
    {
        public Task<IEnumerable<Log>> GetLogsAsync();

        public Task PostLogAsync(Log log);
    }


    public class LogRepository : ILogRepository
    {
        private readonly ApplicationDbContext _context;

        public LogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Log>> GetLogsAsync()
        {
            var logs = await _context.Logs.Include(u => u.Recipients).ToArrayAsync();
            return logs;
        }

        public async Task PostLogAsync(Log log)
        {
            await _context.Logs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}