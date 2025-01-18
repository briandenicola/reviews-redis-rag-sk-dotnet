namespace Reviews.RedisRag.SK.Dotnet.Services;

/// <summary>
/// Configuration settings for Azure OpenAI service.
/// </summary>
internal class AzureOpenAIConfiguration
{
    /// <summary>
    /// Gets or sets the endpoint URL for the Azure OpenAI service.
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the API key for accessing the Azure OpenAI service.
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the deployment name for the embedding model.
    /// </summary>
    public string EmbeddingDeployment { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the deployment name for the completions model.
    /// </summary>
    public string CompletionsDeployment { get; set; } = string.Empty;
}
