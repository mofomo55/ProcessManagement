using ProcessManagment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagment.Application.interfaces
{
    public interface ITaskSendEmail
    {
        public Task SendEmail(EmailModel email);
    }
}
