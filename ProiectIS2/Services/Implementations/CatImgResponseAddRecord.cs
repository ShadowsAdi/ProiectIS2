namespace ProiectIS2.Services.Implementations;

public class CatImgResponseAddRecord
{
    public int ResponseCode { get; set; }
    
    public IFormFile Data { get; set; } = null!;
}