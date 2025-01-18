using Spectre.Console;

namespace Reviews.RedisRag.SK.Dotnet.Services;

/// <summary>
/// Defines the contract for a service that loads and vectorizes data.
/// </summary>
internal interface IDataLoaderService
{
    /// <summary>
    /// Vectorizes and loads data asynchronously.
    /// </summary>
    /// <param name="progress">The progress reporter to report the progress of the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task VectorizeAndLoadDataAsync(IProgress<string> progress);
}
