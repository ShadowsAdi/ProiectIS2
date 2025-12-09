using ProiectIS2.Models.DataTransferObjects.Pagination;

namespace ProiectIS2.Services.Abstractions;

public interface IObjectService<T, TQueryParams, TY, TZ>
{
    public Task<PagedResponse<T>> GetObjects(TQueryParams queryParams);
    
    public Task<T?> GetObject(int objectId);

    public Task<int> AddObject(TY obj);
    public Task UpdateObject(TZ obj);
    public Task DeleteObject(int objectId);
}