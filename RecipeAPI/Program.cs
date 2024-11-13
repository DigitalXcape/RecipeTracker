using RecipeAPI.Repositories;
using RecipeAPI.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/recipeAPILog.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRecipeService, RecipeService>();

builder.Services.AddSingleton<IRecipeRepository, SqlRecipeRepository>();
builder.Services.AddScoped<SqlRecipeRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var repository = scope.ServiceProvider.GetRequiredService<SqlRecipeRepository>();
        await repository.SeedDataAsync();
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "An error has occured during seeding database");
    }
}

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
