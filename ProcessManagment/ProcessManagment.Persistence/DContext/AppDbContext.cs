using Microsoft.EntityFrameworkCore;
using ProcessManagment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagment.Persistence.DContext
{
    public class AppDbContext:DbContext
    {
        public DbSet<BackgroundTasks> BackgroundTasks { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}
