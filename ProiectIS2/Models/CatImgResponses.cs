using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProiectIS2.Models;

public class CatImgResponses
{
    [Key]
    [Required]
    public int ResponseCode { get; set; }
    
    // https://mariadb.com/docs/server/reference/data-types/string-data-types/mediumblob
    // ahrisuficient pentru asta
    [Required]
    [Column(TypeName = "MEDIUMBLOB")]
    public byte[] Data { get; set; } = Array.Empty<byte>();
    
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
    public DateTime UpdatedAt { get; set; }
    
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
    public DateTime DeletedAt { get; set; }
}