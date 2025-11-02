using ProiectIS2.Models;

namespace ProiectIS2.Contexts;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext 
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }    
    public DbSet<Facts> CatFacts { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        /*
         * https://huggingface.co/ngxson/demo_simple_rag_py/raw/main/cat-facts.txt
         */
        modelBuilder.Entity<Facts>().HasData(
            new Facts { Id = 1, Fact = "On average, cats spend 2/3 of every day sleeping. That means a nine-year-old cat has been awake for only three years of its life.", SpecialType = true},
            new Facts { Id = 2, Fact = "Unlike dogs, cats do not have a sweet tooth. Scientists believe this is due to a mutation in a key taste receptor." },
            new Facts { Id = 3, Fact = "When a cat chases its prey, it keeps its head level. Dogs and humans bob their heads up and down." }
        );
    }
}