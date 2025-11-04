using Microsoft.EntityFrameworkCore;
using ProiectIS2.Contexts;
using Microsoft.Extensions.DependencyInjection;
using ProiectIS2.Database.Seeders;
using ProiectIS2.Middleware;

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
        
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

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
        
        app.UseMiddleware<ApiKeyMiddleware>();

        app.MapControllers();
        
        var obj = new DatabaseSeeder();
        _ = obj.DownloadData();

        app.Run();
    }
}