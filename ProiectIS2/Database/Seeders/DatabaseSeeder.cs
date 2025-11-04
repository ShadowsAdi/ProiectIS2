using Microsoft.EntityFrameworkCore;

namespace ProiectIS2.Database.Seeders;

using Contexts;
using Models;
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
        var filePath = Path.Combine(outputDir, "cat-facts.txt");

        try
        {
            var response = await client.GetAsync(url);
            
            if(!response.IsSuccessStatusCode)
                throw new Exception("Couldn't retrieve the facts.");
            
            var text = await response.Content.ReadAsStringAsync();
            
            var facts = text.Split('\n');
            
            var catFacts = facts.Select(f => new Facts()
            {
                Fact = f.Trim(),
                SpecialType = false
            }).ToList();
            
            _context?.CatFacts.AddRangeAsync(catFacts);
            if (_context != null) await _context.SaveChangesAsync();
            Console.WriteLine($"Inserted {catFacts.Count} cat facts into the database.");

            /*await using var stream = await response.Content.ReadAsStreamAsync();
            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await stream.CopyToAsync(fileStream);
            Console.WriteLine("Saved");*/
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

        foreach (var code in httpCodes)
        {
            var url = $"https://http.cat/{code}.jpg";

            try
            {
                var exists = _context != null 
                              && await _context.CatImgResponses.AnyAsync(resp => 
                                  resp.ResponseCode == code);
                if (exists)
                    continue;

                var bytes = await httpClient.GetByteArrayAsync(url);

                var catImage = new CatImgResponses
                {
                    ResponseCode = code,
                    Data = bytes
                };

                _context!.CatImgResponses.Add(catImage);

                Console.WriteLine($"Inserted {code}.jpg ({bytes.Length} bytes)");
            }
            catch (HttpRequestException)
            {
                Console.WriteLine($"Not found {code}");
            }
        }

        await _context!.SaveChangesAsync();
        Console.WriteLine("HTTP cat images done.");
    }
}