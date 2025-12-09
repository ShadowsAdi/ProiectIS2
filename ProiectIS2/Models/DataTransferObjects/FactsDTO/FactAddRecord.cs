using System.ComponentModel.DataAnnotations;

namespace ProiectIS2.Models.DataTransferObjects.FactsDTO;

public class FactAddRecord
{
    [Required]
    public required string Fact { get; set; }

    public bool SpecialType { get; init; } = false;

    [Required] 
    public int CategoryId { get; set; } // Clientul trebuie să specifice ID-ul categoriei
}