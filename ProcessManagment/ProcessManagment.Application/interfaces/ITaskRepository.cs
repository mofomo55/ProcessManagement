using ProcessManagment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagment.Application.interfaces
{
    public interface ITaskRepository
    {
        public  Task<List<BackgroundTasks>> GetTasks(string status);
        public Task UpdateStatus(int id, string status);

        public Task ExcuteSQL(string SQL,IEnumerable<object?> parameters);
    }
}
