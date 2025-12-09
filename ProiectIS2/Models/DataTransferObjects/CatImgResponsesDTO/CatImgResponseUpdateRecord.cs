namespace ProiectIS2.Models.DataTransferObjects.CatImgResponsesDTO;

public class CatImgResponseUpdateRecord
{
    public int ResponseCode { get; set; } 
    
    public IFormFile Data { get; set; } = null!;
}