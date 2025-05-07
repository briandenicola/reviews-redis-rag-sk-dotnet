#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Console = Spectre.Console.AnsiConsole;
using Spectre.Console;
using Reviews.RedisRag.SK.Dotnet.Services;
using Reviews.RedisRag.SK.Dotnet.DependencyInjection;

namespace Reviews.RedisRag.SK.Dotnet;

internal class Program
{
    async static Task Main()
    {
        Console.Write(new FigletText("Redis Reviews").Color(Color.Red));


        var config = GetConfiguration();

        var azureOpenAIConfig = config.GetSection("AzureOpenAI").Get<AzureOpenAIConfiguration>();
        var redisConfig = config.GetSection("Redis").Get<RedisConfiguration>();

        ServiceCollection services = new();
        services.AddRedisRagServices(config, azureOpenAIConfig, redisConfig);
        var serviceProvider = services.BuildServiceProvider();

        const string vectorize = "1.\tVectorize the review(s) and store it in Redis";
        const string search = "2.\tAsk AI Assistant (search for a review by asking a question)";
        const string exit = "3.\tExit this Application";

        while (true)
        {

            var selectedOption = Console.Prompt(
                  new SelectionPrompt<string>()
                      .Title("Select an option to continue")
                      .PageSize(3)
                      .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                      .AddChoices([vectorize ,search, exit]));


            switch (selectedOption)
            {
                case vectorize:
                    await VectorizeAsync(serviceProvider.GetRequiredService<IDataLoaderService>());
                    break;
                case search:
                    await PerformSearch(serviceProvider.GetRequiredService<ISearchService>());
                    break;
                case exit:
                    return;
            }
        }
    }

    private static async Task PerformSearch(ISearchService searchService)
    {
        string userQuery = Console.Prompt(
              new TextPrompt<string>("Type your question, hit enter when ready.")
                  .PromptStyle("teal")
          );
        Console.WriteLine();
        await Console.Status()
               .StartAsync("Processing...", async ctx =>
               {
                   ctx.Spinner(Spinner.Known.Star);
                   ctx.SpinnerStyle(Style.Parse("green"));
                   var progress = new Progress<string>(status => ctx.Status(status));
                   var response = await searchService.SearchAsync(userQuery, progress);
                   Console.WriteLine(response.ToString());
                   Console.WriteLine();
               });
    }

    private static async Task VectorizeAsync(IDataLoaderService dataLoader)
    {
        await Console.Status()
               .StartAsync("Processing...", async ctx =>
               {
                   ctx.Spinner(Spinner.Known.Star);
                   ctx.SpinnerStyle(Style.Parse("green"));
                   var progress = new Progress<string>(status => ctx.Status(status));
                   await dataLoader.VectorizeAndLoadDataAsync(progress);
               });
    }

    private static IConfigurationRoot GetConfiguration()
    {
        var configuration = new ConfigurationBuilder()
                                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                            .AddUserSecrets<Program>()
                                            .AddEnvironmentVariables(prefix: "BJD_");

        var config = configuration.Build();
        return config;
    }
}
