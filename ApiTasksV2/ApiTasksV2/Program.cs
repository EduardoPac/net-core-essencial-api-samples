using ApiTasksV2.Endpoints;
using ApiTasksV2.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddPersistence();

var app = builder.Build();

app.MapTasksEndpoints();

app.Run();
