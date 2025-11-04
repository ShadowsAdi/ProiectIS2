namespace ProiectIS2.Database.Seeders;

public class DatabaseSeeder
{
    public async Task DownloadData()
    {
        // TODO Download and insert the datasets in DB, not file
        string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Datasets/CatFacts");
        Directory.CreateDirectory(outputDir);
        
        using var client = new HttpClient();
        
        const string url = "https://huggingface.co/ngxson/demo_simple_rag_py/raw/main/cat-facts.txt";
        var filePath = Path.Combine(outputDir, "cat-facts.txt");

        try
        {
            var response = await client.GetAsync(url);
 
            await using var stream = await response.Content.ReadAsStreamAsync();
            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await stream.CopyToAsync(fileStream);
            Console.WriteLine("Saved");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}