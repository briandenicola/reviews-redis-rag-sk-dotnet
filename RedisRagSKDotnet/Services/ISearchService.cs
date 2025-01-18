namespace Reviews.RedisRag.SK.Dotnet.Services;


/// <summary>
/// Defines the contract for search services.
/// </summary>
internal interface ISearchService
{
    /// <summary>
    /// Asynchronously performs a search operation based on the provided query.
    /// </summary>
    /// <param name="query">The search query string.</param>
    /// <param name="progress">An object to report progress updates.</param>
    /// <returns>A task that represents the asynchronous search operation. The task result contains the search result as a string.</returns>
    Task<string> SearchAsync(string query, IProgress<string> progress);
}
