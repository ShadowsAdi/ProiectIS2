using Microsoft.EntityFrameworkCore;
using ProiectIS2.Models.Domain;

namespace ProiectIS2.Database.Seeders;

using Contexts;

public class DatabaseSeeder
{
    private readonly ApplicationDbContext? _context;

    public DatabaseSeeder(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task DownloadData()
    {
        await this.DownloadFacts();
        await DownloadHttpCats();
    }
    
    public async Task DownloadFacts()
    {
        // TODO Download and insert the datasets in DB, not file. Done
        var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Datasets/CatFacts");
        Directory.CreateDirectory(outputDir);
        
        var client = new HttpClient();
        
        const string url = "https://huggingface.co/ngxson/demo_simple_rag_py/raw/main/cat-facts.txt";

        try
        {
            var response = await client.GetAsync(url);
            
            if(!response.IsSuccessStatusCode)
                throw new Exception("Couldn't retrieve the facts.");
            
            var text = await response.Content.ReadAsStringAsync();
            
            var facts = text.Split('\n');

            var category = new Random();
            
            var catFacts = facts.Select(f => new Facts()
            {
                Fact = f.Trim(),
                SpecialType = false,
                CategoryId = category.Next(1, 4) 
            }).ToList();
            
            _context?.CatFacts.AddRangeAsync(catFacts);
            if (_context != null) await _context.SaveChangesAsync();
            Console.WriteLine($"Inserted {catFacts.Count} cat facts into the database.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    
    public async Task DownloadHttpCats()
    {
        using var httpClient = new HttpClient();

        int[] httpCodes =
        [
            100, 101, 102,
            200, 201, 202, 203, 204, 205, 206, 207, 208, 226,
            300, 301, 302, 303, 304, 305, 307, 308,
            400, 401, 402, 403, 404, 405, 406, 407, 408, 409,
            410, 411, 412, 413, 414, 415, 416, 417, 418, 421,
            422, 423, 424, 425, 426, 428, 429, 431, 451,
            500, 501, 502, 503, 504, 505, 506, 507, 508, 510, 511
        ];
        
        const string responseDirectory = "./responses";
        Directory.CreateDirectory(responseDirectory);

        foreach (var code in httpCodes)
        {
            var fileName = $"{code}.jpg";
            var url = $"https://http.cat/{fileName}";
            
            
            var filePath = Path.Combine(responseDirectory, fileName); 

            try
            {
                var exists = _context != null 
                             && await _context.CatImgResponses.AnyAsync(resp => 
                                 resp.ResponseCode == code);
                if (exists)
                {
                    Console.WriteLine($"Skipping {code}. Database entry already exists.");
                    continue;
                }

                var bytes = await httpClient.GetByteArrayAsync(url);
        
                await File.WriteAllBytesAsync(filePath, bytes);

                var catImage = new CatImgResponses
                {
                    ResponseCode = code,
                    Data = filePath 
                };

                _context!.CatImgResponses.Add(catImage);

                Console.WriteLine($"Inserted {code}.jpg. Stored file path: {filePath}");
            }
            catch (HttpRequestException)
            {
                Console.WriteLine($"Not found {code}");
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                Console.WriteLine($"Error saving file for {code}: {ex.Message}");
            }
        }

        await _context!.SaveChangesAsync();
        Console.WriteLine("HTTP cat images done.");
    }
}