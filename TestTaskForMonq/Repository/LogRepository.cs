using System.Threading.Tasks;
using TestTaskForMonq.Models;
using Microsoft.EntityFrameworkCore;


namespace TestTaskForMonq.Repository
{
    public interface ILogRepository
    {
        public Task<Log[]> GetLogsAsync();

        public Log PostLog(Log log);
    }


    public class LogRepository : ILogRepository
    {
        private readonly ApplicationDbContext _context;

        public LogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Method for getting all logs from database
        /// </summary>
        /// <returns>Collection of Log</returns>
        public async Task<Log[]> GetLogsAsync()
        {
            return await _context.Logs.Include(u => u.Recipients).ToArrayAsync();
        }

        /// <summary>
        /// Method for added information about sending email to database
        /// </summary>
        /// <param name="log">Object that contains information about the sent email, the delivery status and the date of departure</param>
        /// <returns></returns>
        public Log PostLog(Log log)
        {
            _context.Logs.Add(log);
            _context.SaveChanges();
            return log;
        }
    }
}