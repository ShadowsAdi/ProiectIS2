using ProiectIS2.Models.DTOs;
using ProiectIS2.Models.DTOs.Pagination;

namespace ProiectIS2.Services.Abstractions;

public interface IObjectService<T, TQueryParams, TY, TZ>
{
    public Task<PagedResponse<T>> GetObjects(TQueryParams queryParams);
    
    public Task<T?> GetObject(int objectId);

    public Task AddObject(TY obj);
    public Task UpdateObject(TZ obj);
    public Task DeleteObject(int objectId);
}