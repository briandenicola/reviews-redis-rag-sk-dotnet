#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

using Microsoft.SemanticKernel.Embeddings;
using Reviews.RedisRag.SK.Dotnet.Models;

namespace Reviews.RedisRag.SK.Dotnet.Services;

/// <summary>
/// Service to load data into Redis
/// </summary>
internal class DataLoaderService : IDataLoaderService
{
    private readonly IFileReaderService _fileReader;
    private readonly IRedisDBService _redisDBService;
    private readonly ITextEmbeddingGenerationService _embeddingService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataLoaderService"/> class.
    /// </summary>
    /// <param name="fileReader">The file reader service.</param>
    /// <param name="redisDBService">The Redis DB service.</param>
    /// <param name="embeddingService">The text embedding generation service.</param>
    public DataLoaderService(
        IFileReaderService fileReader,
        IRedisDBService redisDBService,
        ITextEmbeddingGenerationService embeddingService)
    {
        _fileReader = fileReader;
        _redisDBService = redisDBService;
        _embeddingService = embeddingService;
    }

    /// <summary>
    /// Reads reviews from CSV file, vectorizes them and loads them into Redis.
    /// </summary>
    /// <param name="progress">The progress reporter.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task VectorizeAndLoadDataAsync(IProgress<string> progress)
    {
        progress.Report("Reading reviews from CSV file...");
        var reviews = await _fileReader.ReadAsync(Path.Combine("assets", "reviews.csv"));
        progress.Report("Reading reviews from CSV file - Done");

        progress.Report("Flushing db...");
        await _redisDBService.FlushAsync();
        progress.Report("Flushing db - Done");

        progress.Report("Vectorizing & upserting...");
        await VectorizeAndUpsertReviewAsync(reviews, progress);
        progress.Report("Vectorizing & upserting - Done");
    }

    /// <summary>
    /// Vectorizes the reviews and upserts them into Redis.
    /// </summary>
    /// <param name="reviews">The list of reviews.</param>
    /// <param name="progress">The progress reporter.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task VectorizeAndUpsertReviewAsync(List<Review> reviews, IProgress<string> progress)
    {
        var tasks = reviews.Select(async review =>
        {
            review.Combined = $"productid: {review.ProductId} score: {review.Score} text: {review.Text}";
            progress.Report($"Vectorizing review for product {review.ProductId}...");
            review.Embedding = await _embeddingService.GenerateEmbeddingAsync(review.Combined);
            progress.Report($"Vectorizing review for product {review.ProductId} - Done");
        });

        await Task.WhenAll(tasks);

        await _redisDBService.UpsertAsync(reviews);
    }
}