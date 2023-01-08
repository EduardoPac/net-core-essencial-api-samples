using Dapper.Contrib.Extensions;
using static ApiTasksV2.Data.TaskContext;

namespace ApiTasksV2.Endpoints
{
    public static class TasksEndpoints
    {
        public static void MapTasksEndpoints(this WebApplication app) 
        {
            app.MapGet("/", () => $"Welcome to Tasks API {DateTime.Now}");

            app.MapGet("/tasks", async (GetConnection conectionGetter) =>
            {
                using var conn = await conectionGetter();
                var tasks = conn.GetAll<Task>().ToList();
                if (tasks is null)
                    return Results.NotFound();

                return Results.Ok(tasks);
            });
            
            app.MapGet("/tasks/{id}", async (GetConnection conectionGetter, int id) =>
            {
                using var conn = await conectionGetter();
                var task = conn.Get<Task>(id);

                if (task is null)
                    return Results.NotFound();

                return Results.Ok(task);
            });

            app.MapPost("/tasks", async (GetConnection conectionGetter, Task task) =>
            {
                using var conn = await conectionGetter();
                var id = conn.Insert(task);

                return Results.Created($"/tasks/{id}", task);
            });

            app.MapPut("/tasks", async (GetConnection conectionGetter, Task task) =>
            {
                using var conn = await conectionGetter();
                var id = conn.Update(task);

                return Results.Ok();
            });

            app.MapPut("/tasks/{id}", async (GetConnection conectionGetter, int id) =>
            {
                using var conn = await conectionGetter();

                var deleted = conn.Get<Task>(id);

                if(deleted is null) 
                    return Results.NotFound();  

                conn.Delete(deleted);
                return Results.Ok(deleted);
            });
        }
    }
}
