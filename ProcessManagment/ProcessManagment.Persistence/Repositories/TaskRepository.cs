using Microsoft.EntityFrameworkCore;
using ProcessManagment.Application.interfaces;
using ProcessManagment.Domain.Models;
using ProcessManagment.Persistence.DContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagment.Persistence.Repositories
{
    public class TaskRepository: ITaskRepository
    {
        private readonly AppDbContext _context;
        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<BackgroundTasks>> GetTasks(string status)
        {
            var tBackgroundTasks = await _context.BackgroundTasks
                .FromSqlInterpolated($"SELECT * FROM BackgroundTasks WHERE Status = {status} LIMIT 5")
                .ToListAsync();
            return tBackgroundTasks;
        }

        public async Task UpdateStatus(int id, string status)
        {
            int affected = await _context.Database.ExecuteSqlInterpolatedAsync(
            $"UPDATE BackgroundTasks SET Status = {status} WHERE Id = {id}");

        }

    }
}
