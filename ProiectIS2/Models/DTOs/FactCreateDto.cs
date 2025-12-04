using System.ComponentModel.DataAnnotations;

namespace ProiectIS2.Models.DTOs;

public class FactCreateDto
{
    [Required]
    [StringLength(800, MinimumLength=10)]
    public string Fact { get; set; }

    public bool SpecialType { get; init; }

    [Required]
    public int CategoryId { get; set; } // Clientul trebuie să specifice ID-ul categoriei
}