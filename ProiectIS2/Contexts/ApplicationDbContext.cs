using ProiectIS2.Models.Domain;

namespace ProiectIS2.Contexts;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext 
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }    
    public DbSet<Facts> CatFacts { get; set; }
    
    /*protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        /*
         * https://huggingface.co/ngxson/demo_simple_rag_py/raw/main/cat-facts.txt
        modelBuilder.Entity<Facts>().HasData(
            new Facts { Id = 1, Fact = "On average, cats spend 2/3 of every day sleeping. That means a nine-year-old cat has been awake for only three years of its life.", SpecialType = true},
            new Facts { Id = 2, Fact = "Unlike dogs, cats do not have a sweet tooth. Scientists believe this is due to a mutation in a key taste receptor." },
            new Facts { Id = 3, Fact = "When a cat chases its prey, it keeps its head level. Dogs and humans bob their heads up and down." }
        );
    }*/
    
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