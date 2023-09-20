using DistributedKeyValueStore.Node;

var builder = WebApplication.CreateBuilder(args);

builder.Logging
     .AddConsole();
builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();

var logger = app.Services.GetService<ILoggerFactory>().CreateLogger<Program>();

//Now both are working
logger.LogDebug("Debug World");
logger.LogInformation("Hello World");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
