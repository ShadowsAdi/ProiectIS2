using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProiectIS2.Contexts;
using ProiectIS2.Models.DataTransferObjects.FactsDTO;
using ProiectIS2.Models.DataTransferObjects.Pagination;
using ProiectIS2.Models.Domain;
using ProiectIS2.Services.Abstractions;

namespace ProiectIS2.Services.Implementations.FactsImpl;

public class FactsService(ApplicationDbContext context) : IObjectService<FactRecord, SearchPaginationQueryParams, FactAddRecord, FactUpdateRecord>
{
    public async Task<PagedResponse<FactRecord>> GetObjects(SearchPaginationQueryParams queryParams)
    {
        var search = !string.IsNullOrWhiteSpace(queryParams.Search) ? 
            queryParams.Search.Trim() : "";
        return new PagedResponse<FactRecord>()
        {
            Page = queryParams.Page,
            PageSize = queryParams.PageSize,
            TotalCount = await context.Set<Facts>().CountAsync(e => e.Fact.Contains(search)),
            Items = await context.Set<Facts>()
                .Where(e => e.Fact.Contains(search) && e.DeletedAt == null)
                .Skip((queryParams.Page - 1))
                .Take(queryParams.PageSize)
                .Select(e => new FactRecord()
                {
                    Id = e.Id,
                    FactText = e.Fact,
                    SpecialType = e.SpecialType,
                    CategoryId = e.CategoryId,
                })
            .ToListAsync()
        };
    }

    public async Task<FactRecord?> GetObject(int objectId)
    {
        return await context.Set<Facts>()
            .Where(e=>e.DeletedAt == null)
            .Select(e => new FactRecord()
            {
                Id = e.Id,
                FactText = e.Fact,
                SpecialType = e.SpecialType,
                CategoryId = e.CategoryId,
            })
            .FirstOrDefaultAsync(e => e.Id == objectId);
    }

    public async Task<int> AddObject(FactAddRecord obj)
    {
        var entity = new Facts()
        {
            Fact = obj.Fact,
            CategoryId = obj.CategoryId, 
            SpecialType = obj.SpecialType,
            CreatedAt = DateTime.UtcNow 
        };
    
        await context.Set<Facts>().AddAsync(entity);
        await context.SaveChangesAsync();
        
        return entity.Id;
    }

    public async Task UpdateObject(FactUpdateRecord obj)
    {
        var entity = await context.Set<Facts>()
            .FirstAsync(e => e.Id == obj.Id);
        
        entity.Fact = obj.FactText.IsNullOrEmpty() ? entity.Fact : obj.FactText;
        entity.SpecialType = obj.SpecialType ?? entity.SpecialType;
        entity.CategoryId = obj.CategoryId ?? entity.CategoryId;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.DeletedAt = obj.DeletedAt;
        
        context.Set<Facts>().Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteObject(int objectId)
    {
        var entity = await context.Set<Facts>()
            .FirstAsync(e => e.Id == objectId);
        
        context.Set<Facts>().Remove(entity);
        await context.SaveChangesAsync();
    }
}