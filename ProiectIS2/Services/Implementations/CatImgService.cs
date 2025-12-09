using Microsoft.EntityFrameworkCore;
using ProiectIS2.Contexts;
using ProiectIS2.Models.Domain;
using ProiectIS2.Models.DTOs;
using ProiectIS2.Models.DTOs.Pagination;
using ProiectIS2.Services.Abstractions;

namespace ProiectIS2.Services.Implementations;

public class CatImgService(ApplicationDbContext context) : IObjectService<CatImgResponsesRecord, PaginationQueryParams, CatImgResponseAddRecord, CatImgResponseUpdateRecord>
{
    public async Task<PagedResponse<CatImgResponsesRecord>> GetObjects(PaginationQueryParams queryParams)
    {
        return new PagedResponse<CatImgResponsesRecord>()
        {
            Page = queryParams.Page,
            PageSize = queryParams.PageSize,
            TotalCount = await context.Set<CatImgResponses>().CountAsync(),
            Items = await context.Set<CatImgResponses>()
 
                .Skip((queryParams.Page - 1))
                .Take(queryParams.PageSize)
                .Select(e => new CatImgResponsesRecord()
                {
                    ResponseCode = e.ResponseCode,
                    DataLocation = e.Data,
                })
            .ToListAsync()
        };
    }

    public async Task<CatImgResponsesRecord?> GetObject(int objectId)
    {
        return await context.Set<CatImgResponses>()
            .Select(e => new CatImgResponsesRecord
            {
                ResponseCode = e.ResponseCode,
                DataLocation = e.Data,
            })
            .FirstOrDefaultAsync(e => e.ResponseCode == objectId);
    }

    public async Task AddObject(CatImgResponseAddRecord obj)
    {
        if (await context.Set<CatImgResponses>().AnyAsync(e => e.ResponseCode == obj.ResponseCode))
        {
            throw new InvalidOperationException($"CatImgResponse with code {obj.ResponseCode} already exists.");
        }
    
        const string responseDirectory = "./responses";
        Directory.CreateDirectory(responseDirectory);

        var fileName = $"{obj.ResponseCode}.jpg";
        var filePath = Path.Combine(responseDirectory, fileName);
    
        byte[] fileBytes;
        using (var memoryStream = new MemoryStream())
        {
            await obj.Data.CopyToAsync(memoryStream);
            fileBytes = memoryStream.ToArray();
        }
    
        await File.WriteAllBytesAsync(filePath, fileBytes);
    
        var entity = new CatImgResponses()
        {
            ResponseCode = obj.ResponseCode,
            Data = filePath, 
            CreatedAt = DateTime.UtcNow 
        };
    
        await context.Set<CatImgResponses>().AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateObject(CatImgResponseUpdateRecord obj)
    {
        var entity = await context.Set<CatImgResponses>()
            .FirstOrDefaultAsync(e => e.ResponseCode == obj.ResponseCode);

        if (entity == null)
        {
            throw new KeyNotFoundException($"CatImgResponse with code {obj.ResponseCode} not found.");
        }

        if (obj.Data.Length == 0)
        {
            Console.WriteLine($"No file provided for update of {obj.ResponseCode}. Skipping file update.");
            return; 
        }

        string filePath = entity.Data; 

        if (string.IsNullOrEmpty(filePath))
        {
             throw new InvalidOperationException($"Existing file path for {obj.ResponseCode} is null or empty. Cannot overwrite file.");
        }

        byte[] newFileBytes;
        using (var memoryStream = new MemoryStream())
        {
            await obj.Data.CopyToAsync(memoryStream); 
            newFileBytes = memoryStream.ToArray();
        }

        try
        {
            await File.WriteAllBytesAsync(filePath, newFileBytes);
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error saving file for {obj.ResponseCode} to {filePath}: {ex.Message}");
            throw new Exception("File system error during update.", ex);
        }
        
        await context.SaveChangesAsync();
    }

    public async Task DeleteObject(int objectId)
    {
        var entity = await context.Set<CatImgResponses>()
            .FirstAsync(e => e.ResponseCode == objectId);
        
        context.Set<CatImgResponses>().Remove(entity);
        await context.SaveChangesAsync();
    }
}