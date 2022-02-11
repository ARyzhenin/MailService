using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mails.Models;
using Microsoft.EntityFrameworkCore;

namespace Mails.Repository
{
    public interface ILogRepository
    {
        public Task<IEnumerable<Log>> GetLogs();
        
        public Task PostLog(Log log);
    }

    
    public class LogRepository : ILogRepository
    {
        private readonly ApplicationDbContext _context;

        public LogRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Log>> GetLogs()
        {
            var logs = await _context.Logs2.ToListAsync();
            return logs;
        }
        
        public async Task PostLog(Log log)
        {
            _context.Logs2.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}