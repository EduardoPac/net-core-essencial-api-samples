using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("TasksDb"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region Endpoints Minimal Map

app.MapGet("/", () => "Hello World");

app.MapGet("/tasks", async (AppDbContext db) => await db.Tasks.ToListAsync());

app.MapPost("/tasks", async (Task task, AppDbContext db) =>
{
    db.Tasks.Add(task);
    await db.SaveChangesAsync();
    return Results.Created($"/tasks/{task.Id}", task);
});

app.MapGet("/tasks/{id}", async (int id, AppDbContext db) =>
    await db.Tasks.FindAsync(id) is Task task ? Results.Ok(task) : Results.NotFound());

app.MapGet("/tasks/done", async (int id, AppDbContext db) =>
    await db.Tasks.Where(t => t.IsDone).ToListAsync());

app.MapPut("/tasks/{id}", async (int id, Task inputTask, AppDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);

    if (task is null) 
        return Results.NotFound();

    task.Name = inputTask.Name;
    task.IsDone = inputTask.IsDone;

    await db.SaveChangesAsync();
    return Results.NoContent();
});


app.MapDelete("/tasks/{id}", async (int id, AppDbContext db) =>
{
    if(await db.Tasks.FindAsync(id) is Task task)
    {
        db.Tasks.Remove(task);
        await db.SaveChangesAsync();
        return Results.Ok(task);
    }

    return Results.NotFound();
});

#endregion

app.Run();

class Task
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsDone { get; set; }

}

class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {}

    public DbSet<Task> Tasks => Set<Task>();
}