using Microsoft.Extensions.VectorData;
using Reviews.RedisRag.SK.Dotnet.Models;

namespace Reviews.RedisRag.SK.Dotnet.Services;

/// <summary>
/// Interface for Redis database service.
/// </summary>
internal interface IRedisDBService
{
    /// <summary>
    /// Flushes the Redis database asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous flush operation.</returns>
    Task FlushAsync();

    /// <summary>
    /// Upserts a list of reviews into the Redis database asynchronously.
    /// </summary>
    /// <param name="reviews">The list of <see cref="Review"> to upsert.</param>
    /// <returns>A task that represents the asynchronous upsert operation.</returns>
    Task UpsertAsync(List<Review> reviews);

    /// <summary>
    /// Performs a vectorized search in the Redis database asynchronously.
    /// </summary>
    /// <param name="searchVector">The search vector to use for the search.</param>
    /// <returns>A task that represents the asynchronous search operation, containing the search results.</returns>
    Task<VectorSearchResults<Review>> VectorizedSearchAsync(ReadOnlyMemory<float> searchVector);
}