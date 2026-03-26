using Microsoft.AspNet.Identity;
using ProcessManagment;
using ProcessManagment.Application.DTO;
using ProcessManagment.Application.interfaces;
using ProcessManagment.Application.Services;
using ProcessManagment.Persistence.Config;
using ProcessManagment.Persistence.Repositories;
using Umbraco.Core.Security;
using Umbraco.Core.Services.Implement;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.Configure<EmailSettings>(
builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<TaskSendEmail>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskSendEmail, TaskSendEmail>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

