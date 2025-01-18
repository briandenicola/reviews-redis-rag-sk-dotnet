#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Reviews.RedisRag.SK.Dotnet.Services;
using StackExchange.Redis;

namespace Reviews.RedisRag.SK.Dotnet.DependencyInjection;


internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Redis RAG services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <param name="config">The configuration root.</param>
    /// <param name="azureOpenAIConfig">The Azure OpenAI configuration.</param>
    /// <param name="redisConfig">The Redis configuration.</param>
    /// <returns>The IServiceCollection with the added services.</returns>
    public static IServiceCollection AddRedisRagServices(this IServiceCollection services,
        IConfigurationRoot config,
        AzureOpenAIConfiguration? azureOpenAIConfig,
        RedisConfiguration? redisConfig)
    {
        services.AddSingleton<IConfiguration>(config);
        services.AddSingleton(sp => ConnectionMultiplexer.Connect($"{redisConfig!.Host}:{redisConfig.Port},password={redisConfig.Password}").GetDatabase());
        services.AddAzureOpenAITextEmbeddingGeneration(azureOpenAIConfig!.EmbeddingDeployment, azureOpenAIConfig.Endpoint, azureOpenAIConfig.Key);
        services.AddAzureOpenAIChatCompletion(azureOpenAIConfig.CompletionsDeployment, azureOpenAIConfig.Endpoint, azureOpenAIConfig.Key);
        services.AddRedisVectorStore();
        services.AddTransient<Kernel>();
        services.AddSingleton<IFileReaderService, CsvFileReaderService>();
        services.AddSingleton<IRedisDBService, RedisDBService>();
        services.AddSingleton<IDataLoaderService, DataLoaderService>();
        services.AddSingleton<ISearchService, SearchService>();
        return services;
    }
}
