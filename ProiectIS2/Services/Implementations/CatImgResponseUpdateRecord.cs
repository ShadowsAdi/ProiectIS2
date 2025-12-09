namespace ProiectIS2.Services.Implementations;

public class CatImgResponseUpdateRecord
{
    public int ResponseCode { get; set; } 
    
    public IFormFile Data { get; set; } = null!;
}