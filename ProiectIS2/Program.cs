using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ProiectIS2.Contexts;
using Microsoft.Extensions.DependencyInjection;
using ProiectIS2.Database.Seeders;
using ProiectIS2.Middleware;
using dotenv.net;
using Microsoft.AspNetCore.Mvc;
using TGolla.Swashbuckle.AspNetCore.SwaggerGen;

namespace ProiectIS2;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddMvc();
        
        // Adding auth
        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/security?view=aspnetcore-8.0
        builder.Services.AddAuthentication();

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        // Definirea helper-ului pentru ordinea controllerelor (ramane neschimbata)
        SwaggerControllerOrder<ControllerBase> swaggerControllerOrder = new SwaggerControllerOrder<ControllerBase>(Assembly.GetEntryAssembly());

        builder.Services.AddSwaggerGen(c =>
        {
            c.OrderActionsBy(apiDesc =>
            {
                // 1. Controller: Aflam numele si ordinea definita prin atribute
                var controllerName = apiDesc.ActionDescriptor.RouteValues["controller"];
                string controllerSortKey = swaggerControllerOrder.OrderKey(controllerName);

                // 2. Metoda HTTP: Definim prioritatea explicita (GET primul, etc.)
                var method = apiDesc.HttpMethod?.ToUpper() ?? "UNKNOWN";
                int methodPriority = method switch
                {
                    "GET"     => 1,
                    "POST"    => 2,
                    "PUT"     => 3,
                    "PATCH"   => 4,
                    "DELETE"  => 5,
                    "OPTIONS" => 6,
                    "HEAD"    => 7,
                    _         => 99 // Metodele necunoscute la final
                };

                // 3. Calea (RelativePath): Pentru a separa endpoint-urile care au aceeasi metoda (ex: GET /Facts si GET /Facts/{id})
                var path = apiDesc.RelativePath;

                // 4. CONSTRUCTIA CHEII DE SORTARE
                // Folosim metoda ToString("D2") pentru a transforma 1 in "01". 
                // Astfel, sortarea string-urilor va fi corecta numeric ("02" vine inainte de "10").
                // Format final: "0000000001_Facts_01_api/Facts"
                return $"{controllerSortKey}_{controllerName}_{methodPriority.ToString("D2")}_{path}";
            });
        });
        DotEnv.Load();
        
        var envVars = DotEnv.Read();
        
        Console.WriteLine($"DB_HOST: {envVars["DB_HOST"]}");
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
        
        Console.WriteLine("Use Middleware?");
        var command = Console.ReadLine();

        if (command == "yes")
        {
            app.UseMiddleware<ApiKeyMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();
        }
        
        app.MapControllers();
        
        Console.WriteLine("Seed Database?");
        
        command = Console.ReadLine();

        if (command == "yes")
        {
            var scope = app.Services.CreateScope();
            var obj = new DatabaseSeeder(scope.ServiceProvider.GetRequiredService<ApplicationDbContext>());
            // trick
            _ = Task.Run(async () => await obj.DownloadData());
        }

        app.Run();
    }
}