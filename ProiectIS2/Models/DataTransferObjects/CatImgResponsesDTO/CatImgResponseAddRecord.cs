namespace ProiectIS2.Models.DataTransferObjects.CatImgResponsesDTO;

public class CatImgResponseAddRecord
{
    public int ResponseCode { get; set; }
    
    public IFormFile Data { get; set; } = null!;
}