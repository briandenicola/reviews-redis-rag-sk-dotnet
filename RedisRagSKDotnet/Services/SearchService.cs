#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Embeddings;
using Reviews.RedisRag.SK.Dotnet.Models;
using System.Text;

namespace Reviews.RedisRag.SK.Dotnet.Services;

/// <summary>
/// Service for performing search operations.
/// </summary>
internal class SearchService : ISearchService
{
    private readonly ITextEmbeddingGenerationService _embeddingService;
    private readonly IRedisDBService _dbService;
    private readonly IChatCompletionService _chatService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchService"/> class.
    /// </summary>
    /// <param name="embeddingService">The text embedding generation service.</param>
    /// <param name="dbService">The Redis database service.</param>
    /// <param name="chatService">The chat completion service.</param>
    public SearchService(
        ITextEmbeddingGenerationService embeddingService,
        IRedisDBService dbService,
        IChatCompletionService chatService)
    {
        _embeddingService = embeddingService;
        _dbService = dbService;
        _chatService = chatService;
    }

    /// <summary>
    /// Asynchronously performs a search operation based on the provided query.
    /// </summary>
    /// <param name="query">The search query string.</param>
    /// <param name="progress">An object to report progress updates.</param>
    /// <returns>A task that represents the asynchronous search operation. The task result contains the search result as a string.</returns>
    public async Task<string> SearchAsync(string query, IProgress<string> progress)
    {
        progress.Report("Generating embedding for the user query...");
        var searchVector = await _embeddingService.GenerateEmbeddingAsync(query);
        progress.Report("Generating embedding for the user query - Done");

        progress.Report("Vector search for the reviews based on the user query...");
        var searchResult = await _dbService.VectorizedSearchAsync(searchVector);
        progress.Report("Vector search for the reviews based on the user query - Done");

        progress.Report("Generating chat completion for the user query...");
        var response = await GenerateChatCompletionAsync(searchResult, query);
        progress.Report("Generating chat completion for the user query - Done");
        return response;
    }

    /// <summary>
    /// Generates a chat completion based on the search results and user query.
    /// </summary>
    /// <param name="searchResult">The search results containing reviews.</param>
    /// <param name="userQuery">The user's search query.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the chat completion response as a string.</returns>
    private async Task<string> GenerateChatCompletionAsync(IAsyncEnumerable<VectorSearchResult<Review>> searchResult, string userQuery)
    {
        var stringBuilder = new StringBuilder();
        await foreach (var record in searchResult)
        {
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"ProductId: {record.Record.ProductId} Score: {record.Score} Text: {record.Record.Text} ");
            stringBuilder.AppendLine();
        }

        string prompt = PreparePrompt(userQuery, stringBuilder);

        var response = await _chatService.GetChatMessageContentAsync(prompt);

        return response.ToString();
    }

    /// <summary>
    /// Prepares the prompt for the chat completion service.
    /// </summary>
    /// <param name="userQuery">The user's search query.</param>
    /// <param name="stringBuilder">The string builder containing the search results context.</param>
    /// <returns>The prepared prompt as a string.</returns>
    private static string PreparePrompt(string userQuery, StringBuilder stringBuilder)
    {
        return $@"
                    context :{stringBuilder}

                    Answer the below question based on the context above. 
                    Context is a set of product reviews. Summarize the reviews based on the context
                    Provide the product id associated with the answer as well. 
                    If the information to answer the question is not present in the given context then reply ""I don't know"".
                            
                    Question: {userQuery}
                    Answer: 
                ";
    }
}
