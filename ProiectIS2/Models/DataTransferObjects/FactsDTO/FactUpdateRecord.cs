namespace ProiectIS2.Models.DataTransferObjects.FactsDTO;

public class FactUpdateRecord
{
    public int Id { get; set; }

    public string? FactText { get; set; }
    
    public bool? SpecialType { get; init; }
    
    public int? CategoryId { get; set; }
    
    public DateTime? DeletedAt { get; set; }
}