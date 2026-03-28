using ProcessManagment.Application.interfaces;
using ProcessManagment.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace ProcessManagment.Application.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskSendEmail _TaskSendEmail;

        public TaskService(ITaskRepository taskRepository, ITaskSendEmail TaskSendEmail)
        {
            _taskRepository = taskRepository;
            _TaskSendEmail = TaskSendEmail;
        }

        public async Task ProcessTask(BackgroundTasks task)
        {
          try
          {
           var taskId = task.Id;
           await _taskRepository.UpdateStatus(taskId, "Processing");
           var tXml = task.Data;
          
           if (task.TaskType == "Email")
           {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(tXml);
            var email = new EmailModel();
            var tBody = doc.SelectSingleNode("/Email/Body")?.InnerText;
            var formattedBody = tBody.Replace("\n", "<br>").Replace("\r", "");
            email.To = doc.SelectSingleNode("/Email/To")?.InnerText;
            email.Body = formattedBody;
            email.Subject = doc.SelectSingleNode("/Email/Subject")?.InnerText;
            await _TaskSendEmail.SendEmail(email);
           }else if (task.TaskType == "Procdure")
           {
            var xml = XDocument.Parse(task.Data);
            string procedureName = xml.Root.Element("Name")?.Value;
            var parameters = xml.Root
             .Element("Parameters")
             .Elements("Param")
             .Select(p => p.Value)
             .ToList();
            var placeholders = string.Join(", ",parameters.Select((p, i) => $"@p{i}"));
            string sql = $"CALL {procedureName}({placeholders})";
            await _taskRepository.ExcuteSQL(sql, parameters.ToArray());
           }
            await _taskRepository.UpdateStatus(task.Id, "Done");
           }
           catch
           {
            await _taskRepository.UpdateStatus(task.Id, "Failed");
           }
        }

        public async Task<List<BackgroundTasks>> GetPendingTask()
        {
           return await _taskRepository.GetTasks("Pending");
        }

    }
}
