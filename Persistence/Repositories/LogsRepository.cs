using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class LogsRepository : ILogsRepository
    {
        private readonly ApplicationDbContext _context;
        public LogsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ShiftEntityLog>> GetLogsQueries()
        {
           var logs= await _context.ShiftEntityLog.Where(x=>x.status == "not sent").ToListAsync();
            return logs;
        }

        public async Task <string> UpdateLogsCommand(List<ShiftEntityLog> items)
        {
            foreach (var item in items)
            {
                item.status = "sent";
                await _context.SaveChangesAsync();
            }
            return "Data Updated";
        }
    }
}
