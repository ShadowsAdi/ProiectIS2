using System.ComponentModel.DataAnnotations;

namespace ProiectIS2.Models;

public class Facts {
    public int Id { get; init; }
    
    [MaxLength(800)]
    public required string? Fact { get; init; }
    
    public bool SpecialType { get; init; }
}