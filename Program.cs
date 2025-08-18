using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);


var app = builder.Build();

// CRUD  эндпоинты

app.MapPost("/tasks", async (AppDbContext db, TaskItem task) => 
{
    db.Tasks.Add(task);
    await db.SaveChangesAsync();
    return Results.StatusCode(201);
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

app.MapGet("/tasks/{id}", async (AppDbContext db, int id) =>
    await db.Tasks.FindAsync(id) is TaskItem task ? Results.Ok(task) : Results.NotFound()
);

app.MapPut("/tasks/{id}", async (AppDbContext db, int id, TaskItem inputTask) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task is null) return Results.NotFound();

    task.Title = inputTask.Title;
    task.Description = inputTask.Description;
    task.Status = inputTask.Status;
    task.Priority = inputTask.Priority;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/tasks/{id}", async (AppDbContext db, int id) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task is null) return Results.NotFound();

    db.Tasks.Remove(task);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapGet("/", () => "API работает");


app.Run();





