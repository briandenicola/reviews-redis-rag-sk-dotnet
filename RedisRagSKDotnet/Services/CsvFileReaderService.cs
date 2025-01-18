using System.Globalization;
using CsvHelper;
using Reviews.RedisRag.SK.Dotnet.Models;

namespace Reviews.RedisRag.SK.Dotnet.Services;

/// <summary>
/// A service to read CSV files
/// </summary>
internal class CsvFileReaderService : IFileReaderService
{
    
    /// <summary>
    /// Reads a CSV file asynchronously and returns a list of reviews.
    /// </summary>
    /// <param name="filePath">The path to the CSV file.</param>
    /// <returns>A task that represents the asynchronous read operation. The task result contains a list of reviews.</returns>
    public async Task<List<Review>> ReadAsync(string filePath)
    {
        var reader = new StreamReader(filePath);
        var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<ReviewMap>();
        var reviews = csv.GetRecordsAsync<Review>();
        var reviewList = new List<Review>();
        await foreach (var review in reviews)
        {
            reviewList.Add(review);
        }
        return reviewList;
    }
}
