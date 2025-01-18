using Reviews.RedisRag.SK.Dotnet.Models;

namespace Reviews.RedisRag.SK.Dotnet.Services;

/// <summary>
/// Interface for reading a file asynchronously
/// </summary>
internal interface IFileReaderService
{
    /// <summary>
    /// Reads the file asynchronously and returns a list of reviews.
    /// </summary>
    /// <param name="filePath">The path to the file to be read.</param>
    /// <returns>A task that represents the asynchronous read operation. The task result contains a list of reviews.</returns>
    Task<List<Review>> ReadAsync(string filePath);
}
