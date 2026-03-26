using ProcessManagment.Application.interfaces;
using ProcessManagment.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Threading.Tasks;

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

                if (task.TaskType == "Email")
                {
                    var tXml = task.Data;
                    XmlDocument doc = new XmlDocument();
                    var email = new EmailModel();
                    doc.LoadXml(tXml);
                    var tBody = doc.SelectSingleNode("/Email/Body")?.InnerText;
                    var formattedBody = tBody.Replace("\n", "<br>").Replace("\r", "");
                    email.To = doc.SelectSingleNode("/Email/To")?.InnerText;
                    email.Body = formattedBody;
                    email.Subject = doc.SelectSingleNode("/Email/Subject")?.InnerText;
                    await _TaskSendEmail.SendEmail(email);
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
