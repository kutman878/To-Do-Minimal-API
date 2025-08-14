using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// CRUD  эндпоинты

app.MapPost("/tasks", async (AppDbContext db, TaskItem task) => 
{
    db.Tasks.Add(task);
    await db.SaveChangesAsync();
    return Results.Created($"/tasks{task.Id}", task);
});

app.MapGet("/tasks", async (AppDbContext db, string? status, string? priority) => 
{
    var query = db.Tasks.AsQueryable();
    if (!string.IsNullOrEmpty(status))
    {
        query = query.Where(t => t.Status == status);
    }
    if (!string.IsNullOrEmpty(priority))
    {
        query = query.Where(t => t.Priority == priority);
    }
    return await query.ToListAsync();
});




