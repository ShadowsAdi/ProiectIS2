namespace ProiectIS2.Models.DataTransferObjects.FactsDTO;

public class FactRecord
{
    public int Id { get; set; }
    public required string FactText { get; set; }
    
    public bool SpecialType { get; init; } = false!;
    public required int CategoryId { get; set; } // Returnăm numele categoriei, nu doar ID-ul
}