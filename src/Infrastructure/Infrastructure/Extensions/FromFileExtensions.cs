namespace Infrastructure.Extensions;

using Microsoft.AspNetCore.Http;
using System.Text;

public static class FromFileExtensions
{
    public static async Task<string> ReadAsStringAsync(this IFormFile file)
    {
        if (file is null)
        {
            throw new ArgumentNullException(nameof(file));
        }
        
        var result = new StringBuilder();
        using (var reader = new StreamReader(file.OpenReadStream()))
        {
            while (reader.Peek() >= 0)
            {
                var line = await reader.ReadLineAsync();
                result.Append(line);
            }
        }
        return result.ToString();
    }
}