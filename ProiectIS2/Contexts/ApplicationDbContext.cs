using ProiectIS2.Models.Domain;

namespace ProiectIS2.Contexts;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext 
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }    
    public DbSet<Facts> CatFacts { get; set; }
    
    public DbSet<Category> Categories { get; set; }
    // În ApplicationDbContext.cs
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "General" },
            new Category { Id = 2, Name = "Science" },
            new Category { Id = 3, Name = "Funny" }
        );
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Facts>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    public DbSet<CatImgResponses> CatImgResponses { get; set; }
}