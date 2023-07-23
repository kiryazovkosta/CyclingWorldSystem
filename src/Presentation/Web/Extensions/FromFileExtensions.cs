namespace Web.Extensions;

using Microsoft.AspNetCore.Http;
using System.Text;

public static class FromFileExtensions
{
    public static async Task<string> ReadAsStringAsync(this IFormFile file)
    {
        var result = new StringBuilder();
        using (var reader = new StreamReader(file.OpenReadStream()))
        {
            while (reader.Peek() >= 0)
                result.AppendLine(await reader.ReadLineAsync());
        }
        return result.ToString();
    }
}