using System.ComponentModel.DataAnnotations;

namespace ProiectIS2.Models.Domain;

public class Category
{
    [Key]
    public int Id { get; init; }

    [Required]
    [StringLength(100)]
    public required string Name { get; set; } // Ex: "Science", "Funny"

    // Proprietatea de navigare: O categorie are o listă de Facts
    public ICollection<Facts> Facts { get; set; } = new List<Facts>();
}