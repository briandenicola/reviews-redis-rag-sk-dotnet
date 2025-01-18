using Microsoft.Extensions.VectorData;

namespace Reviews.RedisRag.SK.Dotnet.Models;

/// <summary>
/// Represents a review for a product.
/// </summary>
internal class Review
{
    /// <summary>
    /// Gets or sets the unique identifier for the review.
    /// </summary>
    [VectorStoreRecordKey]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product identifier associated with the review.
    /// </summary>
    [VectorStoreRecordData]
    public string ProductId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user identifier who wrote the review.
    /// </summary>
    [VectorStoreRecordData]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the profile name of the user who wrote the review.
    /// </summary>
    [VectorStoreRecordData]
    public string ProfileName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the numerator of the helpfulness score.
    /// </summary>
    [VectorStoreRecordData]
    public int HelpfulnessNumerator { get; set; }

    /// <summary>
    /// Gets or sets the denominator of the helpfulness score.
    /// </summary>
    [VectorStoreRecordData]
    public int HelpfulnessDenominator { get; set; }

    /// <summary>
    /// Gets or sets the score of the review.
    /// </summary>
    [VectorStoreRecordData]
    public int Score { get; set; }

    /// <summary>
    /// Gets or sets the time the review was written, represented as a Unix timestamp.
    /// </summary>
    [VectorStoreRecordData]
    public long Time { get; set; }

    /// <summary>
    /// Gets or sets the summary of the review.
    /// </summary>
    [VectorStoreRecordData]
    public string Summary { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full text of the review.
    /// </summary>
    [VectorStoreRecordData]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the combined text of the review, which may include additional data.
    /// </summary>
    [VectorStoreRecordData]
    public string Combined { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the embedding vector for the review.
    /// </summary>
    [VectorStoreRecordVector(1536)]
    public ReadOnlyMemory<float> Embedding { get; set; } = Array.Empty<float>();
}
