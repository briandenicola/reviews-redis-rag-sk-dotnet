namespace Reviews.RedisRag.SK.Dotnet.Services;

/// <summary>
/// Represents the configuration settings for connecting to a Redis server.
/// </summary>
internal class RedisConfiguration
{
    /// <summary>
    /// Gets or sets the Redis server host.
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Redis server port.
    /// </summary>
    public string Port { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password for the Redis server.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
