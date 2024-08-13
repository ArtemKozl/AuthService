using Ocelot.Middleware;
using OnionTest.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiAuthentication(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.UseOcelot().Wait();


app.Run();
