namespace ProiectIS2.Models.DataTransferObjects.Pagination;

public class PaginationQueryParams
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}