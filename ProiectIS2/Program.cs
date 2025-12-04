using Microsoft.EntityFrameworkCore;
using ProiectIS2.Contexts;
using Microsoft.Extensions.DependencyInjection;
using ProiectIS2.Database.Seeders;
using ProiectIS2.Middleware;
using dotenv.net;

namespace ProiectIS2;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Adding auth
        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/security?view=aspnetcore-8.0
        builder.Services.AddAuthentication();

        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        DotEnv.Load();
        
        var envVars = DotEnv.Read();

        Console.WriteLine($"DB_HOST: {envVars["DB_HOST"]}");
        Console.WriteLine($"DB_PW: {envVars["DB_PASSWORD"]}");
        Console.WriteLine($"DB_USER: {envVars["DB_USER"]}");
        Console.WriteLine($"DB_DATABASE: {envVars["DB_DATABASE"]}");
        
        string connectionString = $"user={envVars["DB_USER"]};Password={envVars["DB_PASSWORD"]};Server={envVars["DB_HOST"]};Database={envVars["DB_DATABASE"]};";
        
        //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProiectIS2");
                c.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        var useMiddleware = Environment.GetEnvironmentVariable("USE_MIDDLEWARE");
        var seedDatabase = Environment.GetEnvironmentVariable("SEED_DATABASE");

        Console.WriteLine($"USE_MIDDLEWARE: {useMiddleware ?? "not set (default no)"}");
        Console.WriteLine($"SEED_DATABASE: {seedDatabase ?? "not set (default no)"}");

        if (useMiddleware?.ToLowerInvariant() == "yes" || useMiddleware?.ToLowerInvariant() == "true")
        {
            app.UseMiddleware<ApiKeyMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();
            Console.WriteLine("Middleware ENABLED.");
        }
        else
        {
            Console.WriteLine("Middleware SKIPPED.");
        }

        app.MapControllers();

        if (seedDatabase?.ToLowerInvariant() == "yes" || seedDatabase?.ToLowerInvariant() == "true")
        {
            var scope = app.Services.CreateScope();
            var obj = new DatabaseSeeder(scope.ServiceProvider.GetRequiredService<ApplicationDbContext>());
            // trick
            _ = Task.Run(async () => await obj.DownloadData());
            Console.WriteLine("Database Seeding STARTED.");
        }
        else
        {
            Console.WriteLine("Database Seeding SKIPPED.");
        }

        app.Run();
    }
}