using System.ComponentModel.DataAnnotations;

namespace ProiectIS2.Models;

public class Facts {
    [Key]
    public int Id { get; init; }
    
    [Required]
    [StringLength(800, MinimumLength=10)]
    public required string Fact { get; init; }
    
    public bool SpecialType { get; init; }
    
    // https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/complex-data-model?view=aspnetcore-8.0
    // https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
    public DateTime? UpdatedAt { get; set; }
    
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
    public DateTime? DeletedAt { get; set; }
    
    public bool IsDeleted => DeletedAt != null;
}