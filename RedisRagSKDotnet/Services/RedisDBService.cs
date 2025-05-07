using Microsoft.Extensions.VectorData;
using Reviews.RedisRag.SK.Dotnet.Models;
using StackExchange.Redis;

namespace Reviews.RedisRag.SK.Dotnet.Services;


/// <summary>
/// Service for interacting with Redis database.
/// </summary>
internal class RedisDBService : IRedisDBService
{
    private const string FlusgDBCommand = "FLUSHDB";
    private const string CollectionName = "sk-reviews";
    private readonly IDatabase _database;
    private readonly IVectorStoreRecordCollection<string, Review> _collection;

    /// <summary>
    /// Initializes a new instance of the <see cref="RedisDBService"/> class.
    /// </summary>
    /// <param name="database">The Redis database instance.</param>
    /// <param name="vectorStore">The vector store instance.</param>
    public RedisDBService(IDatabase database, IVectorStore vectorStore)
    {
        _database = database;
        _collection = vectorStore.GetCollection<string, Review>(CollectionName);
    }

    /// <summary>
    /// Flushes the Redis database asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous flush operation.</returns>
    public async Task FlushAsync()
    {
        await _database.ExecuteAsync(FlusgDBCommand);
    }

    /// <summary>
    /// Upserts a list of reviews into the Redis database asynchronously.
    /// </summary>
    /// <param name="reviews">The list of <see cref="Review"/> to upsert.</param>
    /// <returns>A task that represents the asynchronous upsert operation.</returns>
    public async Task UpsertAsync(List<Review> reviews)
    {
        await GetOrCreateCollectionAsync();
        reviews.ForEach(async review => await _collection.UpsertAsync(review));
    }

    /// <summary>
    /// Performs a vectorized search in the Redis database asynchronously.
    /// </summary>
    /// <param name="searchVector">The search vector to use for the search.</param>
    /// <returns>A task that represents the asynchronous search operation, containing the search results.</returns>
    public async Task<IAsyncEnumerable<VectorSearchResult<Review>>> VectorizedSearchAsync(ReadOnlyMemory<float> searchVector)
    {
        var options = new VectorSearchOptions<Review>
        {
            VectorProperty = r => r.Embedding
        };

        await GetOrCreateCollectionAsync();
        var results = _collection.SearchEmbeddingAsync(searchVector, top: 3, options);
        return results;
    }

    /// <summary>
    /// Gets or creates the collection asynchronously.
    /// </summary>
    /// <returns>The collection of reviews.</returns>
    private async Task<IVectorStoreRecordCollection<string, Review>> GetOrCreateCollectionAsync()
    {
        await _collection.CreateCollectionIfNotExistsAsync();
        return _collection;
    }
}
