using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mails.Models;
using Microsoft.EntityFrameworkCore;

namespace Mails.Repository
{
    public interface ILogRepository
    {
        public IEnumerable<Log> GetLogs();

        public void PostLog(Log log);
    }


    public class LogRepository : ILogRepository
    {
        private readonly ApplicationDbContext _context;

        public LogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Log> GetLogs()
        {
            var logs = _context.Logs2.ToList();
            return logs;
        }

        public void PostLog(Log log)
        {
            _context.Logs2.Add(log);
            _context.SaveChanges();
        }
    }
}