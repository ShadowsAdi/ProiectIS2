using System.ComponentModel.DataAnnotations;

namespace ProiectIS2.Models;

public class Facts {
    public int Id { get; set; }
    
    [MaxLength(800)]
    public required string Fact { get; init; }
    
    public bool SpecialType { get; set; }
}