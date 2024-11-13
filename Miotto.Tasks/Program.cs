using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Miotto.Tasks.API.Configurations;
using Miotto.Tasks.Domain.Interfaces;
using Miotto.Tasks.Infra;
using Miotto.Tasks.Infra.Repositories;
using Miotto.Tasks.Service;
using Miotto.Tasks.Service.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMvc();
builder.Services.AddValidatorsFromAssemblyContaining<ProjectValidator>();

#region Microservice DI

builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectTaskService, ProjectTaskService>();
builder.Services.AddScoped<ITaskCommentService, TaskCommentService>();

builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectTaskRepository, ProjectTaskRepository>();
builder.Services.AddScoped<ITaskCommentRepository, TaskCommentRepository>();

#endregion

builder.Services.AddDbContext<TasksContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("TasksContextConnection")));

var app = builder.Build();

#region Database Migrations

using var provider = app.Services.CreateScope();
var context = provider.ServiceProvider.GetRequiredService<TasksContext>();
context.Database.Migrate();

#endregion

// Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseMiddleware<ContextMiddleware>();

app.MapControllers();

app.Run();
